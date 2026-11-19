using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ObsidianProtocol.Game.Runtime;
using ObsidianProtocol.Game.Units;

namespace ObsidianProtocol.Game.Deployment
{
    public sealed class BattlefieldReinforcementSelector : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Button openButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private Transform contentRoot;
        [SerializeField] private Text pointsLabel;
        [SerializeField] private Text statusLabel;

        private BattlefieldReinforcementService service;

        private void Awake()
        {
            service = GetComponent<BattlefieldReinforcementService>();

            if (openButton != null)
                openButton.onClick.AddListener(Open);

            if (closeButton != null)
                closeButton.onClick.AddListener(Close);

            if (panel != null)
                panel.SetActive(false);
        }

        public void Open()
        {
            Refresh();
            if (panel != null)
                panel.SetActive(true);
        }

        public void Close()
        {
            if (panel != null)
                panel.SetActive(false);
        }

        public void Refresh()
        {
            BattlefieldDeploymentContext context =
                BattlefieldDeploymentContext.Instance;

            int remaining = context != null
                ? context.RemainingReinforcementPoints
                : 0;

            if (pointsLabel != null)
                pointsLabel.text = "REINFORCEMENT POINTS // " + remaining;

            ClearOptions();

            if (context == null || !context.ReinforcementsAllowed)
            {
                SetStatus("REINFORCEMENTS UNAVAILABLE");
                return;
            }

            if (service == null)
            {
                SetStatus("REINFORCEMENT SERVICE NOT FOUND");
                return;
            }

            if (!UnitDatabaseRuntime.EnsureInitialized() ||
                UnitDatabaseRuntime.Database == null)
            {
                SetStatus("UNIT DATABASE UNAVAILABLE");
                return;
            }

            int availableCount = 0;

            foreach (UnitDefinition definition in UnitDatabaseRuntime.Database.Units)
            {
                if (!IsEligible(definition, remaining))
                    continue;

                CreateOption(definition);
                availableCount++;
            }

            if (availableCount == 0)
                SetStatus("NO AVAILABLE UNITS WITHIN CURRENT POINTS");
            else
                SetStatus(availableCount + " REINFORCEMENT OPTIONS AVAILABLE");
        }

        private static bool IsEligible(UnitDefinition definition, int remaining)
        {
            return definition != null &&
                   !string.IsNullOrWhiteSpace(definition.UnitId) &&
                   definition.ModelPrefab != null &&
                   definition.Economy.deploymentPointsDefined &&
                   definition.Economy.deploymentPoints > 0 &&
                   definition.Economy.deploymentPoints <= remaining;
        }

        private void CreateOption(UnitDefinition definition)
        {
            GameObject option = new GameObject(
                "REINFORCEMENT // " + definition.UnitId,
                typeof(RectTransform),
                typeof(Image),
                typeof(Button),
                typeof(LayoutElement));

            option.transform.SetParent(contentRoot, false);

            Image background = option.GetComponent<Image>();
            background.color = new Color(0.08f, 0.14f, 0.19f, 0.96f);

            LayoutElement layout = option.GetComponent<LayoutElement>();
            layout.minHeight = 76f;
            layout.preferredHeight = 76f;

            Button button = option.GetComponent<Button>();
            button.targetGraphic = background;

            GameObject iconObject = new GameObject(
                "UNIT IMAGE", typeof(RectTransform), typeof(Image));
            iconObject.transform.SetParent(option.transform, false);

            RectTransform iconRect = iconObject.GetComponent<RectTransform>();
            iconRect.anchorMin = new Vector2(0f, 0.5f);
            iconRect.anchorMax = new Vector2(0f, 0.5f);
            iconRect.pivot = new Vector2(0f, 0.5f);
            iconRect.anchoredPosition = new Vector2(10f, 0f);
            iconRect.sizeDelta = new Vector2(56f, 56f);

            Image unitImage = iconObject.GetComponent<Image>();
            unitImage.preserveAspect = true;
            unitImage.sprite = definition.StoreImage;

            GameObject labelObject = new GameObject(
                "UNIT DETAILS", typeof(RectTransform), typeof(Text));
            labelObject.transform.SetParent(option.transform, false);

            RectTransform labelRect = labelObject.GetComponent<RectTransform>();
            labelRect.anchorMin = new Vector2(0f, 0f);
            labelRect.anchorMax = new Vector2(1f, 1f);
            labelRect.offsetMin = new Vector2(78f, 5f);
            labelRect.offsetMax = new Vector2(-10f, -5f);

            Text label = labelObject.GetComponent<Text>();
            label.font = Font.CreateDynamicFontFromOSFont("Arial", 15);
            label.fontSize = 15;
            label.color = Color.white;
            label.alignment = TextAnchor.MiddleLeft;
            label.horizontalOverflow = HorizontalWrapMode.Wrap;
            label.verticalOverflow = VerticalWrapMode.Overflow;
            label.text =
                definition.DisplayName + "\n" +
                definition.Category + " // " +
                definition.UnitId + "\n" +
                definition.Economy.deploymentPoints + " DP";

            string selectedUnitId = definition.UnitId;
            button.onClick.AddListener(() => Deploy(selectedUnitId));
        }

        private void Deploy(string unitId)
        {
            if (service == null)
                return;

            if (service.RequestReinforcement(unitId))
                Refresh();
        }

        private void ClearOptions()
        {
            if (contentRoot == null)
                return;

            for (int i = contentRoot.childCount - 1; i >= 0; i--)
                Destroy(contentRoot.GetChild(i).gameObject);
        }

        private void SetStatus(string message)
        {
            if (statusLabel != null)
                statusLabel.text = message;
        }
    }
}





