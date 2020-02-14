using System;
using System.Collections.Generic;
using System.Linq;
using IctBaden.Stonehenge3.Hosting;
using IctBaden.Stonehenge3.Kestrel;
using IctBaden.Stonehenge3.Resources;
using IctBaden.Stonehenge3.Vue;
using InfoScreen.Twitter;

namespace InfoScreen
{
    internal static class Program
    {
        private static TwitterClient _client;

        public static int MaxTweets { get; set; } = 10;

        public static List<string> Tweets = new List<string>();
        public static event Action NewTweet;
        
        static void Main(string[] args)
        {
            Console.WriteLine("NOSSUED InfoScreen");

            // var resolver = new UdpServerResolver("NOSSUED", 56987);
            // var server = resolver.Resolve();
            
            
            var key = Environment.GetEnvironmentVariable("twitter-key");
            var secret = Environment.GetEnvironmentVariable("twitter-secret");

            _client = new TwitterClient();
            _client.Connect(key, secret, "JetBrains");
            _client.NewTweet += OnNewTweet;
            
            var vue = new VueResourceProvider();
            var provider = StonehengeResourceLoader.CreateDefaultLoader(vue);
            var options = new StonehengeHostOptions();
            var host = new KestrelHost(provider, options);
            host.Start("localhost", 32000);
            
            
            Console.ReadLine();
            _client.Dispose();
        }

        private static void OnNewTweet(string message)
        {
            Tweets.Insert(0, message);
            Tweets = Tweets.Take(MaxTweets).ToList();
            NewTweet?.Invoke();
        }

    }
}