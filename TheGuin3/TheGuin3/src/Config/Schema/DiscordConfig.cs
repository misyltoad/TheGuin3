using System;
using System.Collections.Generic;
using System.Text;

namespace TheGuin3.Config.Schema
{
    public class DiscordConfigSchema
    {
        public ulong OwnerId { get; set; }
        public string LoginToken { get; set; }
    }

    public class DefaultDiscordConfig : DefaultConfig
    {
        public DefaultDiscordConfig()
        {
            DefaultContent = "{\n'OwnerId': 123123123,\n'LoginToken': 'thebotlogintokenhere'\n}";
        }
    }

    public class DiscordConfig : ConfigInterface<DiscordConfigSchema, DefaultDiscordConfig>
    {

    }
}
