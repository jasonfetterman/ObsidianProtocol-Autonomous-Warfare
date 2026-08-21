using System.IO;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    public class GarageSaveManager : MonoBehaviour
    {
        [Header("Save Settings")]
        [SerializeField]
        private string saveFileName = "garage_save.json";

        public string SavePath =>
            Path.Combine(
                Application.persistentDataPath,
                saveFileName);

        public bool Save(GaragePersistenceState state)
        {
            if (state == null)
                return false;

            state.lastSavedTimestamp =
                System.DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            string json =
                JsonUtility.ToJson(state, true);

            File.WriteAllText(SavePath, json);

            return true;
        }

        public GaragePersistenceState Load()
        {
            if (!File.Exists(SavePath))
                return null;

            string json =
                File.ReadAllText(SavePath);

            if (string.IsNullOrWhiteSpace(json))
                return null;

            return JsonUtility.FromJson<GaragePersistenceState>(
                json);
        }

        public bool HasSave()
        {
            return File.Exists(SavePath);
        }

        public void DeleteSave()
        {
            if (File.Exists(SavePath))
                File.Delete(SavePath);
        }
    }
}
