using System;
using CoreTweet.Streaming;

namespace InfoScreen.Twitter
{
    public class TweetReceiver : IObserver<StreamingMessage>
    {
        public event Action<string> NewTweet;

        public TweetReceiver()
        {
            
        }
        
        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(StreamingMessage msg)
        {
            if (!(msg is StatusMessage status)) return;
                
            Console.WriteLine(status.Status.User.Name + ": " + status.Status.Text);
            
            NewTweet?.Invoke(status.Status.Text);
        }
    }
}