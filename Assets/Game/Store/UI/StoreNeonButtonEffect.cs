using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ObsidianProtocol.Game.Store.UI
{
    [RequireComponent(typeof(Graphic))]
    public sealed class StoreNeonButtonEffect : MonoBehaviour,
        IPointerEnterHandler, IPointerExitHandler,
        IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Color normalGlow = new Color(.02f, .45f, .65f, .55f);
        [SerializeField] private Color activeGlow = new Color(.05f, .9f, 1f, 1f);
        [SerializeField] private Vector2 normalDistance = new Vector2(2f, -2f);
        [SerializeField] private Vector2 activeDistance = new Vector2(4f, -4f);

        private Outline outline;
        private bool hovered;
        private bool selected;
        private bool pressed;

        private void Awake()
        {
            outline = GetComponent<Outline>();
            if (outline == null)
                outline = gameObject.AddComponent<Outline>();

            Apply();
        }

        public void SetSelected(bool value)
        {
            selected = value;
            Apply();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            hovered = true;
            Apply();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            hovered = false;
            pressed = false;
            Apply();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            pressed = true;
            Apply();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            pressed = false;
            Apply();
        }

        private void Apply()
        {
            if (outline == null) return;

            bool illuminated = hovered || selected || pressed;
            outline.effectColor = illuminated ? activeGlow : normalGlow;
            outline.effectDistance = illuminated ? activeDistance : normalDistance;
            outline.useGraphicAlpha = true;
        }
    }
}
