using UnityEngine;

namespace Obsidian.UI
{
    public class SelectionBox : MonoBehaviour
    {
        public Texture2D boxTexture;
        public Color boxColor = new Color(0f, 0.8f, 1f, 0.25f);

        private Vector2 startPos;
        private Vector2 endPos;
        private bool isDragging;

        void OnGUI()
        {
            if (!isDragging)
                return;

            var rect = GetRect(startPos, endPos);

            GUI.color = boxColor;
            GUI.DrawTexture(rect, boxTexture);
        }

        public void Begin(Vector2 screenPos)
        {
            startPos = screenPos;
            endPos = screenPos;
            isDragging = true;
        }

        public void UpdateDrag(Vector2 screenPos)
        {
            endPos = screenPos;
        }

        public void End()
        {
            isDragging = false;
        }

        public Rect GetSelectionRect()
        {
            return GetRect(startPos, endPos);
        }

        private Rect GetRect(Vector2 p1, Vector2 p2)
        {
            float x = Mathf.Min(p1.x, p2.x);
            float y = Mathf.Min(p1.y, p2.y);
            float w = Mathf.Abs(p1.x - p2.x);
            float h = Mathf.Abs(p1.y - p2.y);

            return new Rect(x, y, w, h);
        }
    }
}
