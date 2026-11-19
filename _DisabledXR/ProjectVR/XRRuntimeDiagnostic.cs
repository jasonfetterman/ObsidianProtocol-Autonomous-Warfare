using UnityEngine;
using UnityEngine.XR.Management;

public class XRRuntimeDiagnostic : MonoBehaviour
{
    void Start()
    {
        var gs = XRGeneralSettings.Instance;

        Debug.Log("[XR DIAG] GeneralSettings = " + (gs != null));
        
        if (gs == null)
            return;

        var manager = gs.Manager;

        Debug.Log("[XR DIAG] Manager = " + (manager != null));

        if (manager == null)
            return;

        Debug.Log("[XR DIAG] InitializationComplete = " + manager.isInitializationComplete);
        Debug.Log("[XR DIAG] ActiveLoader = " + (manager.activeLoader != null ? manager.activeLoader.GetType().FullName : "NULL"));
        Debug.Log("[XR DIAG] LoaderCount = " + manager.loaders.Count);

        foreach (var loader in manager.loaders)
            Debug.Log("[XR DIAG] Loader: " + loader.GetType().FullName);
    }
}
