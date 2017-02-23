using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace TheGuin3.Interfaces.Discord
{
    public class Embed
    {
        public Embed()
        {
            Sections = new List<Section>();
        }

        public class AuthorData
        {
            public string Name;
            public string IconUrl;
            public string HyperlinkUrl;
        }
        public AuthorData Author;

        public class FooterData
        {
            public string Text;
            public string IconUrl;
        }

        public FooterData Footer;

        public string HyperlinkUrl;
        public string ThumbnailUrl;
        public string ImageUrl;
        public DateTimeOffset? Timestamp;
        public Color? Colour;

        public class Section
        {
            public string Name;
            public string Value;
            public bool IsInline;
        }

        public List<Section> Sections;
    }
}
