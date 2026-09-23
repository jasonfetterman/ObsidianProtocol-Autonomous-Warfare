using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

namespace ObsidianProtocol.Game.Intelligence
{
    public sealed class IntelligenceWorldInteraction : MonoBehaviour
    {
        [SerializeField] private Camera targetCamera;

        private void Awake()
        {
            if (targetCamera == null)
                targetCamera = GameObject.Find("INTELLIGENCE CAMERA")?.GetComponent<Camera>();
        }

        private void Update()
        {
            if (targetCamera == null ||
                Mouse.current == null ||
                (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) ||
                !Mouse.current.leftButton.wasPressedThisFrame)
                return;

            Ray ray = targetCamera.ScreenPointToRay(
                Mouse.current.position.ReadValue());

            RaycastHit[] hits = Physics.RaycastAll(ray);

            Debug.Log("[INTELLIGENCE CLICK] Ray hits: " + hits.Length);

            System.Array.Sort(
                hits,
                (a, b) => a.distance.CompareTo(b.distance));

            foreach (RaycastHit hit in hits)
            {
                Debug.Log(
                    "[INTELLIGENCE CLICK] HIT: " +
                    hit.collider.gameObject.name);

                IntelligenceSensorArrayTarget target =
                    hit.collider.GetComponent<IntelligenceSensorArrayTarget>();

                if (target != null)
                {
                    target.Select();
                    return;
                }
            }
        }
    }
}
