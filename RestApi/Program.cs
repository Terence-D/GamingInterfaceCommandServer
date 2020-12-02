using CommandLine;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace GIC.RestApi
{
    public class Program
    {
        public class Options
        {
            [Option("port", Default = false, Required = true, HelpText = "IP Port to Listen on")]
            public int Port { get; set; }
            [Option("password", Default = false, Required = true, HelpText = "Password to expect from client")]
            public string Password { get; set; }
            [Option("app", Default = false, Required = true, HelpText = "Application to send commands to")]
            public string Application { get; set; }
        }

        public static string key;

        public static void Main(string[] args)
        {
            Start(args);
        }

        private static IHost host;

        private static IHostBuilder CreateHostBuilder(string url) =>
            Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls(url);
                });

        public static void Stop()
        {
            host.StopAsync();
        }

        public static bool Start(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptions)
                .WithNotParsed(HandleParseError);
            return true;
        }

        private static void RunOptions(Options opts)
        {
            CheckFirewall(opts.Port);
            key = Crypto.Encrypt(opts.Password);
            string baseAddress = "http://" + "*" + ":" + opts.Port + "/";

            host = CreateHostBuilder(baseAddress).Build();
            host.Run();
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {
            //handle errors
        }

        private static void CheckFirewall(int port)
        {
            foreach (IPAddress address in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                IPEndPoint ipLocalEndPoint = new IPEndPoint(address, port);

                TcpListener t = new TcpListener(ipLocalEndPoint);
                t.Start();
                t.Stop();
            }
        }

    }
}
