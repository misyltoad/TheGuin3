using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.WebSocket;

namespace TheGuin3.Interfaces.Discord
{
    public class Client : Base.Client
    {
        public Client()
        {
            DiscordInterface = new DiscordSocketClient();
        }

        public struct ConfigData
        {
            string Token;
        }

        public ConfigData Config;

        public override void Start()
        {

        }

        DiscordSocketClient DiscordInterface;
    }
}
