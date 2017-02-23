using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.WebSocket;

namespace TheGuin3.Interfaces.Discord
{
    public class Message : Base.Message
    {
        public string InterfaceName => Meta.InterfaceName;

        public Message(string value)
            : base(value)
        { }

        private void AddEmbed(Embed embed)
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder.Author.Name = embed.Author.Name;
            builder.Author.IconUrl = embed.Author.IconUrl;
            builder.Author.Url = embed.Author.HyperlinkUrl;

            builder.Url = embed.HyperlinkUrl;
            builder.ThumbnailUrl = embed.ThumbnailUrl;
            builder.ImageUrl = embed.ImageUrl;

            builder.Timestamp = embed.Timestamp;

            Color color = new Color(embed.Colour.Value.R, embed.Colour.Value.G, embed.Colour.Value.B);

            builder.Color = color;
            builder.Footer.IconUrl = embed.Footer.IconUrl;
            builder.Footer.Text = embed.Footer.Text;

            foreach (var section in embed.Sections)
            {
                EmbedFieldBuilder fieldBuilder = new EmbedFieldBuilder();
                fieldBuilder.IsInline = section.IsInline;
                fieldBuilder.Name = section.Name;
                fieldBuilder.Value = section.Value;
            }
        }

        public Message(Embed embed)
            : base(null)
        {
            AddEmbed(embed);
        }

        public Message(Embed embed, string value)
            : base(value)
        {
            AddEmbed(embed);
        }

        public static implicit operator global::Discord.Embed(Message msg)
        {
            if (msg.DiscordInterface != null)
                return msg.DiscordInterface;
            return null;
        }

        private EmbedBuilder DiscordInterface;
    }
}
