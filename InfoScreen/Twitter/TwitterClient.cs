using System;
using System.Diagnostics;
using CoreTweet;

namespace InfoScreen.Twitter
{
    public class TwitterClient : IDisposable
    {
        private IDisposable _subscription;
        private TweetReceiver _receiver;

        public event Action<string> NewTweet;
        
        public bool Connect(string key, string secret, string trackWord)
        {
            var session = OAuth.Authorize(key, secret);
            
            var startInfo = new ProcessStartInfo(session.AuthorizeUri.AbsoluteUri)
            {
                UseShellExecute = true
            };
            System.Diagnostics.Process.Start(startInfo);
            
            Console.Write("Enter PIN code:");
            var pin = Console.ReadLine();

            try
            {
                var tokens = session.GetTokens(pin);

                var stream = tokens.Streaming
                    .FilterAsObservable(track => trackWord);

                Console.WriteLine("Waiting for tweets...");
            
                _receiver = new TweetReceiver();
                _receiver.NewTweet += OnNewTweetReceived;
                _subscription = stream.Subscribe(_receiver);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private void OnNewTweetReceived(string message)
        {
            NewTweet?.Invoke(message);
        }

        public void Dispose()
        {
            _receiver.NewTweet -= OnNewTweetReceived;
            _subscription.Dispose();
        }
    }
}