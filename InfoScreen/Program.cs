using System;
using InfoScreen.Twitter;

namespace InfoScreen
{
    internal static class Program
    {
        private static TwitterClient _client;
        
        static void Main(string[] args)
        {
            Console.WriteLine("NOSSUED InfoScreen");

            var resolver = new UdpServerResolver("NOSSUED", 56987);
            var server = resolver.Resolve();
            
            
            var key = Environment.GetEnvironmentVariable("twitter-key");
            var secret = Environment.GetEnvironmentVariable("twitter-secret");

            _client = new TwitterClient();
            _client.Connect(key, secret, "nossued");
            
            Console.ReadLine();
            _client.Dispose();
        }


    }
}