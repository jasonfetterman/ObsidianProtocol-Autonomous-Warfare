using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ObsidianProtocol.Game.Store.UI
{
    public sealed class StoreCategoryButton : MonoBehaviour
    {
        public string Category;
    }

    [RequireComponent(typeof(Image))]
    public sealed class StoreNeonButtonFeedback : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Image image;
        private Outline outline;
        private Color normal;
        private bool selected;
        private bool hovered;

        private static readonly Color Neon = new Color(0f, 0.85f, 1f, 1f);
        private static readonly Color Hover = new Color(0.02f, 0.28f, 0.36f, 1f);
        private static readonly Color Active = new Color(0.02f, 0.38f, 0.48f, 1f);

        private void Awake()
        {
            image = GetComponent<Image>();
            normal = image.color;
            outline = GetComponent<Outline>();
            if (outline == null) outline = gameObject.AddComponent<Outline>();
            outline.effectColor = Neon;
            outline.effectDistance = new Vector2(2f, 2f);
            outline.enabled = false;
        }

        public void SetSelected(bool value)
        {
            selected = value;
            Refresh();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            hovered = true;
            Refresh();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            hovered = false;
            Refresh();
        }

        private void Refresh()
        {
            if (image == null) return;
            image.color = selected ? Active : hovered ? Hover : normal;
            if (outline != null) outline.enabled = selected || hovered;
        }
    }
}
