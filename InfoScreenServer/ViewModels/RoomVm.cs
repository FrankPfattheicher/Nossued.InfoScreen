using System;
using System.Threading.Tasks;
using IctBaden.Stonehenge3.Core;
using IctBaden.Stonehenge3.ViewModel;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace InfoScreenServer.ViewModels
{
    // ReSharper disable once UnusedType.Global
    public class RoomVm : ActiveViewModel, IDisposable
    {
        public string RoomImage { get; set; }
        public string RoomName { get; set; }
        public string NextSlot { get; set; }
        public string NextTopic { get; set; }


        public RoomVm(AppSession session) : base(session)
        {
            RoomName = "Sonne";
            RoomImage = $"images/{RoomName}.png";

            NextSlot = "11:00 - 11:45";
            NextTopic = "XAML Baml";
        }

        public void Dispose()
        {
        }

        public override void OnLoad()
        {
            if (!Session.Parameters.ContainsKey("room")) return;

            RoomName = Session.Parameters["room"];
            RoomImage = $"images/{RoomName}.png";

            NextSlot = "11:00 - 11:45";
            NextTopic = "XAML Baml";

            Task.Run(() =>
            {
                Task.Delay(TimeSpan.FromSeconds(30)).Wait();
                NavigateTo("tweets");
            });
        }
        
    }
}