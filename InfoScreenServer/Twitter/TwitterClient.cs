using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CoreTweet;

namespace InfoScreenServer.Twitter
{
    public class TwitterClient : IDisposable
    {
        private readonly InfoSettings _settings;
        private IDisposable _subscription;
        private TweetReceiver _receiver;


        public List<TwitterMessage> Tweets = new List<TwitterMessage>();

        public TwitterClient(InfoSettings settings)
        {
            _settings = settings;
        }

        public event Action<TwitterMessage> NewTweet;
        
        public void Connect(string key, string secret)
        {
            var session = OAuth.Authorize(key, secret);
            
            var startInfo = new ProcessStartInfo(session.AuthorizeUri.AbsoluteUri)
            {
                UseShellExecute = true
            };
            Process.Start(startInfo);
            
            Console.Write("Enter PIN code:");
            var pin = Console.ReadLine();

            try
            {
                var tokens = session.GetTokens(pin);
                var parameters = new Dictionary<string, object>
                {
                    { "track", _settings.Keywords },
                    { "tweet_mode", TweetMode.Extended }
                };
                var stream = tokens.Streaming
                    .FilterAsObservable(parameters);

                Console.WriteLine("Waiting for tweets...");
            
                _receiver = new TweetReceiver(_settings.ColoredTweets);
                _receiver.NewTweet += OnNewTweetReceived;
                _subscription = stream.Subscribe(_receiver);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void OnNewTweetReceived(TwitterMessage tweet)
        {
            Tweets.Insert(0, tweet);
            Tweets = Tweets.Take(_settings.MaxTweets).ToList();
            NewTweet?.Invoke(tweet);
        }

        public void Dispose()
        {
            _receiver.NewTweet -= OnNewTweetReceived;
            _subscription.Dispose();
        }
    }
}