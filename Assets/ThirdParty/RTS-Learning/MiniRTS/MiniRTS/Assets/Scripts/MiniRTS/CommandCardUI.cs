using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace MiniRTS
{
    /// <summary>
    /// Runtime-built command card, production queue, and selection panel.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class CommandCardUI : MonoBehaviour
    {
        private readonly List<CommandBinding> bindings =
            new List<CommandBinding>();

        private SelectionController selection;
        private PlayerEconomy economy;
        private Transform commandButtonRoot;
        private Text selectionText;
        private Text contextText;
        private Text queueText;
        private GameObject progressObject;
        private RectTransform progressFill;
        private GameObject tooltipPanel;
        private Text tooltipText;
        private int contextKey = int.MinValue;

        public void Initialize(
            SelectionController selectionController,
            PlayerEconomy playerEconomy)
        {
            selection = selectionController ??
                throw new ArgumentNullException(nameof(selectionController));
            economy = playerEconomy ??
                throw new ArgumentNullException(nameof(playerEconomy));
            EnsureEventSystem();
            BuildUi();
            RefreshContext();
            RefreshDynamicContent();
        }

        private void Update()
        {
            if (selection == null || economy == null)
            {
                return;
            }

            RefreshContext();
            RefreshDynamicContent();
        }

        private void BuildUi()
        {
            GameObject canvasObject = new GameObject(
                "CommandCanvas",
                typeof(RectTransform),
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster));
            canvasObject.transform.SetParent(transform, false);

            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 20;

            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;

            GameObject commandPanel = CreatePanel(
                "CommandCard",
                canvasObject.transform,
                new Vector2(1f, 0f),
                new Vector2(1f, 0f),
                new Vector2(1f, 0f),
                new Vector2(-18f, 18f),
                new Vector2(440f, 360f));
            contextText = CreateText(
                "Context",
                commandPanel.transform,
                new Vector2(16f, 316f),
                new Vector2(408f, 32f),
                20,
                TextAnchor.MiddleLeft,
                new Color(0.72f, 0.86f, 1f));
            contextText.fontStyle = FontStyle.Bold;

            GameObject buttonRoot = new GameObject(
                "CommandButtons",
                typeof(RectTransform),
                typeof(GridLayoutGroup));
            buttonRoot.transform.SetParent(commandPanel.transform, false);
            RectTransform buttonRect = buttonRoot.GetComponent<RectTransform>();
            buttonRect.anchorMin = Vector2.zero;
            buttonRect.anchorMax = Vector2.zero;
            buttonRect.pivot = Vector2.zero;
            buttonRect.anchoredPosition = new Vector2(16f, 76f);
            buttonRect.sizeDelta = new Vector2(408f, 232f);
            GridLayoutGroup gridLayout = buttonRoot.GetComponent<GridLayoutGroup>();
            gridLayout.cellSize = new Vector2(196f, 68f);
            gridLayout.spacing = new Vector2(12f, 10f);
            gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayout.constraintCount = 2;
            commandButtonRoot = buttonRoot.transform;

            queueText = CreateText(
                "QueueText",
                commandPanel.transform,
                new Vector2(16f, 38f),
                new Vector2(408f, 30f),
                17,
                TextAnchor.MiddleLeft,
                Color.white);
            progressObject = CreatePanel(
                "ProgressBack",
                commandPanel.transform,
                Vector2.zero,
                Vector2.zero,
                Vector2.zero,
                new Vector2(16f, 16f),
                new Vector2(408f, 16f));
            progressObject.GetComponent<Image>().color =
                new Color(0.02f, 0.035f, 0.055f, 0.95f);
            GameObject fill = CreatePanel(
                "ProgressFill",
                progressObject.transform,
                Vector2.zero,
                new Vector2(1f, 1f),
                Vector2.zero,
                Vector2.zero,
                Vector2.zero);
            fill.GetComponent<Image>().color = new Color(0.12f, 0.72f, 1f, 0.95f);
            progressFill = fill.GetComponent<RectTransform>();
            progressFill.offsetMin = new Vector2(2f, 2f);
            progressFill.offsetMax = new Vector2(-2f, -2f);

            GameObject selectionPanel = CreatePanel(
                "SelectionPanel",
                canvasObject.transform,
                new Vector2(0.5f, 0f),
                new Vector2(0.5f, 0f),
                new Vector2(0.5f, 0f),
                new Vector2(0f, 18f),
                new Vector2(640f, 112f));
            selectionText = CreateText(
                "SelectionText",
                selectionPanel.transform,
                new Vector2(20f, 12f),
                new Vector2(600f, 88f),
                21,
                TextAnchor.MiddleCenter,
                Color.white);

            tooltipPanel = CreatePanel(
                "CommandTooltip",
                canvasObject.transform,
                new Vector2(1f, 0f),
                new Vector2(1f, 0f),
                new Vector2(1f, 0f),
                new Vector2(-18f, 390f),
                new Vector2(440f, 66f));
            tooltipPanel.GetComponent<Image>().color =
                new Color(0.015f, 0.025f, 0.04f, 0.98f);
            tooltipText = CreateText(
                "TooltipText",
                tooltipPanel.transform,
                new Vector2(14f, 6f),
                new Vector2(412f, 54f),
                17,
                TextAnchor.MiddleLeft,
                new Color(0.9f, 0.94f, 1f));
            tooltipPanel.SetActive(false);
        }

        private void RefreshContext()
        {
            int newKey = CalculateContextKey();
            if (newKey == contextKey)
            {
                return;
            }

            contextKey = newKey;
            RebuildCommands();
        }

        private int CalculateContextKey()
        {
            Building building = selection.SelectedBuilding;
            if (building != null)
            {
                return unchecked(
                    building.GetInstanceID() * 2 +
                    (building.IsConstructed ? 1 : 0));
            }

            int unitContext = selection.HasSelectedWorker
                ? -2
                : selection.HasSelectedUnits
                    ? -3
                    : -1;
            return selection.IsAwaitingAttackMoveTarget
                ? unitContext - 10
                : unitContext;
        }

        private void RebuildCommands()
        {
            bindings.Clear();
            for (int i = commandButtonRoot.childCount - 1; i >= 0; i--)
            {
                GameObject child = commandButtonRoot.GetChild(i).gameObject;
                child.SetActive(false);
                Destroy(child);
            }

            Building building = selection.SelectedBuilding;
            if (building != null)
            {
                contextText.text = building.IsConstructed
                    ? building.DisplayName.ToUpperInvariant()
                    : $"{building.DisplayName.ToUpperInvariant()} — CONSTRUCTING";
                if (!building.IsConstructed)
                {
                    return;
                }

                if (building.Type == BuildingType.Headquarters)
                {
                    AddUnitButton(UnitType.Worker, "S");
                }
                else if (building.Type == BuildingType.Barracks)
                {
                    AddUnitButton(UnitType.Marine, "M");
                }
                else if (building.Type == BuildingType.Factory)
                {
                    AddUnitButton(UnitType.Tank, "T");
                }

                return;
            }

            if (selection.HasSelectedWorker)
            {
                contextText.text = selection.IsAwaitingAttackMoveTarget
                    ? "ATTACK-MOVE — CLICK GROUND"
                    : "WORKER COMMANDS";
                AddBuildingButton(BuildingType.SupplyDepot, "D");
                AddBuildingButton(BuildingType.Barracks, "B");
                AddBuildingButton(BuildingType.Factory, "F");
                AddBuildingButton(BuildingType.Refinery, "R");
            }
            else if (selection.HasSelectedUnits)
            {
                contextText.text = selection.IsAwaitingAttackMoveTarget
                    ? "ATTACK-MOVE — CLICK GROUND"
                    : "COMBAT COMMANDS";
            }
            else
            {
                contextText.text = "COMMANDS";
            }

            if (selection.HasSelectedUnits)
            {
                AddCombatButtons();
            }
        }

        private void AddCombatButtons()
        {
            CreateCommandButton(
                "Attack-Move\n[A]",
                "Attack-Move\nMove toward a point and engage enemies encountered.",
                () => selection.BeginAttackMoveTargeting());
            CreateCommandButton(
                "Stop / Hold\n[H]",
                "Stop / Hold\nCancel the current order and hold position.",
                () => selection.StopSelectedUnits());
        }

        private void AddBuildingButton(BuildingType type, string hotkey)
        {
            BuildingDefinition definition = BuildingDefinition.Get(type);
            Button button = CreateCommandButton(
                $"{definition.DisplayName}\n[{hotkey}]",
                $"{definition.DisplayName}\n" +
                FormatCost(definition.MineralCost, definition.GasCost, 0) +
                $" • {definition.BuildSeconds:0}s build • {definition.HitPoints} HP",
                () => selection.BeginBuildingPlacement(type));
            bindings.Add(CommandBinding.ForBuilding(button, type));
        }

        private void AddUnitButton(UnitType type, string hotkey)
        {
            UnitDefinition definition = UnitDefinition.Get(type);
            Button button = CreateCommandButton(
                $"Train {definition.DisplayName}\n[{hotkey}]",
                $"Train {definition.DisplayName}\n" +
                FormatCost(
                    definition.MineralCost,
                    definition.GasCost,
                    definition.SupplyCost) +
                $" • {definition.TrainingSeconds:0}s",
                () => selection.QueueSelectedUnit(type));
            bindings.Add(CommandBinding.ForUnit(button, type));
        }

        private Button CreateCommandButton(
            string label,
            string tooltip,
            UnityEngine.Events.UnityAction action)
        {
            GameObject buttonObject = new GameObject(
                "CommandButton",
                typeof(RectTransform),
                typeof(Image),
                typeof(Button),
                typeof(CommandTooltipTrigger));
            buttonObject.transform.SetParent(commandButtonRoot, false);
            Image image = buttonObject.GetComponent<Image>();
            image.color = new Color(0.08f, 0.16f, 0.25f, 0.98f);
            Button button = buttonObject.GetComponent<Button>();
            ColorBlock colors = button.colors;
            colors.normalColor = Color.white;
            colors.highlightedColor = new Color(1.16f, 1.16f, 1.16f);
            colors.pressedColor = new Color(0.65f, 0.78f, 0.9f);
            colors.disabledColor = new Color(0.35f, 0.35f, 0.35f, 0.78f);
            button.colors = colors;
            button.onClick.AddListener(action);

            Text buttonText = CreateText(
                "Label",
                buttonObject.transform,
                new Vector2(8f, 4f),
                new Vector2(180f, 60f),
                17,
                TextAnchor.MiddleCenter,
                Color.white);
            buttonText.fontStyle = FontStyle.Bold;
            buttonObject.GetComponent<CommandTooltipTrigger>().Initialize(
                tooltipPanel,
                tooltipText,
                tooltip);
            return button;
        }

        private void RefreshDynamicContent()
        {
            RefreshSelectionText();
            RefreshProgress();
            RefreshButtonStates();
        }

        private void RefreshSelectionText()
        {
            Building building = selection.SelectedBuilding;
            if (building != null)
            {
                selectionText.text = building.IsConstructed
                    ? $"{building.DisplayName}\n{building.HitPoints}/{building.MaxHitPoints} HP"
                    : $"{building.DisplayName} — Construction " +
                      $"{building.ConstructionProgress * 100f:0}%\n" +
                      $"{building.HitPoints}/{building.MaxHitPoints} HP";
                return;
            }

            int workers = 0;
            int marines = 0;
            int tanks = 0;
            IReadOnlyList<Unit> units = selection.SelectedUnits;
            if (units.Count == 1 && units[0] != null)
            {
                Unit onlyUnit = units[0];
                selectionText.text =
                    $"{onlyUnit.DisplayName}\n" +
                    $"{onlyUnit.HitPoints}/{onlyUnit.MaxHitPoints} HP";
                return;
            }

            for (int i = 0; i < units.Count; i++)
            {
                Unit unit = units[i];
                if (unit == null)
                {
                    continue;
                }

                switch (unit.Type)
                {
                    case UnitType.Worker:
                        workers++;
                        break;
                    case UnitType.Marine:
                        marines++;
                        break;
                    case UnitType.Tank:
                        tanks++;
                        break;
                }
            }

            StringBuilder text = new StringBuilder();
            AppendCount(text, "Worker", workers);
            AppendCount(text, "Marine", marines);
            AppendCount(text, "Tank", tanks);
            selectionText.text = text.Length > 0 ? text.ToString() : "No selection";
        }

        private void RefreshProgress()
        {
            Building building = selection.SelectedBuilding;
            float progress = 0f;
            string label = string.Empty;
            bool visible = false;

            if (building != null && !building.IsConstructed)
            {
                visible = true;
                progress = building.ConstructionProgress;
                label = $"CONSTRUCTION  {progress * 100f:0}%";
            }
            else if (building != null)
            {
                ProductionQueue queue = building.GetComponent<ProductionQueue>();
                if (queue != null)
                {
                    visible = true;
                    progress = queue.Progress;
                    StringBuilder queueLabel = new StringBuilder("QUEUE");
                    if (queue.Count == 0)
                    {
                        queueLabel.Append("  EMPTY");
                    }
                    else
                    {
                        queueLabel.Append("  ");
                        for (int i = 0; i < queue.Count; i++)
                        {
                            if (i > 0)
                            {
                                queueLabel.Append("  ");
                            }

                            queueLabel.Append('[');
                            queueLabel.Append(
                                UnitDefinition.Get(queue.GetQueuedType(i)).DisplayName);
                            queueLabel.Append(']');
                        }
                    }

                    label = queueLabel.ToString();
                }
            }

            queueText.gameObject.SetActive(visible);
            progressObject.SetActive(visible);
            if (visible)
            {
                queueText.text = label;
                progressFill.anchorMax = new Vector2(progress, 1f);
            }
        }

        private void RefreshButtonStates()
        {
            ProductionQueue queue = selection.SelectedBuilding != null
                ? selection.SelectedBuilding.GetComponent<ProductionQueue>()
                : null;
            for (int i = 0; i < bindings.Count; i++)
            {
                CommandBinding binding = bindings[i];
                if (binding.IsUnit)
                {
                    binding.Button.interactable =
                        queue != null && queue.CanTrain(binding.UnitType);
                }
                else
                {
                    BuildingDefinition definition =
                        BuildingDefinition.Get(binding.BuildingType);
                    binding.Button.interactable =
                        selection.HasSelectedWorker &&
                        economy.CanAfford(
                            definition.MineralCost,
                            definition.GasCost);
                }
            }
        }

        private static void AppendCount(
            StringBuilder builder,
            string displayName,
            int count)
        {
            if (count <= 0)
            {
                return;
            }

            if (builder.Length > 0)
            {
                builder.Append("     ");
            }

            builder.Append(displayName);
            builder.Append("  x");
            builder.Append(count);
        }

        private static string FormatCost(int minerals, int gas, int supply)
        {
            StringBuilder cost = new StringBuilder();
            cost.Append(minerals);
            cost.Append(" Minerals");
            if (gas > 0)
            {
                cost.Append(" • ");
                cost.Append(gas);
                cost.Append(" Gas");
            }

            if (supply > 0)
            {
                cost.Append(" • ");
                cost.Append(supply);
                cost.Append(" Supply");
            }

            return cost.ToString();
        }

        private static void EnsureEventSystem()
        {
            if (EventSystem.current != null)
            {
                return;
            }

            GameObject eventSystemObject = new GameObject(
                "EventSystem",
                typeof(EventSystem),
                typeof(InputSystemUIInputModule));
            eventSystemObject
                .GetComponent<InputSystemUIInputModule>()
                .AssignDefaultActions();
        }

        private static GameObject CreatePanel(
            string objectName,
            Transform parent,
            Vector2 anchorMin,
            Vector2 anchorMax,
            Vector2 pivot,
            Vector2 anchoredPosition,
            Vector2 sizeDelta)
        {
            GameObject panelObject = new GameObject(
                objectName,
                typeof(RectTransform),
                typeof(Image));
            panelObject.transform.SetParent(parent, false);
            RectTransform rect = panelObject.GetComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.pivot = pivot;
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = sizeDelta;
            panelObject.GetComponent<Image>().color =
                new Color(0.025f, 0.045f, 0.075f, 0.94f);
            return panelObject;
        }

        private static Text CreateText(
            string objectName,
            Transform parent,
            Vector2 anchoredPosition,
            Vector2 sizeDelta,
            int fontSize,
            TextAnchor alignment,
            Color color)
        {
            GameObject textObject = new GameObject(
                objectName,
                typeof(RectTransform),
                typeof(Text));
            textObject.transform.SetParent(parent, false);
            RectTransform rect = textObject.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.zero;
            rect.pivot = Vector2.zero;
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = sizeDelta;
            Text text = textObject.GetComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = fontSize;
            text.alignment = alignment;
            text.color = color;
            text.raycastTarget = false;
            return text;
        }

        private sealed class CommandBinding
        {
            public Button Button { get; private set; }
            public bool IsUnit { get; private set; }
            public UnitType UnitType { get; private set; }
            public BuildingType BuildingType { get; private set; }

            public static CommandBinding ForUnit(Button button, UnitType type)
            {
                return new CommandBinding
                {
                    Button = button,
                    IsUnit = true,
                    UnitType = type
                };
            }

            public static CommandBinding ForBuilding(
                Button button,
                BuildingType type)
            {
                return new CommandBinding
                {
                    Button = button,
                    IsUnit = false,
                    BuildingType = type
                };
            }
        }
    }
}
