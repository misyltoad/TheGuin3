using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace TheGuin3.Config
{
    // YES I KNOW THIS IS BAD
    // YES IT WILL BE CACHED LATER

    public static class ConfigSystem
    {
        public static string GetText(string path)
        {
            try
            {
                return File.ReadAllText(String.Format("{0}/{1}", StaticConfig.Paths.ConfigPath, path));
            }
            catch
            {
                return null;
            }
        }

        public static List<T> DeserialiseFileToList<T>(string path)
        {
            try
            {
                string text = GetText(path);

                if (!String.IsNullOrWhiteSpace(text))
                {
                    JsonTextReader reader = new JsonTextReader(new StringReader(text));
                    reader.SupportMultipleContent = true;

                    List<T> thingies = new List<T>();

                    while (true)
                    {
                        if (!reader.Read())
                            break;

                        JsonSerializer serialiser = new JsonSerializer();
                        thingies.Add(serialiser.Deserialize<T>(reader));
                    }

                    return thingies;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return new List<T>();
        }


        public static T DeserialiseFile<T>(string path)
        {
            var list = DeserialiseFileToList<T>(path);
            if (list != null && list.Count > 0)
                return list[0];

            return default(T);
        }

        public static void SerialiseListToFile<T>(List<T> things, string path)
        {
            string content = "";

            foreach (var thing in things)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    StringWriter sw = new StringWriter(sb);

                    JsonWriter writer = new JsonTextWriter(sw);
                    writer.Formatting = Formatting.Indented;
                    JsonSerializer serialiser = new JsonSerializer();
                    serialiser.Serialize(writer, thing);

                    content += String.Format("{0}\n", sb.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            File.WriteAllText(String.Format("{0}/{1}", StaticConfig.Paths.ConfigPath, path), content);
        }

        public static void SerialiseToFile<T>(T thing, string path)
        {
            List<T> list = new List<T>();
            list.Add(thing);
            SerialiseListToFile(list, path);
        }
    }
}
