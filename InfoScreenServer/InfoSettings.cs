// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace InfoScreenServer
{
    public class InfoSettings
    {
        public string Name { get; set; }
        
        /// <summary>
        /// Comma separated list
        /// </summary>
        public string Keywords { get; set; }
        public bool ColoredTweets { get; set; }
        
        public int MaxTweets { get; set; } = 30;
    }
}