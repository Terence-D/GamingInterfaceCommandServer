using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Windows;

namespace GIC.Wpf
{
    /// <summary>
    /// Interaction logic for Help.xaml
    /// </summary>
    public partial class Help : Window
    {
        public Help()
        {
            InitializeComponent();
            List<string> addresses = AllIPAddresses();
            lstAddresses.ItemsSource = addresses;
        }

        private string PreferredIPAddress()
        {
            try
            {
                string localIP;
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {
                    socket.Connect("8.8.8.8", 65530);
                    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                    localIP = endPoint.Address.ToString();
                }

                return localIP;
            } catch (Exception)
            {
                return null;
            }
 
        }

        private List<string> AllIPAddresses()
        {
            try
            {
                List<string> addresses = new List<string>();
                string strHostName = string.Empty;
                // Getting Ip address of local machine...
                // First get the host name of local machine.
                strHostName = Dns.GetHostName();

                // Then using host name, get the IP address list..
                IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
                IPAddress[] addr = ipEntry.AddressList;

                for (int i = 0; i < addr.Length; i++)
                {
                    addresses.Add(addr[i].ToString());
                }
                return addresses;
            } catch (Exception)
            {
                return null;
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", "https://github.com/Terence-D/GamingInterfaceCommandServer/wiki");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            txtPreferred.Text = PreferredIPAddress();
        }
    }
}
