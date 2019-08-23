using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Launcher.src.data
{
    public class ConfigurationData
    {
        private static String path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Configuration.ApplicationName, "config.xml");

        public List<String> Tags = new List<string>();

        private static ConfigurationData Instance;

        private ConfigurationData() { }
        public static ConfigurationData getInstance()
        {
            if (Instance == null)
                Instance = Load();
            return Instance;
        }

        public void Save()
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            using (StreamWriter writer = new StreamWriter(path))
            {
                serializer.Serialize(writer.BaseStream, this);
            }
        }

        private static ConfigurationData Load()
        {
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(ConfigurationData));
                using (StreamReader reader = new StreamReader(path))
                {
                    return deserializer.Deserialize(reader.BaseStream) as ConfigurationData;
                }
            }
            catch
            {
                return new ConfigurationData();
            }
        }
    }
}
