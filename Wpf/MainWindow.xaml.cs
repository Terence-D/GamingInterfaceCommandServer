using GIC.Common;
using GIC.Common.Services;
using System.Windows;

namespace GIC.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private MainController controller;
        private bool isRunning = false;
        private readonly IConfigurationService configurationService;

        public MainWindow(IConfigurationService configurationService)
        {
            this.configurationService = configurationService;
            InitializeComponent();
            LoadSettings();
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
                SaveSettings();
            }
        }

        private void LoadSettings()
        {
            foreach (string entry in configurationService.Applications)
            {
                txtTarget.Items.Add(entry);
            }
            txtPassword.Password = Crypto.Decrypt(configurationService.Password);
            txtPort.Text = configurationService.Port;
        }

        private void SaveSettings()
        {
            configurationService.Password = txtPassword.Password;
            configurationService.Port = txtPort.Text;
            if (!configurationService.Applications.Contains(txtTarget.Text))
                configurationService.Applications.Add(txtTarget.Text);
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
                string[] args = new string[]
                {
                    "--port",
                    txtPort.Text,
                    "--password",
                    txtPassword.Password,
                    "--app",
                    txtTarget.Text,
                };
                isRunning = RestApi.Program.Start(args);
                txtPassword.IsEnabled = false;
                txtPort.IsEnabled = false;
                txtTarget.IsEnabled = false;
            }
            toggleText();
        }

        private void toggleText()
        {
            if (!isRunning)
                btnStart.Content = "Start";
            else
                btnStart.Content = "Stop";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //About window = new About();
            //window.Show();
        }
    }
}
