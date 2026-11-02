using UnityEngine;

namespace ObsidianProtocol.Garage
{
    [DisallowMultipleComponent]
    public sealed class GarageTextSpinEffect : MonoBehaviour
    {
        [SerializeField] private float degreesPerSecond = 8f;

        private RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (rectTransform == null)
                return;

            rectTransform.Rotate(
                0f, 0f, degreesPerSecond * Time.unscaledDeltaTime,
                Space.Self);
        }
    }
}
