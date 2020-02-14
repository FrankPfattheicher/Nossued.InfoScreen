using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using IctBaden.Framework.AppUtils;
using IctBaden.Framework.IniFile;
using IctBaden.Stonehenge3.Hosting;
using IctBaden.Stonehenge3.Kestrel;
using IctBaden.Stonehenge3.Resources;
using IctBaden.Stonehenge3.Vue;
using InfoScreenServer.Twitter;

namespace InfoScreenServer
{
    internal static class Program
    {
        private static TwitterClient _client;

        static void Main(string[] args)
        {
            Console.WriteLine("NOSSUED InfoScreenServer");

            var cfgFile = Path.Combine(Application.GetApplicationDirectory(), "InfoScreen.cfg");
            var settings = new Profile(cfgFile);

            var eventName = settings["Event"].Get("Name", "NOSSUED");
            var eventKeyword = settings["Event"].Get("Keyword", "NOSSUED");

            var key = Environment.GetEnvironmentVariable("twitter-key");
            var secret = Environment.GetEnvironmentVariable("twitter-secret");

            _client = new TwitterClient();
            _client.Connect(key, secret, "JetBrains");
            
            var vue = new VueResourceProvider();
            var provider = StonehengeResourceLoader.CreateDefaultLoader(vue);
            provider.Services.AddService(typeof(TwitterClient), _client);
            var options = new StonehengeHostOptions();
            var host = new KestrelHost(provider, options);
            host.Start("localhost", 32000);

            Process.Start(new ProcessStartInfo(host.BaseUrl)
            {
                UseShellExecute = true
            });
            
            Console.ReadLine();
            _client.Dispose();
        }

    }
}