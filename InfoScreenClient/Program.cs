using System;
using System.IO;
using IctBaden.Framework.AppUtils;
using IctBaden.Framework.IniFile;

// ReSharper disable ConvertToUsingDeclaration

namespace InfoScreenClient
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("InfoScreenClient");

            var cfgFile = Path.Combine(Application.GetApplicationDirectory(), "InfoScreen.cfg");
            var settings = new Profile(cfgFile);

            var room = settings["Room"].Get("Name", "Raumname?");
            
            // connect to server
            
            
            // navigate to room's page
            
            
        }
    }
}