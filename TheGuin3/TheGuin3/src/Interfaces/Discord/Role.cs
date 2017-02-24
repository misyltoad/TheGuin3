using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.WebSocket;

namespace TheGuin3.Interfaces.Discord
{
    public class Role : Base.Role
    {
        public override string InterfaceName => Meta.InterfaceName;

        public Role(SocketRole discordInterface)
        {
            DiscordInterface = discordInterface;
        }

        public override string Id => DiscordInterface.Id.ToString();
        public override string Name => DiscordInterface.Name;
        public override bool IsAdmin => DiscordInterface.Permissions.Administrator;

        public SocketRole DiscordInterface;
    }
}
