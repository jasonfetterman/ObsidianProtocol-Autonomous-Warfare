using System.IO;
using UnityEngine;

namespace ObsidianProtocol.Game.Core
{
    public static class SaveLoadService
    {
        private const string FileName = "obsidian_protocol_save.json";

        private static string SavePath =>
            Path.Combine(Application.persistentDataPath, FileName);

        public static void Save(SaveData data)
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(SavePath, json);
        }

        public static SaveData Load()
        {
            if (!File.Exists(SavePath))
            {
                return new SaveData();
            }

            string json = File.ReadAllText(SavePath);
            return JsonUtility.FromJson<SaveData>(json);
        }

        public static bool Exists()
        {
            return File.Exists(SavePath);
        }

        public static void Delete()
        {
            if (File.Exists(SavePath))
            {
                File.Delete(SavePath);
            }
        }
    }
}
