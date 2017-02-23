using System;
using System.IO;

namespace TheGuin3
{
    class Program
    {
        static void Main(string[] args)
        {
            Directory.CreateDirectory(Config.StaticConfig.Paths.DataPath);
            Directory.CreateDirectory(Config.StaticConfig.Paths.ModulesPath);
            Directory.CreateDirectory(Config.StaticConfig.Paths.TempPath);

            foreach (var login in Config.Schema.DiscordConfig.GetList())
            {
                var client = new Interfaces.Discord.Client();
                client.Config.Token = login.LoginToken;
                client.Start();
            }

            while (true)
            { }
        }
    }
}