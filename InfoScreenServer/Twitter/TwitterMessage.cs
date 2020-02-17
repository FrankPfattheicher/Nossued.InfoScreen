using System;
// ReSharper disable UnusedMember.Global

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace InfoScreenServer.Twitter
{
    public class TwitterMessage
    {
        public long Id { get; set; }
        public DateTime Time { get; set; }
        public string TimeText => Time.ToString("HH:mm");
        
        public string UserName { get; set; }
        public string UserHandle { get; set; }
        public string UserImage { get; set; }

        
        public string TextColor { get; set; }
        public string BackgroundColor { get; set; }
        public string Text { get; set; }

        public bool HasMedia => !string.IsNullOrEmpty(MediaUrl);
        public string MediaUrl { get; set; }

    }
}