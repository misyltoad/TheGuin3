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
            DiscordInterface = new EmbedBuilder();
            var builder = DiscordInterface;

            if (!String.IsNullOrWhiteSpace(embed.Author.HyperlinkUrl) ||
                !String.IsNullOrWhiteSpace(embed.Author.IconUrl) ||
                !String.IsNullOrWhiteSpace(embed.Author.Name))
            {
                EmbedAuthorBuilder authorBuilder = new EmbedAuthorBuilder();
                try { authorBuilder.IconUrl = embed.Author.IconUrl; } catch { }
                try { authorBuilder.Name = embed.Author.Name; } catch { }
                try { authorBuilder.Url = embed.Author.HyperlinkUrl; } catch { }
                builder.Author = authorBuilder;
            }

            if (!String.IsNullOrWhiteSpace(embed.Footer.IconUrl) ||
                !String.IsNullOrWhiteSpace(embed.Footer.Text))
            {
                EmbedFooterBuilder footerBuilder = new EmbedFooterBuilder();
                try { footerBuilder.IconUrl = embed.Footer.IconUrl; } catch { }
                try { footerBuilder.Text = embed.Footer.Text; } catch { }
                builder.Footer = footerBuilder;
            }

            try { builder.Url = embed.HyperlinkUrl; } catch { }
            try { builder.ThumbnailUrl = embed.ThumbnailUrl; } catch { }
            try { builder.ImageUrl = embed.ImageUrl; } catch { }

            try { builder.Timestamp = embed.Timestamp; } catch { }

            Color color = new Color();
            try { color = new Color(embed.Colour.Value.R, embed.Colour.Value.G, embed.Colour.Value.B); } catch { }

            try { builder.Color = color; } catch { }
            try { builder.Footer.IconUrl = embed.Footer.IconUrl; } catch { }
            try { builder.Footer.Text = embed.Footer.Text; } catch { }

            foreach (var section in embed.Sections)
            {
                EmbedFieldBuilder fieldBuilder = new EmbedFieldBuilder();
                try { fieldBuilder.IsInline = section.IsInline; } catch { }
                try { fieldBuilder.Name = section.Name; } catch { }
                try { fieldBuilder.Value = section.Value; } catch { }

                DiscordInterface.AddField(fieldBuilder);
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
