using UnityEngine;

public class FogOfWar : MonoBehaviour
{
    public Texture2D fogTexture;
    public Color fogColor = new Color(0, 0, 0, 0.85f);
    public Color revealColor = new Color(0, 0, 0, 0f);

    public int resolution = 256;
    public float worldSize = 200f;

    void Start()
    {
        fogTexture = new Texture2D(resolution, resolution, TextureFormat.RGBA32, false);
        ClearFog();
        ApplyFog();
    }

    void ClearFog()
    {
        for (int x = 0; x < resolution; x++)
            for (int y = 0; y < resolution; y++)
                fogTexture.SetPixel(x, y, fogColor);
    }

    void ApplyFog()
    {
        fogTexture.Apply();
        GetComponent<Renderer>().material.mainTexture = fogTexture;
    }

    public void RevealArea(Vector3 worldPos, float radius, LayerMask losBlockers)
    {
        Vector2 texPos = WorldToTex(worldPos);

        int r = Mathf.RoundToInt(radius * (resolution / worldSize));

        for (int x = -r; x <= r; x++)
        {
            for (int y = -r; y <= r; y++)
            {
                int px = (int)texPos.x + x;
                int py = (int)texPos.y + y;

                if (px < 0 || px >= resolution || py < 0 || py >= resolution)
                    continue;

                Vector3 sampleWorld = TexToWorld(new Vector2(px, py));

                if (Vector3.Distance(worldPos, sampleWorld) > radius)
                    continue;

                if (!HasLineOfSight(worldPos, sampleWorld, losBlockers))
                    continue;

                fogTexture.SetPixel(px, py, revealColor);
            }
        }

        fogTexture.Apply();
    }

    bool HasLineOfSight(Vector3 from, Vector3 to, LayerMask blockers)
    {
        Vector3 dir = to - from;
        if (Physics.Raycast(from, dir.normalized, dir.magnitude, blockers))
            return false;

        return true;
    }

    Vector2 WorldToTex(Vector3 world)
    {
        float tx = (world.x / worldSize + 0.5f) * resolution;
        float ty = (world.z / worldSize + 0.5f) * resolution;
        return new Vector2(tx, ty);
    }

    Vector3 TexToWorld(Vector2 tex)
    {
        float wx = (tex.x / resolution - 0.5f) * worldSize;
        float wz = (tex.y / resolution - 0.5f) * worldSize;
        return new Vector3(wx, 0, wz);
    }
}
