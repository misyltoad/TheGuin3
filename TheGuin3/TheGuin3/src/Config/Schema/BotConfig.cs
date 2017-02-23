using System;
using System.Collections.Generic;
using System.Text;

namespace TheGuin3.Config.Schema
{
    public class BotConfigSchema
    {
        public string CommandPrefix { get; set; }
    }

    public class DefaultBotConfig : DefaultConfig
    {
        public DefaultBotConfig()
        {
            DefaultContent = "{\n'CommandPrefix': '!'\n}";
        }
    }

    public class BotConfig : ConfigInterface<BotConfigSchema, DefaultBotConfig>
    {
    }
}
