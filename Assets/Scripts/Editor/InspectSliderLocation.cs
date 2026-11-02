#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class InspectSliderLocation
{
    [MenuItem("Tools/Inspect Master_Volume Slider")]
    public static void Inspect()
    {
        Transform[] allTransforms = Resources.FindObjectsOfTypeAll<Transform>();
        Transform mv = null;
        foreach (var t in allTransforms)
        {
            if (t.name == "Master_Volume" && t.gameObject.scene.isLoaded)
            {
                mv = t;
                break;
            }
        }

        if (mv == null)
        {
            Debug.LogError("Could not find Master_Volume anywhere in scene!");
            return;
        }

        Debug.Log($"Master_Volume found on GameObject: {mv.name}");
        Debug.Log($"  Parent: {mv.parent?.name}");
        Debug.Log($"  Slider on self: {mv.GetComponent<Slider>() != null}");
        Debug.Log($"  Slider on children: {mv.GetComponentInChildren<Slider>(true) != null}");
        Debug.Log($"  Slider on parent: {mv.GetComponentInParent<Slider>() != null}");
    }
}
#endif
