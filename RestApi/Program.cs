using CommandLine;
using GIC.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace GIC.RestApi
{
    public class Program
    {
        internal static void ReceivedKey(Command value, bool quickCommand)
        {
            if (AppListener != null)
                AppListener.KeystrokeReceived(value.ActivatorType, value.Key, value.Modifier, quickCommand);
        }

        internal static void ReceivedVersionCheck()
        {
            if (AppListener != null)
                AppListener.Output("Version check received");
        }

        public static string Key { get; internal set; }
        public static IListener AppListener;

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

        public static bool Start(string[] args, IListener listener = null)
        {
            if (listener != null)
                AppListener = listener;
            Parser.Default.ParseArguments<CommandLineParameters>(args)
                .WithParsed(RunServer)
                .WithNotParsed(HandleParseError);
            return true;
        }

        public static void Start(CommandLineParameters opts, IListener listener = null)
        {
            if (listener != null)
                AppListener = listener;

            RunServer(opts);
        }

        private static void RunServer(CommandLineParameters opts)
        {
            CheckFirewall(opts.Port);
            Key = Crypto.Encrypt(opts.Password);
            KeyMaster.Action.Application = opts.Application;
            string baseAddress = "http://" + "*" + ":" + opts.Port + "/";

            host = CreateHostBuilder(baseAddress).Build();
            host.Run();
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {
            //handle errors
            System.Environment.Exit(1);
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
