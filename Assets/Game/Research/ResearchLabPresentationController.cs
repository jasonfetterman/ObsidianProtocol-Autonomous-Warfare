using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace ObsidianProtocol.Game.Research
{
    public sealed class ResearchLabPresentationController : MonoBehaviour
    {
        private GameObject researchHud;
        private Collider researchConsole;
        private Camera sceneCamera;
        private CharacterController playerController;

        private float pitch;
        private float verticalVelocity;
        private bool hudOpen;

        private const float MoveSpeed = 4f;
        private const float MouseSensitivity = 0.12f;
        private const float Gravity = -20f;
        private const float LookLimit = 80f;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RegisterSceneHandler()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "Research_Lab")
                return;

            GameObject cameraObject = GameObject.Find("RESEARCH LAB CAMERA");
            if (cameraObject == null)
            {
                Debug.LogError("[RESEARCH LAB] Scene camera not found.");
                return;
            }

            if (cameraObject.GetComponent<ResearchLabPresentationController>() == null)
                cameraObject.AddComponent<ResearchLabPresentationController>();
        }

        private void Awake()
        {
            sceneCamera = GetComponent<Camera>();
            researchHud = GameObject.Find("RESEARCH HUD");

            GameObject consoleObject = GameObject.Find("RESEARCH CONTROL CONSOLE 1");
            if (consoleObject != null)
                researchConsole = consoleObject.GetComponent<Collider>();

            if (researchHud != null)
                researchHud.SetActive(false);
            else
                Debug.LogError("[RESEARCH LAB] HUD root not found.");

            if (researchConsole == null)
                Debug.LogWarning("[RESEARCH LAB] Research console collider not found.");

            if (sceneCamera != null)
                SetupPlayer();
        }

        private void SetupPlayer()
        {
            Vector3 cameraStart = sceneCamera.transform.position;
            float startingYaw = sceneCamera.transform.eulerAngles.y;
            pitch = sceneCamera.transform.eulerAngles.x;
            if (pitch > 180f)
                pitch -= 360f;

            GameObject player = new GameObject("RESEARCH LAB PLAYER");
            player.transform.position = new Vector3(cameraStart.x, 0.9f, cameraStart.z);
            player.transform.rotation = Quaternion.Euler(0f, startingYaw, 0f);

            playerController = player.AddComponent<CharacterController>();
            playerController.height = 1.8f;
            playerController.radius = 0.35f;
            playerController.center = Vector3.zero;
            playerController.stepOffset = 0.3f;
            playerController.skinWidth = 0.08f;
            playerController.minMoveDistance = 0f;

            sceneCamera.transform.SetParent(player.transform, false);
            sceneCamera.transform.localPosition = new Vector3(0f, 0.7f, 0f);
            sceneCamera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            if (researchHud == null || sceneCamera == null)
                return;

            if (hudOpen)
            {
                if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
                {
                    CloseResearchHud();
                    return;
                }

                HandleTechnologyNodeSelection();
                return;
            }

            if (Keyboard.current != null &&
                Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            if (Mouse.current != null &&
                Mouse.current.leftButton.wasPressedThisFrame &&
                Cursor.lockState != CursorLockMode.Locked)
            {
                TryLockCursor();
                return;
            }

            if (Cursor.lockState != CursorLockMode.Locked)
                return;

            HandleLook();
            HandleMovement();
            HandleConsoleInteraction();
        }

        private void HandleLook()
        {
            if (Mouse.current == null)
                return;

            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            playerController.transform.Rotate(Vector3.up, mouseDelta.x * MouseSensitivity, Space.World);

            pitch -= mouseDelta.y * MouseSensitivity;
            pitch = Mathf.Clamp(pitch, -LookLimit, LookLimit);
            sceneCamera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }

        private void HandleMovement()
        {
            if (playerController == null || Keyboard.current == null)
                return;

            Vector3 input = Vector3.zero;

            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
                input.z += 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
                input.z -= 1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                input.x += 1f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                input.x -= 1f;

            input = Vector3.ClampMagnitude(input, 1f);
            Vector3 movement = playerController.transform.TransformDirection(input) * MoveSpeed;

            if (playerController.isGrounded && verticalVelocity < 0f)
                verticalVelocity = -2f;

            if (playerController.isGrounded && Keyboard.current.spaceKey.wasPressedThisFrame)
                verticalVelocity = 7f;

            verticalVelocity += Gravity * Time.deltaTime;
            movement.y = verticalVelocity;

            playerController.Move(movement * Time.deltaTime);
        }

        private void HandleConsoleInteraction()
        {
            bool interactPressed =
                (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame) ||
                (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame);

            if (!interactPressed)
                return;

            if (EventSystem.current != null &&
                EventSystem.current.IsPointerOverGameObject())
                return;

            Ray ray = sceneCamera.ScreenPointToRay(
                new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f));

            RaycastHit[] hits = Physics.RaycastAll(ray);
            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            foreach (RaycastHit hit in hits)
            {
                if (researchConsole != null &&
                    (hit.collider == researchConsole ||
                     hit.collider.transform.IsChildOf(researchConsole.transform)))
                {
                    OpenResearchHud();
                    return;
                }
            }
        }

        private void HandleTechnologyNodeSelection()
        {
            if (Mouse.current == null ||
                !Mouse.current.leftButton.wasPressedThisFrame)
                return;

            Vector2 pointerPosition = Mouse.current.position.ReadValue();
            Ray ray = sceneCamera.ScreenPointToRay(pointerPosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);
            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            foreach (RaycastHit hit in hits)
            {
                string nodeName = hit.collider.gameObject.name.ToUpperInvariant();
                if (!nodeName.Contains("TECH NODE"))
                    continue;

                var selectionController =
                    FindObjectOfType<ObsidianProtocol.UI.Research.ResearchSelectedController>();

                if (selectionController == null)
                {
                    Debug.LogError("[RESEARCH UI] Selection controller not found.");
                    return;
                }

                var technologies =
                    UnityEngine.Resources.LoadAll<ObsidianProtocol.Game.Technology.TechnologyDefinition>(
                        "Technology");

                ObsidianProtocol.Game.Technology.TechnologyDefinition selected = null;

                foreach (var technology in technologies)
                {
                    if (technology == null)
                        continue;

                    if ((!string.IsNullOrEmpty(technology.TechnologyId) &&
                         nodeName.Contains(technology.TechnologyId.ToUpperInvariant())) ||
                        (!string.IsNullOrEmpty(technology.DisplayName) &&
                         nodeName.Contains(technology.DisplayName.ToUpperInvariant())) ||
                        nodeName.Contains(technology.Category.ToString().ToUpperInvariant()))
                    {
                        selected = technology;
                        break;
                    }
                }

                if (selected == null && technologies.Length == 1)
                    selected = technologies[0];

                if (selected == null)
                {
                    Debug.LogWarning(
                        "[RESEARCH UI] No technology asset matches node: " + hit.collider.name);
                    return;
                }

                selectionController.SetSelectedTechnology(selected);
                selectionController.ViewSelectedTechnologyDetails();
                return;
            }
        }

        private void TryLockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OpenResearchHud()
        {
            hudOpen = true;
            researchHud.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void CloseResearchHud()
        {
            researchHud.SetActive(false);
            hudOpen = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnDestroy()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}


