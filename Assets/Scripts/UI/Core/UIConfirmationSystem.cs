using UnityEngine;
using System;
using TMPro;

namespace ObsidianProtocol.UI
{
    public class UIConfirmationSystem : MonoBehaviour
    {
        public static UIConfirmationSystem Instance { get; private set; }

        [Header("Confirmation Window Root")]
        [SerializeField] private GameObject confirmationRoot;

        [Header("Confirmation Text Element")]
        [SerializeField] private TMP_Text confirmationText;

        private Action onConfirm;
        private Action onCancel;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            if (confirmationRoot != null)
                confirmationRoot.SetActive(false);
        }

        public void ShowConfirmation(string message, Action confirmCallback, Action cancelCallback)
        {
            if (confirmationRoot == null)
                return;

            onConfirm = confirmCallback;
            onCancel = cancelCallback;

            if (confirmationText != null)
                confirmationText.text = message;

            confirmationRoot.SetActive(true);
        }

        public void Confirm()
        {
            confirmationRoot.SetActive(false);

            onConfirm?.Invoke();
            onConfirm = null;
            onCancel = null;
        }

        public void Cancel()
        {
            confirmationRoot.SetActive(false);

            onCancel?.Invoke();
            onConfirm = null;
            onCancel = null;
        }

        public void Hide()
        {
            if (confirmationRoot != null)
                confirmationRoot.SetActive(false);

            onConfirm = null;
            onCancel = null;
        }
    }
}
