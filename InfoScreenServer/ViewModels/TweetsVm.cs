using System;
using System.Collections.Generic;
using System.Linq;
using IctBaden.Stonehenge3.Core;
using IctBaden.Stonehenge3.ViewModel;
using InfoScreenServer.Twitter;

// ReSharper disable MemberCanBePrivate.Global

namespace InfoScreenServer.ViewModels
{
    public class TweetsVm : ActiveViewModel, IDisposable
    {
        private readonly TwitterClient _twitter;
        private readonly EventSettings _settings;

        public string Name => _settings.Name;
        public TwitterMessage[] Tweets => _twitter.Tweets.ToArray();

        public TweetsVm(AppSession session, TwitterClient twitter, EventSettings settings) 
            : base(session)
        {
            _twitter = twitter;
            _settings = settings;
        }

        public override void OnLoad()
        {
            _twitter.NewTweet += OnNewTweet;
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