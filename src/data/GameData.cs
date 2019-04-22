using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

namespace Launcher.src.data
{
    public class GameData
    {
        private static String path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Configuration.ApplicationName, "data.xml");
        public ObservableCollection<Game> List { get; } = new ObservableCollection<Game>();
        private static GameData Instance;

        private GameData() { }

        public static GameData getInstance()
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

        private static GameData Load()
        {
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(GameData));
                using (StreamReader reader = new StreamReader(path))
                {
                    return deserializer.Deserialize(reader.BaseStream) as GameData;
                }
            } catch
            {
                return new GameData();
            }
        }
    }

}
