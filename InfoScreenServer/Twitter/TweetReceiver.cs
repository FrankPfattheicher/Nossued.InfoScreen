using System;
using CoreTweet.Streaming;

namespace InfoScreenServer.Twitter
{
    public class TweetReceiver : IObserver<StreamingMessage>
    {
        public event Action<TwitterMessage> NewTweet;

        public TweetReceiver()
        {
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(StreamingMessage streamingMessage)
        {
            if (!(streamingMessage is StatusMessage message)) return;

            var tweet = new TwitterMessage
            {
                Id = message.Status.Id,
                Text = message.Status.Text,
                Time = message.Status.CreatedAt.DateTime,
                UserName = message.Status.User.Name
            };
            Console.WriteLine(message.Status.User.Name + ": " + message.Status.Text);

            NewTweet?.Invoke(tweet);
        }
    }
}