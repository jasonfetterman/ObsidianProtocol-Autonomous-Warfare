using UnityEngine;

namespace ObsidianProtocol.UI
{
    public class UIInputManager : MonoBehaviour
    {
        public static UIInputManager Instance { get; private set; }

        [Header("Input Settings")]
        [SerializeField] private string submitButton = "Submit";
        [SerializeField] private string cancelButton = "Cancel";
        [SerializeField] private string horizontalAxis = "Horizontal";
        [SerializeField] private string verticalAxis = "Vertical";

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public bool GetSubmit()
        {
            return Input.GetButtonDown(submitButton);
        }

        public bool GetCancel()
        {
            return Input.GetButtonDown(cancelButton);
        }

        public float GetHorizontal()
        {
            return Input.GetAxis(horizontalAxis);
        }

        public float GetVertical()
        {
            return Input.GetAxis(verticalAxis);
        }
    }
}
