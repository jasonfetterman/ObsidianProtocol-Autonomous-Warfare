using UnityEngine;

public class MapManager : MonoBehaviour
{
    public MapDefinition[] maps;

    private MapLoader loader;   // pure C# → no SerializeField

    int currentMapIndex = 0;

    private void Awake()
    {
        loader = ServiceLocator.Get<MapLoader>();
    }

    public void LoadNextMap()
    {
        currentMapIndex++;
        if (currentMapIndex >= maps.Length)
            currentMapIndex = 0;

        loader.map = maps[currentMapIndex];
        ReloadScene();
    }

    public void LoadMap(int index)
    {
        if (index < 0 || index >= maps.Length) return;

        currentMapIndex = index;
        loader.map = maps[currentMapIndex];
        ReloadScene();
    }

    void ReloadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}
