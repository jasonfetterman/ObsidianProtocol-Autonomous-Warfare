using UnityEditor;
using UnityEditor.XR.Management;
using UnityEngine;
using UnityEngine.XR.Management;
using UnityEngine.XR.OpenXR;
using System.Collections.Generic;

public static class ForceOpenXRStandalone
{
    [MenuItem("Obsidian Protocol/Fix XR/Configure OpenXR Windows")]
    public static void Configure()
    {
        var buildTarget = BuildTargetGroup.Standalone;

        var general =
            XRGeneralSettingsPerBuildTarget.XRGeneralSettingsForBuildTarget(buildTarget);

        if (general == null)
        {
            Debug.LogError("[XR FIX] No Standalone XRGeneralSettings asset exists.");
            return;
        }

        var manager = general.Manager;

        if (manager == null)
        {
            Debug.LogError("[XR FIX] Standalone XRManagerSettings is NULL.");
            return;
        }

        var loaders = new List<XRLoader>();

        foreach (var loader in manager.activeLoaders)
        {
            if (loader != null)
                loaders.Add(loader);
        }

        bool hasOpenXR = false;

        foreach (var loader in loaders)
        {
            if (loader is OpenXRLoader)
            {
                hasOpenXR = true;
                break;
            }
        }

        if (!hasOpenXR)
        {
            var openXR = ScriptableObject.CreateInstance<OpenXRLoader>();
            openXR.name = "OpenXR Loader";
            AssetDatabase.AddObjectToAsset(openXR, manager);
            loaders.Insert(0, openXR);
        }

        manager.TrySetLoaders(loaders);

        general.InitManagerOnStart = true;

        EditorUtility.SetDirty(manager);
        EditorUtility.SetDirty(general);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("[XR FIX] OpenXR configured for Windows Standalone.");
        Debug.Log("[XR FIX] Active loader count: " + manager.activeLoaders.Count);

        foreach (var loader in manager.activeLoaders)
        {
            if (loader != null)
                Debug.Log("[XR FIX] Loader: " + loader.GetType().FullName);
        }
    }
}
