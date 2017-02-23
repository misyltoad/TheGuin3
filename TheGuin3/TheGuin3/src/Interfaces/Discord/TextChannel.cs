using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Discord;
using Discord.WebSocket;

namespace TheGuin3.Interfaces.Discord
{
    public class TextChannel : Base.TextChannel
    {
        public override string InterfaceName => Meta.InterfaceName;

        // Creation Stuff

        public TextChannel(SocketTextChannel discordInterface)
        {
            DiscordInterface = discordInterface;
        }

        // Satisfying Base

        public override void SendMessage(Base.Message message)
        {
            Discord.Message discMessage = message as Discord.Message;
            if (discMessage != null)
            {
                global::Discord.Embed embed = discMessage;
                if (embed != null)
                {
                    if (discMessage.Text != null)
                        DiscordInterface.SendMessageAsync(discMessage.Text, false, embed);
                    else
                        DiscordInterface.SendMessageAsync("", false, embed);
                    return;
                }
            }
            DiscordInterface.SendMessageAsync(message.Text);
        }
        public override void SendFile(string filename)
        {
            DiscordInterface.SendFileAsync(filename, "");
        }
        public override void SendFile(Stream stream, string fakeName)
        {
            DiscordInterface.SendFileAsync(stream, fakeName, "");
        }

        public override List<Base.User> Users
        {
            get
            {
                List<Base.User> users = new List<Base.User>();
                foreach (var user in DiscordInterface.Users)
                    users.Add(new Discord.User(user));

                return users;
            }
        }
        public override Base.Server Server => new Discord.Server((DiscordInterface as SocketGuildChannel)?.Guild);
        public override string Name { get; }
        public override string Id { get; }

        private SocketTextChannel DiscordInterface;
    }
}
