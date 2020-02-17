using System;
using System.Collections.Generic;
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
using Newtonsoft.Json;

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
            var settings = new InfoSettings();

            var spp = new ClassPropertyProvider(settings);
            spp.SetProperties(profile["Event"].Properties);
            spp.SetProperties(profile["Twitter"].Properties);

            Console.WriteLine($"Event {settings.Name}, Keyword = {settings.Keywords}");


            var key = "";//Environment.GetEnvironmentVariable("twitter-key");
            var secret = "";//Environment.GetEnvironmentVariable("twitter-secret");

            _client = new TwitterClient(settings);
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(secret))
            {
                Console.WriteLine("Missing twitter auth - start demo mode");
                var tweets = Path.Combine(Application.GetApplicationDirectory(), "DemoTweets.json");
                var json = File.ReadAllText(tweets);
                _client.Tweets = JsonConvert.DeserializeObject<List<TwitterMessage>>(json);
            }
            else
            {
                _client.Connect(key, secret);
            }            
            
            var vue = new VueResourceProvider();
            var provider = StonehengeResourceLoader.CreateDefaultLoader(vue);
            
            provider.Services.AddService(typeof(TwitterClient), _client);
            provider.Services.AddService(typeof(InfoSettings), settings);

            var options = new StonehengeHostOptions
            {
                Title = settings.Name
            };
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