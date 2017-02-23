using System;
using System.Collections.Generic;
using System.Text;

namespace TheGuin3.Config.Schema
{
    public class ModuleListConfigSchema
    {
        public List<string> Modules { get; set; }
    }

    public class DefualtModuleConfig : DefaultConfig
    {
        public DefualtModuleConfig()
        {
            DefaultContent = "{\n'Modules': ['core']\n";
        }
    }

    public class ModuleListConfig : ConfigInterface<ModuleListConfigSchema, DefualtModuleConfig>
    { }
}
