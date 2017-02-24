using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Net.Http;
using Discord;
using System.IO;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace TheGuin3.Interfaces.Discord
{
    public class User : Base.User
    {
        public override string InterfaceName => Meta.InterfaceName;

        // Creation Stuff

        public User(SocketUser discordInterface)
        {
            DiscordInterface = discordInterface;
        }

        // Satisfying Base

        public override string Nickname => (DiscordInterface as SocketGuildUser)?.Nickname ?? DiscordInterface.Username;
        public override string Username => DiscordInterface.Username;
        public override string Tag => DiscordInterface.Mention;
        public override string HumanTag => String.Format("@{0}#{1}", Username, Discriminator);
        public override string DataString => String.Format("``{0}`` (``{1}``|``{2}``)", Tag, HumanTag, Nickname);
        public override string Id => DiscordInterface.Id.ToString();
        public override string AvatarUrl => DiscordInterface.GetAvatarUrl();
        public override Bitmap Avatar
        {
            get
            {
                try
                {
                    byte[] imageContent = new HttpClient().GetAsync(DiscordInterface.GetAvatarUrl()).GetAwaiter().GetResult().Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
                    if (imageContent != null)
                    {
                        var image = System.Drawing.Image.FromStream(new MemoryStream(imageContent));
                        return new Bitmap(image);
                    }
                    return null;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            }
        }

        public override List<Base.Role> Roles
        {
            get
            {
                var guildUser = (DiscordInterface as SocketGuildUser);
                if (guildUser != null)
                {
                    List<Base.Role> roles = new List<Base.Role>();
                    foreach (var role in guildUser.Roles)
                        roles.Add(new Discord.Role(role));

                    return roles;
                }
                return new List<Base.Role>(); ;
            }
        }

        public override void GiveRole(Base.Role role)
        {
            var guildUser = (DiscordInterface as SocketGuildUser);
            var discRole = (role as Role);
            if (guildUser != null && discRole != null)
            {
                guildUser.AddRolesAsync(discRole.DiscordInterface);
            }
        }
        public override void SendMessage(Base.Message message)
        {
            Discord.Message discMessage = message as Discord.Message;
            if (discMessage != null)
            {
                global::Discord.Embed embed = discMessage;
                if (embed != null)
                {
                    if (discMessage.Text != null)
                        DiscordInterface.CreateDMChannelAsync().GetAwaiter().GetResult().SendMessageAsync(discMessage.Text, false, embed);
                    else
                        DiscordInterface.CreateDMChannelAsync().GetAwaiter().GetResult().SendMessageAsync("", false, embed);
                    return;
                }
            }
            DiscordInterface.CreateDMChannelAsync().GetAwaiter().GetResult().SendMessageAsync(message.Text);
        }
        public override Base.Server Server
        {
            get
            {
                var guildUser = (DiscordInterface as SocketGuildUser);
                if (guildUser != null)
                    return new Discord.Server(guildUser.Guild);
                return null;
            }
        }

        public override bool IsBotOwner => Id == Config.Schema.DiscordConfig.Get().OwnerId.ToString();

        // Discord Only

        public string Discriminator => DiscordInterface.Discriminator;

        private SocketUser DiscordInterface;
    }
}
