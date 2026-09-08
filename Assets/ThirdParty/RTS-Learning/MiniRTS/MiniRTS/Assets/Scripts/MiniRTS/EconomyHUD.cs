using UnityEngine;
using UnityEngine.UI;

namespace MiniRTS
{
    /// <summary>
    /// Runtime-built UGUI top bar for the local player's economy.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class EconomyHUD : MonoBehaviour
    {
        private PlayerEconomy economy;
        private Text resourceText;

        public void Initialize(PlayerEconomy playerEconomy)
        {
            economy = playerEconomy ??
                throw new System.ArgumentNullException(nameof(playerEconomy));
            BuildUi();
            economy.Changed += Refresh;
            Refresh();
        }

        private void OnDestroy()
        {
            if (economy != null)
            {
                economy.Changed -= Refresh;
            }
        }

        private void BuildUi()
        {
            GameObject canvasObject = new GameObject(
                "EconomyCanvas",
                typeof(RectTransform),
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster));
            canvasObject.transform.SetParent(transform, false);

            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 10;

            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;

            GameObject panelObject = new GameObject(
                "TopBar",
                typeof(RectTransform),
                typeof(Image));
            panelObject.transform.SetParent(canvasObject.transform, false);
            RectTransform panel = panelObject.GetComponent<RectTransform>();
            panel.anchorMin = new Vector2(0.5f, 1f);
            panel.anchorMax = new Vector2(0.5f, 1f);
            panel.pivot = new Vector2(0.5f, 1f);
            panel.anchoredPosition = new Vector2(0f, -12f);
            panel.sizeDelta = new Vector2(620f, 54f);
            panelObject.GetComponent<Image>().color =
                new Color(0.025f, 0.04f, 0.065f, 0.9f);

            GameObject textObject = new GameObject(
                "ResourceText",
                typeof(RectTransform),
                typeof(Text));
            textObject.transform.SetParent(panelObject.transform, false);
            RectTransform textRect = textObject.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(18f, 4f);
            textRect.offsetMax = new Vector2(-18f, -4f);

            resourceText = textObject.GetComponent<Text>();
            resourceText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            resourceText.fontSize = 22;
            resourceText.alignment = TextAnchor.MiddleCenter;
            resourceText.color = Color.white;
            resourceText.raycastTarget = false;
        }

        private void Refresh()
        {
            if (resourceText == null || economy == null)
            {
                return;
            }

            resourceText.text =
                $"MINERALS  {economy.Minerals}      " +
                $"GAS  {economy.Gas}      " +
                $"SUPPLY  {economy.SupplyUsed}/{economy.SupplyCap}";
        }
    }
}
