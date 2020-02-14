using System;
using System.Collections.Generic;
using IctBaden.Stonehenge3.Core;
using IctBaden.Stonehenge3.ViewModel;
using InfoScreenServer.Twitter;

// ReSharper disable MemberCanBePrivate.Global

namespace InfoScreenServer.ViewModels
{
    public class HomeVm : ActiveViewModel, IDisposable
    {
        private readonly TwitterClient _twitter;

        public List<TwitterMessage> Tweets => _twitter.Tweets;

        public HomeVm(AppSession session, TwitterClient twitter) 
            : base(session)
        {
            _twitter = twitter;
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