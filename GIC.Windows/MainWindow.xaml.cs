using CommandLine;
using GIC.Common;
using GIC.Common.Services;
using GIC.RestApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GIC.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IListener
    {
        private bool isRunning = false;
        private readonly IConfigurationService configurationService;
        private CommandLineParameters parameters = new CommandLineParameters();

        public MainWindow(IConfigurationService configurationService)
        {
            this.configurationService = configurationService;
            InitializeComponent();
            LoadSettings();
            CheckArgs();
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
            lstApps.Items.Clear();
            foreach (string entry in configurationService.Applications)
            {
                lstApps.Items.Add(entry);
            }
            lstApps.SelectedIndex = configurationService.SelectedApp;
            txtPassword.Password = configurationService.Password;
            txtPort.Text = configurationService.Port.ToString();
        }

        private void SaveSettings()
        {
            configurationService.Password = txtPassword.Password;

            if (ushort.TryParse(txtPort.Text, out ushort port))
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
                Program.Stop();
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
                    SaveSettings();
                    parameters.Application = configurationService.Applications[configurationService.SelectedApp];
                    parameters.Password = configurationService.Password;
                    parameters.Port = configurationService.Port;
                    Output($"Starting Server listening on Port {port} for App {parameters.Application}");
                    Task.Run(() => Program.Start(parameters, this));
                    isRunning = true;
                    txtPassword.IsEnabled = false;
                    txtPort.IsEnabled = false;
                    lstApps.IsEnabled = false;
                    cboApps.IsEnabled = false;
                }

            }
            ToggleText();
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

        public void KeystrokeReceived(int activator, string key, string[] modifier, bool quickCommand)
        {
            string output = $"Key {key}";
            foreach (string mod in modifier)
                output += $"+{mod}";
            if (activator == 0)
                output += " pressed";
            else
                output += " let go";
            Dispatcher.Invoke(() =>
            {

                txtOutput.Text += $"{output}\n";
                txtOutput.ScrollToEnd();
            });
        }

        public void Output(string message)
        {
            Dispatcher.Invoke(() =>
            {
                txtOutput.Text += $"{message}\n";
                txtOutput.ScrollToEnd();
            });
        }

        private void CheckArgs()
        {
            if (App.Arguments != null)
                Parser.Default.ParseArguments<CommandLineParameters>(App.Arguments)
                    .WithParsed(RunRestApi)
                    .WithNotParsed(HandleParseError);
        }

        private void RunRestApi(CommandLineParameters opts)
        {
            if (string.IsNullOrEmpty(opts.Password))
            {
                opts.Password = configurationService.Password;
            }
            else
            {
                configurationService.Password = opts.Password;
            }
            if (opts.Port == 0)
            {
                opts.Port = configurationService.Port;
            }
            else
            {
                configurationService.Port = opts.Port;
            }
            if (!string.IsNullOrEmpty(opts.Application))
            {
                List<string> apps = configurationService.Applications;
                bool found = false;
                for(int i=0; i < apps.Count; i++)
                {
                    if (apps[i].Equals(opts.Application)) {
                        configurationService.SelectedApp = i;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    apps.Add(opts.Application);
                    configurationService.SelectedApp = apps.Count - 1;
                }
                configurationService.Applications = apps;

            }

            parameters = opts;
            LoadSettings(); //reload to update the ui
            ToggleServer();
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {
            Environment.Exit(1);
        }
    }
}
