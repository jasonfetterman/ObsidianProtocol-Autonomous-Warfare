using UnityEngine;
using System.Collections.Generic;

namespace ObsidianProtocol.UI
{
    /// <summary>
    /// Handles notifications. Only one notification is visible at a time.
    /// </summary>
    public sealed class UINotificationManager : MonoBehaviour
    {
        public static UINotificationManager Instance { get; private set; }

        private readonly Dictionary<string, GameObject> notifications = new Dictionary<string, GameObject>();
        private GameObject activeNotification;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        // ---------------------------------------------------------
        // REGISTRATION
        // ---------------------------------------------------------

        public void RegisterNotification(string id, GameObject notification)
        {
            if (string.IsNullOrEmpty(id) || notification == null)
            {
                Debug.LogError("[UINotificationManager] Invalid notification registration.");
                return;
            }

            notifications[id] = notification;
            notification.SetActive(false);
        }

        // ---------------------------------------------------------
        // CONTROL
        // ---------------------------------------------------------

        public void ShowNotification(string id)
        {
            if (!notifications.TryGetValue(id, out var notif))
            {
                Debug.LogError($"[UINotificationManager] Notification not found: {id}");
                return;
            }

            if (activeNotification != null)
                activeNotification.SetActive(false);

            activeNotification = notif;
            activeNotification.SetActive(true);
        }

        public void HideNotification()
        {
            if (activeNotification == null)
                return;

            activeNotification.SetActive(false);
            activeNotification = null;
        }
    }
}
