using System;
using System.IO;
using System.Linq;
using IctBaden.Framework.AppUtils;
using IctBaden.Framework.IniFile;
using IctBaden.Stonehenge3.Core;
using IctBaden.Stonehenge3.ViewModel;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace InfoScreenServer.ViewModels
{
    public class OverviewVm : ActiveViewModel, IDisposable
    {
        public RoomDetailsVm[] Rooms { get; private set; }

        public OverviewVm(AppSession session) : base(session)
        {
            Rooms = new RoomDetailsVm[0]; 
        }

        public override void OnLoad()
        {
            var sessionsFile = Path.Combine(Application.GetApplicationDirectory(), "Sessions.cfg");
            var profile = new Profile(sessionsFile);
            Rooms = profile.Sections
                .Where(s => !string.IsNullOrWhiteSpace(s.Name))
                .Select(s => new RoomDetailsVm
                {
                   Name = s.Name,
                   Capacity = profile[s.Name].Get<int>("Capacity")
                }).ToArray();
        }

        public void Dispose()
        {
        }
        
    }
}