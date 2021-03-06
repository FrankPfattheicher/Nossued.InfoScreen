using System;
using System.Threading.Tasks;
using IctBaden.Stonehenge3.Core;
using IctBaden.Stonehenge3.ViewModel;
using InfoScreenServer.Twitter;
// ReSharper disable UnusedMember.Global

// ReSharper disable MemberCanBePrivate.Global

namespace InfoScreenServer.ViewModels
{
    // ReSharper disable once UnusedType.Global
    public class TweetsVm : ActiveViewModel, IDisposable
    {
        private readonly TwitterClient _twitter;
        private readonly InfoSettings _settings;

        public string Name => _settings.Name;
        public TwitterMessage[] Tweets => _twitter.Tweets.ToArray();

        public TweetsVm(AppSession session, TwitterClient twitter, InfoSettings settings) 
            : base(session)
        {
            _twitter = twitter;
            _settings = settings;
        }

        public override void OnLoad()
        {
            _twitter.NewTweet += OnNewTweet;
            if (Session.Parameters.ContainsKey("room"))
            {
                Task.Run(() =>
                {
                    Task.Delay(TimeSpan.FromSeconds(30)).Wait();
                    NavigateTo("room");
                });
            }
        }

        public void Dispose()
        {
            _twitter.NewTweet -= OnNewTweet;
        }
        
        private void OnNewTweet(TwitterMessage tweet)
        {
            NotifyPropertyChanged(nameof(Tweets));
        }


    }
}