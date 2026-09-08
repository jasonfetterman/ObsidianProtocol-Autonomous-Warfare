using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MiniRTS
{
    /// <summary>
    /// Small runtime UGUI tooltip bridge used by command card buttons.
    /// </summary>
    public sealed class CommandTooltipTrigger :
        MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler
    {
        private GameObject tooltipPanel;
        private Text tooltipText;
        private string content;

        public void Initialize(
            GameObject panel,
            Text label,
            string tooltipContent)
        {
            tooltipPanel = panel;
            tooltipText = label;
            content = tooltipContent;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (tooltipPanel == null || tooltipText == null)
            {
                return;
            }

            tooltipText.text = content;
            tooltipPanel.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Hide();
        }

        private void OnDisable()
        {
            Hide();
        }

        private void Hide()
        {
            if (tooltipPanel != null)
            {
                tooltipPanel.SetActive(false);
            }
        }
    }
}
