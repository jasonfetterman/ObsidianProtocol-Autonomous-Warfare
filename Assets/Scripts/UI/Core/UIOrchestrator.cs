using UnityEngine;

namespace ObsidianProtocol.UI
{
    public class UIOrchestrator : MonoBehaviour
    {
        public static UIOrchestrator Instance { get; private set; }

        [Header("Subsystems")]
        [SerializeField] private UIScreenManager screenManager;
        [SerializeField] private UILayerSystem layerSystem;
        [SerializeField] private UIStateSystem stateSystem;
        [SerializeField] private UINavigationSystem navigationSystem;
        [SerializeField] private UIFocusSystem focusSystem;
        [SerializeField] private UISelectionSystem selectionSystem;
        [SerializeField] private UIWindowManager windowManager;
        [SerializeField] private UIModalManager modalManager;
        [SerializeField] private UIPopupManager popupManager;
        [SerializeField] private UINotificationManager notificationManager;
        [SerializeField] private UITooltipSystem tooltipSystem;
        [SerializeField] private UIConfirmationSystem confirmationSystem;
        [SerializeField] private UIAnimationSystem animationSystem;
        [SerializeField] private UITransitionSystem transitionSystem;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            // Auto-bind subsystems if missing
            BindSubsystems();
        }

        private void BindSubsystems()
        {
            if (screenManager == null)
                screenManager = UIScreenManager.Instance;

            if (layerSystem == null)
                layerSystem = UILayerSystem.Instance;

            if (stateSystem == null)
                stateSystem = UIStateSystem.Instance;

            if (navigationSystem == null)
                navigationSystem = UINavigationSystem.Instance;

            if (focusSystem == null)
                focusSystem = UIFocusSystem.Instance;

            if (selectionSystem == null)
                selectionSystem = UISelectionSystem.Instance;

            if (windowManager == null)
                windowManager = UIWindowManager.Instance;

            if (modalManager == null)
                modalManager = UIModalManager.Instance;

            if (popupManager == null)
                popupManager = UIPopupManager.Instance;

            if (notificationManager == null)
                notificationManager = UINotificationManager.Instance;

            if (tooltipSystem == null)
                tooltipSystem = UITooltipSystem.Instance;

            if (confirmationSystem == null)
                confirmationSystem = UIConfirmationSystem.Instance;

            if (animationSystem == null)
                animationSystem = UIAnimationSystem.Instance;

            if (transitionSystem == null)
                transitionSystem = UITransitionSystem.Instance;
        }

        // ---------------------------------------------------------
        // HIGH-LEVEL UI CONTROL
        // ---------------------------------------------------------

        public void ShowScreen(string screenId)
        {
            screenManager.ShowScreen(screenId);
        }

        public void HideScreen(string screenId)
        {
            screenManager.HideScreen(screenId);
        }

        public void SetLayer(string layerId)
        {
            layerSystem.SetLayer(layerId);
        }

        public void SetState(string stateId)
        {
            stateSystem.SetState(stateId);
        }

        public void ClearState()
        {
            stateSystem.ClearState();
        }

        // ---------------------------------------------------------
        // NAVIGATION / SELECTION / FOCUS
        // ---------------------------------------------------------

        public void Select(GameObject target)
        {
            selectionSystem.Select(target);
        }

        public void Focus(GameObject target)
        {
            focusSystem.SetFocus(target);
        }

        public void ClearFocus()
        {
            focusSystem.ClearFocus();
        }

        public void NavigateBack()
        {
            navigationSystem.GoBack();
        }

        // ---------------------------------------------------------
        // WINDOWS / MODALS / POPUPS
        // ---------------------------------------------------------

        public void OpenWindow(string id)
        {
            windowManager.OpenWindow(id);
        }

        public void CloseWindow()
        {
            windowManager.CloseWindow();
        }

        public void OpenModal(string id)
        {
            modalManager.OpenModal(id);
        }

        public void CloseModal()
        {
            modalManager.CloseModal();
        }

        public void OpenPopup(string id)
        {
            popupManager.OpenPopup(id);
        }

        public void ClosePopup()
        {
            popupManager.ClosePopup();
        }

        // ---------------------------------------------------------
        // NOTIFICATIONS / TOOLTIP / CONFIRMATION
        // ---------------------------------------------------------

        public void Notify(string id)
        {
            notificationManager.ShowNotification(id);
        }

        public void HideNotification()
        {
            notificationManager.HideNotification();
        }

        public void Tooltip(string text)
        {
            tooltipSystem.ShowTooltip(text);
        }

        public void HideTooltip()
        {
            tooltipSystem.HideTooltip();
        }

        public void Confirm(string message, System.Action onConfirm, System.Action onCancel)
        {
            confirmationSystem.ShowConfirmation(message, onConfirm, onCancel);
        }

        // ---------------------------------------------------------
        // ANIMATION / TRANSITION
        // ---------------------------------------------------------

        public void FadeIn(CanvasGroup group)
        {
            animationSystem.FadeIn(group);
        }

        public void FadeOut(CanvasGroup group)
        {
            animationSystem.FadeOut(group);
        }

        public void Transition(CanvasGroup fromGroup, CanvasGroup toGroup)
        {
            transitionSystem.Transition(fromGroup, toGroup);
        }
    }
}
