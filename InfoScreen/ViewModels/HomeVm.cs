using System;
using System.Collections.Generic;
using IctBaden.Stonehenge3.Core;
using IctBaden.Stonehenge3.ViewModel;
// ReSharper disable MemberCanBePrivate.Global

namespace InfoScreen.ViewModels
{
    public class HomeVm : ActiveViewModel, IDisposable
    {
        public string Year { get; } = "2020";

        public List<string> Tweets => Program.Tweets;

        public HomeVm(AppSession session) : base(session)
        {
            Program.NewTweet += OnNewTweet;
        }

        public void Dispose()
        {
            Program.NewTweet -= OnNewTweet;
        }
        
        private void OnNewTweet()
        {
            NotifyPropertyChanged(nameof(Tweets));
        }


    }
}