using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IctBaden.Framework.AppUtils;
using IctBaden.Framework.IniFile;
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
        public string RoomColor { get; set; }
        public string SlotTime { get; set; }
        public int SlotMinutes { get; set; }
        public string SlotOwner { get; set; }
        public string SlotTopic { get; set; }
        public bool SlotIsCurrent { get; set; }


        public RoomVm(AppSession session) : base(session)
        {
            RoomName = "Sonne";
            RoomImage = $"images/{RoomName}.png";
            RoomColor = "transparent";

            SlotTime = "11:00 - 11:45";
            SlotOwner = "Test";
            SlotTopic = "XAML Baml";
        }

        public void Dispose()
        {
        }

        public override void OnLoad()
        {
            if (!Session.Parameters.ContainsKey("room")) return;

            var sessionsFile = Path.Combine(Application.GetApplicationDirectory(), "Sessions.cfg");
            var profile = new Profile(sessionsFile);
            
            RoomName = Session.Parameters["room"];
            RoomImage = $"images/{RoomName}.png";

            var sessions = profile[RoomName];
            RoomColor = sessions.Get("Color", "transparent");

            var now = DateTime.Now.Hour * 60 + DateTime.Now.Minute;
            var time = new Regex("([0-9]+):([0-9]+)[^0-9]*([0-9]+):([0-9]+)");
            foreach (var key in sessions.Keys)
            {
                var match = time.Match(key.Name);
                if(!match.Success) continue;

                var start = int.Parse(match.Groups[1].Value) * 60 + int.Parse(match.Groups[2].Value);
                var until = int.Parse(match.Groups[3].Value) * 60 + int.Parse(match.Groups[4].Value);

                if (now <= until)
                {
                    SlotTime = key.Name;

                    var slot = sessions[key.Name].StringValue.Split(',');
                    if (slot.Length > 1)
                    {
                        SlotOwner = slot[0];
                        SlotTopic = slot[1];
                    }

                    SlotIsCurrent = now >= start;
                    SlotMinutes = start - now;
                    break;
                }
                
            }

            Task.Run(() =>
            {
                Task.Delay(TimeSpan.FromSeconds(30)).Wait();
                NavigateTo("tweets");
            });
        }
        
    }
}