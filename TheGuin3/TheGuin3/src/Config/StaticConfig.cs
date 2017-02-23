using System;
using System.Collections.Generic;
using System.Text;

namespace TheGuin3.Config
{
    public static class StaticConfig
    {
        public static class Paths
        {
            public static readonly string DataPath = "data";
            public static readonly string ConfigPath = DataPath + "/config";
            public static readonly string ModulesPath = DataPath + "/modules";
            public static readonly string TempPath = DataPath + "/temp";
        }
    }
}
