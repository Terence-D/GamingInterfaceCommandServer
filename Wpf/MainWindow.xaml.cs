using GIC.Common;
using GIC.Common.Services;
using System.Collections.Generic;
using System.Diagnostics;
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
            WindowHandles();
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

        private new void PreviewTextInput(object sender, TextCompositionEventArgs e) {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void LoadSettings()
        {
            foreach (string entry in configurationService.Applications)
            {
                lstApps.Items.Add(entry);
            }
            lstApps.SelectedIndex = configurationService.SelectedApp;
            txtPassword.Password = Crypto.Decrypt(configurationService.Password);
            txtPort.Text = configurationService.Port.ToString();
        }

        private void SaveSettings()
        {
            configurationService.Password = Crypto.Encrypt(txtPassword.Password);

            ushort port;
            if (ushort.TryParse(txtPort.Text, out port))
                configurationService.Port = port;

            SaveAppList();
        }

        private void SaveAppList()
        {
            List<string> apps = new List<string>();

            configurationService.SelectedApp = lstApps.SelectedIndex;
            foreach (string entry in lstApps.Items)
                apps.Add(entry);
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
                lstApps.IsEnabled = true;
                cboApps.IsEnabled = true;
            }
            else
            {
                if (!ushort.TryParse(txtPort.Text, out ushort port))
                {
                    MessageBox.Show("Please enter a port between 1 and 65535");
                }
                else
                {
                    string[] args = new string[]
                    {
                        "--port",
                        port.ToString(),
                        "--password",
                        txtPassword.Password,
                        "--app",
                        lstApps.SelectedItem.ToString(),
                    };

                    SaveSettings();
                    StartWeb(args);
                    isRunning = true;
                    txtPassword.IsEnabled = false;
                    txtPort.IsEnabled = false;
                    lstApps.IsEnabled = false;
                    cboApps.IsEnabled = false;
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

        private void WindowHandles() {
            Process[] processlist = Process.GetProcesses();

            foreach (Process process in processlist)
            {
                if (!string.IsNullOrEmpty(process.MainWindowTitle))
                {
                    cboApps.Items.Add(process.MainWindowTitle);
                }
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Password.Length < 6)
                MessageBox.Show("Please use a password at least 6 characters long.");
            else if (lstApps.SelectedIndex == -1)
            {
                MessageBox.Show("Ensure you set the target of the application you want to send commands to.");
            }
            else
            {
                ToggleServer();
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (cboApps.Text.Length != 0)
                lstApps.Items.Add(cboApps.Text);
            SaveAppList();
        }

        private void cboApps_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
                lstApps.Items.Add(e.AddedItems[0].ToString());
            SaveAppList();
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (lstApps.SelectedIndex != -1)
            {
                lstApps.Items.Remove(lstApps.SelectedItem);
                lstApps.SelectedIndex = -1;
            }
            SaveAppList();
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            About window = new About();
            window.Show();
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            Help window = new Help();
            window.Show();
        }
    }
}
