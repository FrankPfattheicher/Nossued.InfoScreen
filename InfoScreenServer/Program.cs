using System;
using System.Diagnostics;
using System.IO;
using IctBaden.Framework.AppUtils;
using IctBaden.Framework.IniFile;
using IctBaden.Framework.PropertyProvider;
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

        private static void Main()
        {
            Console.WriteLine("NOSSUED InfoScreenServer");

            var cfgFile = Path.Combine(Application.GetApplicationDirectory(), "InfoScreen.cfg");
            var profile = new Profile(cfgFile);
            var settings = new EventSettings();

            new ClassPropertyProvider(settings)
                .SetProperties(profile["Event"].Properties);

            Console.WriteLine($"Event {settings.Name}, Keyword = {settings.Keyword}");
            
            
            var key = Environment.GetEnvironmentVariable("twitter-key");
            var secret = Environment.GetEnvironmentVariable("twitter-secret");

            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(secret))
            {
                Console.WriteLine("Missing twitter auth.");
                return;
            }
            
            _client = new TwitterClient();
            _client.Connect(key, secret, settings.Keyword);
            
            var vue = new VueResourceProvider();
            var provider = StonehengeResourceLoader.CreateDefaultLoader(vue);
            
            provider.Services.AddService(typeof(TwitterClient), _client);
            provider.Services.AddService(typeof(EventSettings), settings);

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