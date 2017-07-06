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
            DiscordInterface.Connected += () =>
            {
                Task.Run(() =>
                {
                    Console.WriteLine("Successfully connected to Discord.");
                });

                return null;
            };

            DiscordInterface.MessageReceived += (msg) =>
            {
               Task.Run(() =>
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

                   if (socketChannel?.Id == 330510635274338305)
                       DiscordInterface?.GetUser(197474085414895616)?.GetOrCreateDMChannelAsync().Result?.SendMessageAsync("hey edo check memes", true);
                });
                return null;
            };

            DiscordInterface.UserJoined += (user) =>
            {
                Task.Run(() =>
                {
                    OnUserJoined(new Discord.User(user), new Discord.Server(user.Guild));
                });
                return null;
            };

            DiscordInterface.UserBanned += (user, server) =>
            {
                Task.Run(() =>
                {
                    OnUserBanned(new Discord.User(user), new Discord.Server(server));
                });
                return null;
            };

            DiscordInterface.UserLeft += (user) =>
            {
                Task.Run(() =>
                {
                    OnUserLeft(new Discord.User(user), new Discord.Server(user.Guild));
                });
                return null;
            };

            DiscordInterface.UserUnbanned += (user, server) =>
            {
                Task.Run(() =>
                {
                    OnUserUnbanned(new Discord.User(user), new Discord.Server(server));
                });
                return null;
            };

            DiscordInterface.GuildMemberUpdated += (oldUser, newUser) =>
            {
                Task.Run(() =>
                {
                    OnUserChange(new Discord.User(oldUser), new Discord.User(newUser));
                });
                return null;
            };

            /*DiscordInterface.UserUpdated += (oldUser, newUser) =>
            {
                OnUserChange(new Discord.User(oldUser), new Discord.User(newUser));
                return null;
            };*/

            if (Config.Token == "thebotlogintokenhere")
            {
                Console.WriteLine("You need to configure your bot.");
                return;
            }

            DiscordInterface.LoginAsync(TokenType.Bot, Config.Token);
            DiscordInterface.StartAsync();
        }

        public override void Stop()
        {
            DiscordInterface.StopAsync().Wait();
            DiscordInterface.Dispose();
        }

        ~Client()
        {
            Stop();
        }

        DiscordSocketClient DiscordInterface;
    }
}
