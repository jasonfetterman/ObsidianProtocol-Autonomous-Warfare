using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using System.Collections.Generic;

public class XRRuntimeProbe : MonoBehaviour
{
    void Start()
    {
        var gs = XRGeneralSettings.Instance;
        var mgr = gs != null ? gs.Manager : null;

        Debug.Log($"[XR PROBE] GeneralSettings={(gs != null)} Manager={(mgr != null)} Initialized={(mgr != null && mgr.isInitializationComplete)} Running={(mgr != null && mgr.isInitializationComplete && mgr.activeLoader != null)} Loader={(mgr != null && mgr.activeLoader != null ? mgr.activeLoader.GetType().FullName : "NULL")}");

        var displays = new List<XRDisplaySubsystem>();
        SubsystemManager.GetSubsystems(displays);
        Debug.Log($"[XR PROBE] DisplaySubsystems={displays.Count}");

        foreach (var d in displays)
            Debug.Log($"[XR PROBE] DISPLAY running={d.running}");

        var inputs = new List<XRInputSubsystem>();
        SubsystemManager.GetSubsystems(inputs);
        Debug.Log($"[XR PROBE] InputSubsystems={inputs.Count}");

        foreach (var i in inputs)
            Debug.Log($"[XR PROBE] INPUT running={i.running}");
    }
}
