using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace ObsidianProtocol.Game.PersistentMilitary
{
    public class SCN17InputBootstrap : MonoBehaviour
    {
        private static SCN17InputBootstrap instance;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Install()
        {
            if (instance != null)
                return;

            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Persistent_Military" && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "SCN17_PERSISTENT_MILITARY")
                return;

            GameObject go = new GameObject("[SCN17] INPUT SYSTEM");
            instance = go.AddComponent<SCN17InputBootstrap>();

            DontDestroyOnLoad(go);

            EnsureEventSystem();

            go.AddComponent<SCN17MouseClickBridge>();

            Debug.Log("[SCN17] Direct mouse button bridge installed.");
        }

        private static void EnsureEventSystem()
        {
            EventSystem eventSystem = FindFirstObjectByType<EventSystem>();

            if (eventSystem == null)
            {
                GameObject go = new GameObject("EVENT SYSTEM");
                eventSystem = go.AddComponent<EventSystem>();
            }

            eventSystem.enabled = true;

            InputSystemUIInputModule module =
                eventSystem.GetComponent<InputSystemUIInputModule>();

            if (module != null)
            {
                module.enabled = false;
                Debug.Log(
                    "[SCN17] InputSystemUIInputModule disabled. Direct click bridge is active.");
            }
        }
    }

    public class SCN17MouseClickBridge : MonoBehaviour
    {
        private readonly List<GraphicRaycaster> raycasters =
            new List<GraphicRaycaster>();

        private readonly List<RaycastResult> results =
            new List<RaycastResult>();

        private EventSystem eventSystem;

        private string lastButtonName;
        private int lastClickFrame = -1;

        private void Awake()
        {
            eventSystem = FindFirstObjectByType<EventSystem>();
            RefreshRaycasters();
        }

        private void RefreshRaycasters()
        {
            raycasters.Clear();

            GraphicRaycaster[] found =
                FindObjectsByType<GraphicRaycaster>(
                    FindObjectsInactive.Exclude,
                    FindObjectsSortMode.None);

            raycasters.AddRange(found);

            Debug.Log(
                "[SCN17] Direct click bridge raycasters: " +
                raycasters.Count);
        }

        private void Update()
        {
            if (Mouse.current == null)
                return;

            if (!Mouse.current.leftButton.wasPressedThisFrame)
                return;

            if (eventSystem == null)
                eventSystem = FindFirstObjectByType<EventSystem>();

            if (eventSystem == null)
                return;

            Vector2 mousePosition =
                Mouse.current.position.ReadValue();

            results.Clear();

            PointerEventData pointer =
                new PointerEventData(eventSystem);

            pointer.position = mousePosition;

            foreach (GraphicRaycaster raycaster in raycasters)
            {
                if (raycaster == null || !raycaster.isActiveAndEnabled)
                    continue;

                raycaster.Raycast(pointer, results);
            }

            Button target = null;

            for (int i = 0; i < results.Count; i++)
            {
                GameObject hit = results[i].gameObject;

                if (hit == null)
                    continue;

                Button button =
                    hit.GetComponent<Button>();

                if (button == null)
                    button =
                        hit.GetComponentInParent<Button>();

                if (button == null)
                    continue;

                if (!button.isActiveAndEnabled)
                    continue;

                if (!button.interactable)
                    continue;

                if (!button.gameObject.name.StartsWith("BUTTON - "))
                    continue;

                target = button;
                break;
            }

            if (target == null)
                return;

            string buttonName =
                target.gameObject.name;

            if (lastClickFrame == Time.frameCount &&
                lastButtonName == buttonName)
            {
                return;
            }

            lastClickFrame = Time.frameCount;
            lastButtonName = buttonName;

            Debug.Log(
                "[SCN17 CLICK] " +
                buttonName);

            if (eventSystem != null)
                eventSystem.SetSelectedGameObject(target.gameObject);

            /*
             * EXACTLY EIGHT Persistent Unit Record buttons.
             * If one of these was clicked, the dedicated 8-panel
             * controller owns the action and the normal SCN17
             * controller is NOT called.
             */
            SCN17PersistentUnitRecordPanels recordPanels =
                FindFirstObjectByType<SCN17PersistentUnitRecordPanels>();

            if (recordPanels != null &&
                recordPanels.HandleButton(buttonName))
            {
                Debug.Log(
                    "[SCN17 RECORD ACTION] " +
                    buttonName +
                    " dispatched to Persistent Unit Record.");

                return;
            }

            /*
             * Everything else keeps the existing SCN17 routing.
             */
            SCN17PersistentMilitaryUI controller =
                FindFirstObjectByType<SCN17PersistentMilitaryUI>();

            if (controller != null)
            {
                controller.HandleExternalButton(buttonName);

                Debug.Log(
                    "[SCN17 ACTION] " +
                    buttonName +
                    " dispatched once.");
            }
            else
            {
                Debug.LogWarning(
                    "[SCN17] Persistent Military controller not found.");
            }
        }
    }
}

