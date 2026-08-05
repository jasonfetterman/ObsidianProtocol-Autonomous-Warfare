using UnityEngine;

public class GameStateLoader : MonoBehaviour
{
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private FogOfWar fog;

    [SerializeField] private GameObject[] unitPrefabs;
    [SerializeField] private GameObject[] buildingPrefabs;

    public void Load(GameState state)
    {
        ClearWorld();

        if (resourceManager != null)
        {
            resourceManager.SetResource(ResourceType.Wood, state.wood);
            resourceManager.SetResource(ResourceType.Stone, state.stone);
            resourceManager.SetResource(ResourceType.Gold, state.gold);
        }

        LoadUnits(state);
        LoadBuildings(state);
        LoadFog(state);
    }

    private void ClearWorld()
    {
        UnitMover[] units = Object.FindObjectsByType<UnitMover>(FindObjectsInactive.Exclude);
        for (int i = 0; i < units.Length; i++)
            Destroy(units[i].gameObject);

        BuildingHealth[] buildings = Object.FindObjectsByType<BuildingHealth>(FindObjectsInactive.Exclude);
        for (int i = 0; i < buildings.Length; i++)
            Destroy(buildings[i].gameObject);
    }

    private void LoadUnits(GameState state)
    {
        for (int i = 0; i < state.units.Count; i++)
        {
            UnitData d = state.units[i];

            GameObject prefab = FindPrefab(unitPrefabs, d.prefabName);
            if (prefab == null)
                continue;

            Instantiate(prefab, d.position, d.rotation);
        }
    }

    private void LoadBuildings(GameState state)
    {
        for (int i = 0; i < state.buildings.Count; i++)
        {
            BuildingData d = state.buildings[i];

            GameObject prefab = FindPrefab(buildingPrefabs, d.prefabName);
            if (prefab == null)
                continue;

            GameObject b = Instantiate(prefab, d.position, d.rotation);

            // Safe, compatible, compiles everywhere
            BuildingHealth bh = b.GetComponent<BuildingHealth>();
            if (bh != null)
                bh.currentHealth = d.health;
        }
    }

    private void LoadFog(GameState state)
    {
        if (fog == null || fog.fogTexture == null || state.fogPixels == null)
            return;

        Color32[] pixels = fog.fogTexture.GetPixels32();

        int count = Mathf.Min(pixels.Length, state.fogPixels.Length);
        for (int i = 0; i < count; i++)
        {
            pixels[i].a = (byte)(state.fogPixels[i] * 255f);
        }

        fog.fogTexture.SetPixels32(pixels);
        fog.fogTexture.Apply();
    }

    private GameObject FindPrefab(GameObject[] prefabs, string name)
    {
        if (prefabs == null)
            return null;

        for (int i = 0; i < prefabs.Length; i++)
        {
            GameObject p = prefabs[i];
            if (p != null && p.name == name)
                return p;
        }

        return null;
    }
}
