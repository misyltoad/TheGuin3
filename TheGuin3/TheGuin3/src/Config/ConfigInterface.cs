using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TheGuin3.Interfaces.Base;

namespace TheGuin3.Config
{
    public class DefaultConfig
    {
        public string DefaultContent;
    }

    public class ConfigInterface<Schema, Default> where Default : DefaultConfig, new()
    {
        public static void Set(Schema newSchema, Server server = null)
        {
            MakeDefault(server);

            ConfigSystem.SerialiseToFile<Schema>(newSchema, GetRelativePath(server));
        }

        public static void SetList(List<Schema> newSchema, Server server = null)
        {
            MakeDefault(server);

            ConfigSystem.SerialiseListToFile<Schema>(newSchema, GetRelativePath(server));
        }

        public static void Delete(Server server = null)
        {
            string path = GetAbsolutePath(server);

            try
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static string GetRelativePath(Server server = null)
        {
            if (server != null)
                return String.Format("{0}/{1}/{2}{3}", "server", server.Id, typeof(Schema).Name, ".json");
            else
                return String.Format("{0}{1}", typeof(Schema).Name, ".json");
        }

        private static string GetAbsolutePath(Server server = null)
        {
            return String.Format("{0}/{1}", StaticConfig.Paths.ConfigPath, GetRelativePath(server));
        }

        private static void MakeDefault(Server server = null)
        {
            string path = GetAbsolutePath(server);

            try
            {
                if (!File.Exists(path))
                    File.WriteAllText(path, new Default().DefaultContent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static List<Schema> GetList(Server server = null)
        {
            MakeDefault(server);

            return ConfigSystem.DeserialiseFileToList<Schema>(GetRelativePath(server));
        }

        public static Schema Get(Server server = null)
        {
            MakeDefault(server);

            return ConfigSystem.DeserialiseFile<Schema>(GetRelativePath(server));
        }
    }
}
