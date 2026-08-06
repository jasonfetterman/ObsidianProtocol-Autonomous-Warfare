using UnityEngine;
using Obsidian.VR;   // REQUIRED — UnitMover lives here

public class GameStateCollector : MonoBehaviour
{
    public GameState Collect()
    {
        GameState state = new();

        // --- RESOURCES ---
        ResourceManager rm = ServiceLocator.Get<ResourceManager>();
        if (rm != null)
        {
            state.wood = rm.Get(ResourceType.Wood);
            state.stone = rm.Get(ResourceType.Stone);
            state.gold = rm.Get(ResourceType.Gold);
        }

        // --- UNITS ---
        UnitMover[] units = Object.FindObjectsByType<UnitMover>(FindObjectsInactive.Exclude);
        foreach (var u in units)
        {
            UnitData data = new()
            {
                prefabName = u.gameObject.name.Replace("(Clone)", "").Trim(),
                position = u.transform.position,
                rotation = u.transform.rotation
            };

            state.units.Add(data);
        }

        // --- BUILDINGS ---
        BuildingHealth[] buildings = Object.FindObjectsByType<BuildingHealth>(FindObjectsInactive.Exclude);
        foreach (var b in buildings)
        {
            BuildingData data = new()
            {
                prefabName = b.gameObject.name.Replace("(Clone)", "").Trim(),
                position = b.transform.position,
                rotation = b.transform.rotation,
                health = Mathf.RoundToInt(b.currentHealth)
            };

            state.buildings.Add(data);
        }

        // --- FOG ---
        FogOfWar fog = Object.FindAnyObjectByType<FogOfWar>();
        if (fog != null && fog.fogTexture != null)
        {
            Color32[] pixels = fog.fogTexture.GetPixels32();
            state.fogPixels = new float[pixels.Length];

            for (int i = 0; i < pixels.Length; i++)
            {
                state.fogPixels[i] = pixels[i].a / 255f;
            }
        }

        return state;
    }
}
