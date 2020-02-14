using System;
using System.Text.RegularExpressions;
using CoreTweet.Streaming;

namespace InfoScreenServer.Twitter
{
    public class TweetReceiver : IObserver<StreamingMessage>
    {
        public event Action<TwitterMessage> NewTweet;

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(StreamingMessage streamingMessage)
        {
            if (!(streamingMessage is StatusMessage message)) return;

            var text = message.Status.Text;
            var urlPattern = new Regex(@"http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?");
            while (true)
            {
                var match = urlPattern.Match(text);
                if (!match.Success) break;

                text = text.Replace(match.Groups[0].Value, "");
            }

            var mediaUrl = message.Status.Entities.Media?[0].MediaUrlHttps;
            
            var tweet = new TwitterMessage
            {
                Id = message.Status.Id,
                Text = text,
                Time = message.Status.CreatedAt.DateTime,
                UserName = message.Status.User.Name,
                MediaUrl = mediaUrl 
            };

            NewTweet?.Invoke(tweet);
        }
    }
}