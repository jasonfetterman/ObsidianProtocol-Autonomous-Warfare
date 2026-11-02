using UnityEngine;
using UnityEngine.SceneManagement;

public class SCN17CyanOrangeGlowRuntime : MonoBehaviour
{
    void Awake()
    {
        Scene scene = gameObject.scene;
        if (!scene.IsValid() ||
            !scene.path.EndsWith("SCN17_PERSISTENT_MILITARY.unity"))
            return;

        foreach (GameObject root in scene.GetRootGameObjects())
        {
            Renderer[] renderers = root.GetComponentsInChildren<Renderer>(true);

            foreach (Renderer r in renderers)
            {
                if (!r || r.gameObject == gameObject) continue;

                // Keep window glass clear in Play mode; preserve frame glow.
                string objectName = r.gameObject.name;
                if (objectName.IndexOf("WINDOW", System.StringComparison.OrdinalIgnoreCase) >= 0 &&
                    objectName.IndexOf("GLASS", System.StringComparison.OrdinalIgnoreCase) >= 0)
                    continue;

                Material[] mats = r.materials;
                foreach (Material m in mats)
                {
                    if (!m) continue;

                    // Exclude ONLY the window glass material.
                    // All other cyan/orange materials keep their glow.
                    if (m.name.IndexOf("SCN17 Tinted Window Glass",
                        System.StringComparison.OrdinalIgnoreCase) >= 0)
                        continue;

                    string colorProperty = m.HasProperty("_BaseColor")
                        ? "_BaseColor"
                        : m.HasProperty("_Color") ? "_Color" : null;

                    if (colorProperty == null ||
                        !m.HasProperty("_EmissionColor")) continue;

                    Color c = m.GetColor(colorProperty);
                    bool cyan = c.g > c.r * 1.35f &&
                                c.b > c.r * 1.35f &&
                                c.g > 0.2f && c.b > 0.25f;

                    bool orange = c.r > c.g * 1.2f &&
                                  c.g > c.b * 1.2f &&
                                  c.r > 0.2f && c.g > 0.12f;

                    if (!cyan && !orange) continue;

                    m.EnableKeyword("_EMISSION");
                    m.SetColor("_EmissionColor", cyan
                        ? new Color(0.02f, 1.2f, 2.5f, 1f)
                        : new Color(2.5f, 0.8f, 0.08f, 1f));
                }
            }

            foreach (Light light in root.GetComponentsInChildren<Light>(true))
            {
                if (!light) continue;

                Color c = light.color;
                bool cyan = c.g > c.r * 1.35f &&
                            c.b > c.r * 1.35f &&
                            c.g > 0.2f && c.b > 0.25f;

                bool orange = c.r > c.g * 1.2f &&
                              c.g > c.b * 1.2f &&
                              c.r > 0.2f && c.g > 0.12f;

                if (cyan)
                {
                    light.color = new Color(0.02f, 0.75f, 1f, 1f);
                    light.intensity = Mathf.Max(light.intensity, 3f);
                }
                else if (orange)
                {
                    light.color = new Color(1f, 0.35f, 0.06f, 1f);
                    light.intensity = Mathf.Max(light.intensity, 3f);
                }
            }
        }
    }
}


