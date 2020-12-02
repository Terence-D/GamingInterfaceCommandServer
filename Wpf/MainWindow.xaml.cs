using System;
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

        public MainWindow()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Password.Length < 6)
                System.Windows.MessageBox.Show("Please use a password at least 6 characters long.");
            else if (txtTarget.Text.Length <= 0)
            {
                System.Windows.MessageBox.Show("Ensure you set the target of the application you want to send commands to.");
            }
            else
            {
                int port = short.Parse(txtPort.Text);
                //SetApplication(txtTarget.Text);
                //SetPassword(txtPassword.Password);
                //SetPort(port);
                ToggleServer();
                //SaveSettings(txtTarget.Text, txtPassword.Password, port);
            }
        }

        private void LoadSettings()
        {
            txtTarget.Items.Add("Star Citizen");
            txtTarget.Items.Add("Elite - Dangerous (CLIENT)");
            //txtTarget.Text = Properties.Settings.Default.target;
            if (txtTarget.Text == "")
                txtTarget.Text = "Star Citizen";
            //if (Properties.Settings.Default.password.Length > 5)
            //    txtPassword.Password = CryptoHelper.Decrypt(Properties.Settings.Default.password);
            //int port = Properties.Settings.Default.port;
            //if (port == 0)
            //    port = 8091;
            //txtPort.Text = port.ToString();
        }

        private void SaveSettings(string target, string password, int port)
        {
            //Properties.Settings.Default.target = target;
            //String encrypted = CryptoHelper.Encrypt(password);
            //Properties.Settings.Default.password = encrypted;
            //Properties.Settings.Default.port = port;
            //Properties.Settings.Default.Save();
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
