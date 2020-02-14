using System;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace InfoScreenServer.Twitter
{
    public class TwitterMessage
    {
        public long Id { get; set; }
        public DateTime Time { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }

    }
}