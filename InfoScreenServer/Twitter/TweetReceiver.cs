using System;
using System.Text.RegularExpressions;
using CoreTweet.Streaming;

namespace InfoScreenServer.Twitter
{
    public class TweetReceiver : IObserver<StreamingMessage>
    {
        private readonly bool _coloredTweets;
        public event Action<TwitterMessage> NewTweet;

        public TweetReceiver(bool coloredTweets)
        {
            _coloredTweets = coloredTweets;
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

            var text = message.Status.FullText ?? message.Status.Text;
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
                UserHandle = (message.Status.User.ScreenName != message.Status.User.Name)
                    ? "@" + message.Status.User.ScreenName
                    : "",
                UserImage = message.Status.User.ProfileImageUrlHttps,
                TextColor = "black",
                BackgroundColor = GetBackgroundColor(message.Status.User.Name),
                MediaUrl = mediaUrl
            };

            NewTweet?.Invoke(tweet);
        }

        private string GetBackgroundColor(string name)
        {
            if(!_coloredTweets) return "whitesmoke";
            
            name = name.Substring(0, 1).ToLower();
            switch (name[0])
            {
                case 'd':
                    return "lightskyblue";
                case 's':
                    return "#f0b0b0";
                case 'e':
                    return "lightgoldenrodyellow";
                case 'i':
                    return "lightpink";
                case 'w':
                    return "lightgreen";
                case 'k':
                    return "#ffb08a";
                case 'a':
                    return "lightseagreen";
                case 'p':
                    return "lightsteelblue";
                case 'b':
                    return "#8798a9";
                case 'm':
                    return "lightcyan";
                case 'n':
                    return "lightgray";
                case 'r':
                    return "lightyellow";
                case 't':
                    return "navajowhite";
                default:
                    return "whitesmoke";    
            }
        }
        
    }
}