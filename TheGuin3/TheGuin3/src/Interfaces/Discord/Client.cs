using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace TheGuin3.Interfaces.Discord
{
    public class Client : Base.Client
    {
        public override string InterfaceName => Meta.InterfaceName;

        public Client()
        {
            DiscordInterface = new DiscordSocketClient();
        }

        public struct ConfigData
        {
            public string Token;
        }

        public ConfigData Config;

        public override void Init()
        {
            DiscordInterface.LoginAsync(TokenType.Bot, Config.Token);
            DiscordInterface.ConnectAsync();

            DiscordInterface.MessageReceived += (msg) =>
            {
                var socketChannel = msg.Channel as SocketTextChannel;

                if (socketChannel != null)
                {
                    var user = new Discord.User(msg.Author);
                    var channel = new Discord.TextChannel(socketChannel);
                    var server = new Discord.Server(socketChannel.Guild);

                    OnPublicMessageRecieved(user, channel, server, msg.Content);
                }
                else
                {
                    var user = new Discord.User(msg.Author);
                    OnPrivateMessageRecieved(user, msg.Content);
                }

                return null;
            };

            DiscordInterface.UserJoined += (user) =>
            {
                OnUserJoined(new Discord.User(user), new Discord.Server(user.Guild));
                return null;
            };

            DiscordInterface.UserBanned += (user, server) =>
            {
                OnUserBanned(new Discord.User(user), new Discord.Server(server));
                return null;
            };

            DiscordInterface.UserLeft += (user) =>
            {
                OnUserLeft(new Discord.User(user), new Discord.Server(user.Guild));
                return null;
            };

            DiscordInterface.UserUnbanned += (user, server) =>
            {
                OnUserUnbanned(new Discord.User(user), new Discord.Server(server));
                return null;
            };

            DiscordInterface.GuildMemberUpdated += (oldUser, newUser) =>
            {
                OnUserChange(new Discord.User(oldUser), new Discord.User(newUser));
                return null;
            };

            /*DiscordInterface.UserUpdated += (oldUser, newUser) =>
            {
                OnUserChange(new Discord.User(oldUser), new Discord.User(newUser));
                return null;
            };*/
        }

        public override void Stop()
        {
            DiscordInterface.DisconnectAsync();
        }

        DiscordSocketClient DiscordInterface;
    }
}
