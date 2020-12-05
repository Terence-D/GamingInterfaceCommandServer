using GIC.Common;
using GIC.Common.Services;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GIC.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isRunning = false;
        private readonly IConfigurationService configurationService;

        public MainWindow(IConfigurationService configurationService)
        {
            this.configurationService = configurationService;
            InitializeComponent();
            LoadSettings();
        }

        private static readonly Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            return !regex.IsMatch(text);
        }

        private void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string text = (string)e.DataObject.GetData(typeof(string));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private new void PreviewTextInput (object sender, TextCompositionEventArgs e) {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Password.Length < 6)
                MessageBox.Show("Please use a password at least 6 characters long.");
            else if (txtTarget.Text.Length <= 0)
            {
                MessageBox.Show("Ensure you set the target of the application you want to send commands to.");
            }
            else
            {
                ToggleServer();
            }
        }

        private void LoadSettings()
        {
            foreach (string entry in configurationService.Applications)
            {
                txtTarget.Items.Add(entry);
            }
            txtTarget.Text = txtTarget.Items[0].ToString();
            txtPassword.Password = Crypto.Decrypt(configurationService.Password);
            txtPort.Text = configurationService.Port.ToString();
        }

        private void SaveSettings()
        {
            configurationService.Password = Crypto.Encrypt(txtPassword.Password);

            ushort port;
            if (ushort.TryParse(txtPort.Text, out port))
                configurationService.Port = port;

            List<string> apps = new List<string>();
            int i = 0;
            foreach (string entry in txtTarget.Items)
            {
                if (entry.Equals(txtTarget.Text))
                    configurationService.SelectedApp = i;
                apps.Add(entry);
                i++;
            }
            if (!apps.Contains(txtTarget.Text))
            {
                txtTarget.Items.Add(txtTarget.Text);
                apps.Add(txtTarget.Text);
                configurationService.SelectedApp = i;
            }

            configurationService.Applications = apps;
        }

        private void ToggleServer()
        {
            if (isRunning)
            {
                RestApi.Program.Stop();
                isRunning = !isRunning;
                txtPassword.IsEnabled = true;
                txtPort.IsEnabled = true;
                txtTarget.IsEnabled = true;
            }
            else
            {
                ushort port;
                if (!ushort.TryParse(txtPort.Text, out port))
                {
                    MessageBox.Show("Please enter a port between 1 and 65535");
                } else
                {
                    string[] args = new string[]
                    {
                    "--port",
                    port.ToString(),
                    "--password",
                    txtPassword.Password,
                    "--app",
                    txtTarget.Text,
                    };
                    SaveSettings();
                    StartWeb(args);
                    isRunning = true;
                    txtPassword.IsEnabled = false;
                    txtPort.IsEnabled = false;
                    txtTarget.IsEnabled = false;
                }

            }
            ToggleText();
        }

        private void StartWeb(string[] args)
        {
            Task.Run(() => RestApi.Program.Start(args));
        }

        private void ToggleText()
        {
            if (!isRunning)
                btnStart.Content = "Start";
            else
                btnStart.Content = "Stop";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            About window = new About();
            window.Show();
        }
    }
}
