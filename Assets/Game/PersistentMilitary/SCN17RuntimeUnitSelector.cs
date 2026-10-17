using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ObsidianProtocol.Game.Deployment;

namespace ObsidianProtocol.Game.PersistentMilitary
{
    public sealed class SCN17RuntimeUnitSelector : MonoBehaviour
    {
        private const string PanelName = "RUNTIME UNIT SELECTOR";

        private RectTransform content;
        private TextMeshProUGUI selectionStatus;
        private bool built;

        public void Build()
        {
            if (built)
            {
                Refresh();
                return;
            }

            PersistentMilitaryRuntime.Initialize();

            Transform existing = transform.Find(PanelName);
            if (existing != null)
            {
                Destroy(existing.gameObject);
            }

            GameObject panel = CreateUIObject(
                PanelName, transform, typeof(Image));

            RectTransform panelRect = panel.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0f, 0f);
            panelRect.anchorMax = new Vector2(0f, 0f);
            panelRect.pivot = new Vector2(0f, 0f);
            panelRect.anchoredPosition = new Vector2(24f, 24f);
            panelRect.sizeDelta = new Vector2(360f, 280f);

            panel.GetComponent<Image>().color =
                new Color(0.035f, 0.055f, 0.075f, 0.96f);

            TextMeshProUGUI title = CreateText(
                "ROSTER TITLE", panelRect,
                "PERSISTENT UNIT ROSTER", 18,
                new Vector2(12f, -8f),
                new Vector2(336f, 28f));

            title.fontStyle = FontStyles.Bold;

            selectionStatus = CreateText(
                "SELECTION STATUS", panelRect,
                "Select a unit to view its service record.",
                11, new Vector2(12f, -37f),
                new Vector2(336f, 22f));

            GameObject scrollObject = CreateUIObject(
                "ROSTER SCROLL VIEW", panelRect,
                typeof(Image), typeof(ScrollRect));

            RectTransform scrollRect =
                scrollObject.GetComponent<RectTransform>();

            scrollRect.anchorMin = new Vector2(0f, 0f);
            scrollRect.anchorMax = new Vector2(1f, 1f);
            scrollRect.offsetMin = new Vector2(10f, 10f);
            scrollRect.offsetMax = new Vector2(-10f, -66f);

            scrollObject.GetComponent<Image>().color =
                new Color(0.02f, 0.03f, 0.04f, 0.9f);

            GameObject viewportObject = CreateUIObject(
                "ROSTER VIEWPORT", scrollRect,
                typeof(Image), typeof(Mask));

            RectTransform viewport =
                viewportObject.GetComponent<RectTransform>();

            viewport.anchorMin = Vector2.zero;
            viewport.anchorMax = Vector2.one;
            viewport.offsetMin = Vector2.zero;
            viewport.offsetMax = Vector2.zero;

            viewportObject.GetComponent<Image>().color =
                Color.white;

            viewportObject.GetComponent<Mask>().showMaskGraphic = false;

            GameObject contentObject = CreateUIObject(
                "ROSTER CONTENT", viewport,
                typeof(VerticalLayoutGroup),
                typeof(ContentSizeFitter));

            content = contentObject.GetComponent<RectTransform>();
            content.anchorMin = new Vector2(0f, 1f);
            content.anchorMax = new Vector2(1f, 1f);
            content.pivot = new Vector2(0.5f, 1f);
            content.anchoredPosition = Vector2.zero;
            content.sizeDelta = Vector2.zero;

            VerticalLayoutGroup layout =
                contentObject.GetComponent<VerticalLayoutGroup>();

            layout.padding = new RectOffset(5, 5, 5, 5);
            layout.spacing = 4f;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            ContentSizeFitter fitter =
                contentObject.GetComponent<ContentSizeFitter>();

            fitter.horizontalFit =
                ContentSizeFitter.FitMode.Unconstrained;
            fitter.verticalFit =
                ContentSizeFitter.FitMode.PreferredSize;

            ScrollRect scroll =
                scrollObject.GetComponent<ScrollRect>();

            scroll.viewport = viewport;
            scroll.content = content;
            scroll.horizontal = false;
            scroll.vertical = true;

            built = true;
            Refresh();
        }

        public void SelectUnitFromButton(string buttonName)
        {
            const string prefix = "BUTTON - SELECT UNIT - ";

            if (string.IsNullOrWhiteSpace(buttonName) ||
                !buttonName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return;

            string unitId = buttonName.Substring(prefix.Length).Trim();
            if (string.IsNullOrWhiteSpace(unitId))
                return;

            PersistentMilitaryRuntime.Initialize();
            MilitaryUnitRecord record = PersistentMilitaryRuntime.GetUnit(unitId);

            if (record == null)
            {
                Debug.LogWarning("[SCN17] Unit record not found: " + unitId);
                return;
            }

            BattlefieldDeploymentContext context =
                BattlefieldDeploymentContext.Instance;

            if (context == null)
            {
                GameObject contextObject =
                    new GameObject("BATTLEFIELD DEPLOYMENT CONTEXT");
                context = contextObject.AddComponent<BattlefieldDeploymentContext>();
            }

            context.SetSelectedServiceRecord(record.UnitId);
            PersistentMilitaryRuntime.Save();
            Refresh();

            SCN17PersistentMilitaryUI controller =
                FindFirstObjectByType<SCN17PersistentMilitaryUI>();

            if (controller != null)
                controller.SendMessage("RefreshAll", SendMessageOptions.DontRequireReceiver);

            Debug.Log("[SCN17] UNIT SELECTED // " + record.UnitId);
        }
        public void Refresh()
        {
            if (content == null)
                return;

            for (int i = content.childCount - 1; i >= 0; i--)
                Destroy(content.GetChild(i).gameObject);

            PersistentMilitaryRuntime.Initialize();

            var data = PersistentMilitaryRuntime.Data;
            if (data == null || data.Units == null ||
                data.Units.Count == 0)
            {
                CreateRosterMessage("NO UNIT RECORDS FOUND");
                return;
            }

            string selectedId = string.Empty;

            BattlefieldDeploymentContext context =
                BattlefieldDeploymentContext.Instance;

            if (context != null)
                selectedId = context.SelectedServiceRecordId;

            foreach (MilitaryUnitRecord unit in data.Units)
            {
                if (unit == null || string.IsNullOrWhiteSpace(unit.UnitId))
                    continue;

                string unitId = unit.UnitId;
                string unitType = string.IsNullOrWhiteSpace(unit.UnitType)
                    ? "UNKNOWN TYPE"
                    : unit.UnitType;

                bool selected = string.Equals(
                    selectedId, unitId,
                    StringComparison.OrdinalIgnoreCase);

                CreateUnitButton(unitId, unitType, selected);
            }

            if (selectionStatus != null)
            {
                selectionStatus.text = string.IsNullOrEmpty(selectedId)
                    ? "No unit selected"
                    : "Selected: " + selectedId;
            }
        }

        private void CreateUnitButton(
            string unitId, string unitType, bool selected)
        {
            GameObject buttonObject = CreateUIObject(
                "BUTTON - SELECT UNIT - " + unitId,
                content, typeof(Image), typeof(Button));

            RectTransform rect =
                buttonObject.GetComponent<RectTransform>();

            rect.sizeDelta = new Vector2(0f, 34f);

            Image image = buttonObject.GetComponent<Image>();
            image.color = selected
                ? new Color(0.18f, 0.32f, 0.38f, 1f)
                : new Color(0.10f, 0.14f, 0.18f, 1f);

            LayoutElement element =
                buttonObject.AddComponent<LayoutElement>();

            element.minHeight = 34f;
            element.preferredHeight = 34f;

            CreateText(
                "UNIT LABEL", rect,
                unitType + "  |  " + unitId,
                12, new Vector2(8f, 0f),
                new Vector2(-16f, 0f));
        }

        private void CreateRosterMessage(string message)
        {
            CreateText(
                "EMPTY ROSTER MESSAGE", content,
                message, 12, Vector2.zero,
                new Vector2(0f, 30f));
        }

        private static GameObject CreateUIObject(
            string objectName, Transform parent,
            params Type[] components)
        {
            GameObject result = new GameObject(objectName, components);
            result.transform.SetParent(parent, false);
            return result;
        }

        private static TextMeshProUGUI CreateText(
            string objectName, Transform parent,
            string value, float fontSize,
            Vector2 position, Vector2 size)
        {
            GameObject textObject = CreateUIObject(
                objectName, parent, typeof(TextMeshProUGUI));

            RectTransform rect =
                textObject.GetComponent<RectTransform>();

            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.pivot = new Vector2(0f, 1f);
            rect.anchoredPosition = position;
            rect.sizeDelta = size;

            TextMeshProUGUI text =
                textObject.GetComponent<TextMeshProUGUI>();

            text.text = value;
            text.fontSize = fontSize;
            text.color = Color.white;
            text.alignment = TextAlignmentOptions.MidlineLeft;
            text.textWrappingMode = TMPro.TextWrappingModes.NoWrap;
            text.raycastTarget = false;

            if (TMP_Settings.defaultFontAsset != null)
                text.font = TMP_Settings.defaultFontAsset;

            return text;
        }
    }
}