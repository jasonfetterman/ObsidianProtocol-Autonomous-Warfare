using UnityEngine;

public class SaveLoadUI : MonoBehaviour
{
    public GameStateCollector collector;
    public GameStateLoader loader;

    public void SaveGame()
    {
        GameState state = collector.Collect();
        SaveSystem.Save(state);
    }

    public void LoadGame()
    {
        GameState state = SaveSystem.Load();
        if (state != null)
            loader.Load(state);
    }
}
