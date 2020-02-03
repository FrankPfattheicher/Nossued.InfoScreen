using System;
// ReSharper disable ConvertToUsingDeclaration

namespace InfoServer
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("InfoServer");

            using (var clientServer = new UdpClientService("NOSSUED", 56987))
            {
                Console.ReadLine();
            }
            
        }
    }
}