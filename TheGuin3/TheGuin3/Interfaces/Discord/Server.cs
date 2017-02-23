using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.WebSocket;

namespace TheGuin3.Interfaces.Discord
{
    public class Server : Base.Server
    {
        public Server(SocketGuild discordInterface)
        {
            DiscordInterface = discordInterface;
        }

        public override List<Base.TextChannel> TextChannels
        {
            get
            {
                List<Base.TextChannel> channels = new List<Base.TextChannel>();
                foreach (var channel in DiscordInterface.TextChannels)
                {
                    channels.Add(new Discord.TextChannel(channel));
                }

                return channels;
            }
        }
        public override List<Base.User> Users
        {
            get
            {
                List<Base.User> users = new List<Base.User>();
                foreach (var user in DiscordInterface.Users)
                {
                    users.Add(new Discord.User(user));
                }

                return users;
            }
        }

        public override void KickUser(Base.User user)
        {
            foreach (var searchUser in DiscordInterface.Users)
            {
                if (searchUser.Id.ToString() == user.Id)
                    searchUser.KickAsync();
            }
        }
        public override void BanUser(Base.User user, int days)
        {
            foreach (var searchUser in DiscordInterface.Users)
            {
                if (searchUser.Id.ToString() == user.Id)
                    DiscordInterface.AddBanAsync(searchUser, days);
            }
        }

        public override Base.User Owner
        {
            get
            {
                foreach (var searchUser in DiscordInterface.Users)
                {
                    if (searchUser.Id == DiscordInterface.Owner.Id)
                        return new Discord.User(searchUser);
                }
                return null;
            }
        }

        private SocketGuild DiscordInterface;
    }
}
