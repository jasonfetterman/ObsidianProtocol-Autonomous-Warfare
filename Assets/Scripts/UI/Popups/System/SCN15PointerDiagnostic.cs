using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ObsidianProtocol.UI.Popups.System
{
    public class SCN15PointerDiagnostic : MonoBehaviour
    {
        private PointerEventData pointerData;
        private readonly List<RaycastResult> results =
            new List<RaycastResult>();

        private string lastTopObject = "";

        private void Update()
        {
            if (EventSystem.current == null)
            {
                Debug.LogWarning(
                    "[SCN-15 INPUT] NO EVENT SYSTEM."
                );

                return;
            }

            pointerData =
                new PointerEventData(EventSystem.current)
                {
                    position = Input.mousePosition
                };

            results.Clear();

            EventSystem.current.RaycastAll(
                pointerData,
                results
            );

            string topName =
                results.Count > 0 &&
                results[0].gameObject != null
                    ? results[0].gameObject.name
                    : "NOTHING";

            if (topName != lastTopObject)
            {
                lastTopObject = topName;

                Debug.Log(
                    "[SCN-15 INPUT] POINTER=" +
                    Input.mousePosition +
                    " TOP=" +
                    topName +
                    " RAYCASTS=" +
                    results.Count
                );
            }
        }
    }
}
