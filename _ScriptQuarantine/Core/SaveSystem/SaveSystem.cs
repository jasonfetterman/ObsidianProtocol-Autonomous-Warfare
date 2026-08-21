using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void Save(GameState state)
    {
        string path = Application.persistentDataPath + "/save.dat";

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(path, FileMode.Create);

        bf.Serialize(fs, state);
        fs.Close();
    }

    public static GameState Load()
    {
        string path = Application.persistentDataPath + "/save.dat";

        if (!File.Exists(path))
            return null;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(path, FileMode.Open);

        GameState state = bf.Deserialize(fs) as GameState;
        fs.Close();

        return state;
    }
}
