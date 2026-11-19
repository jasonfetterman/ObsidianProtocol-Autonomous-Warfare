using UnityEngine;
using UnityEngine.UI;
using ObsidianProtocol.Game.Command;

public class VRCommandIntentPanelBuilder : MonoBehaviour
{
    [SerializeField] private VRCommandController commandController;
    [SerializeField] private CommandIntentManager intentManager;

    private void Start()
    {
        Build();
    }

    public void Build()
    {
        GameObject panel = GameObject.Find("[PANEL] COMMAND CENTER");

        if (panel == null)
        {
            Debug.LogError("[VR COMMAND] Command Center panel not found.");
            return;
        }

        if (intentManager == null)
            intentManager = FindFirstObjectByType<CommandIntentManager>();

        if (panel.transform.Find("[VR COMMAND] INTENT GRID") != null)
        {
            Debug.Log("[VR COMMAND] Intent grid already exists.");
            return;
        }

        GameObject gridObject = new GameObject(
            "[VR COMMAND] INTENT GRID",
            typeof(RectTransform),
            typeof(GridLayoutGroup)
        );

        gridObject.transform.SetParent(panel.transform, false);

        RectTransform rect = gridObject.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = new Vector2(0f, -80f);
        rect.sizeDelta = new Vector2(760f, 360f);

        GridLayoutGroup grid = gridObject.GetComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(220f, 65f);
        grid.spacing = new Vector2(20f, 18f);
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = 3;
        grid.childAlignment = TextAnchor.MiddleCenter;

        foreach (IntentType intent in System.Enum.GetValues(typeof(IntentType)))
        {
            CreateButton(gridObject.transform, intent);
        }

        Debug.Log("[VR COMMAND] Intent grid built.");
    }

    private void CreateButton(Transform parent, IntentType intent)
    {
        GameObject buttonObject = new GameObject(
            "[INTENT] " + intent,
            typeof(RectTransform),
            typeof(Image),
            typeof(Button),
            typeof(IntentButton)
        );

        buttonObject.transform.SetParent(parent, false);

        Button button = buttonObject.GetComponent<Button>();
        IntentButton intentButton = buttonObject.GetComponent<IntentButton>();

        var intentTypeField = typeof(IntentButton).GetField(
            "intentType",
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance
        );

        var intentManagerField = typeof(IntentButton).GetField(
            "intentManager",
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance
        );

        if (intentTypeField == null || intentManagerField == null)
        {
            Debug.LogError("[VR COMMAND] IntentButton fields could not be found.");
            return;
        }

        intentTypeField.SetValue(intentButton, intent);
        intentManagerField.SetValue(intentButton, intentManager);

        GameObject labelObject = new GameObject(
            "LABEL",
            typeof(RectTransform),
            typeof(Text)
        );

        labelObject.transform.SetParent(buttonObject.transform, false);

        RectTransform labelRect = labelObject.GetComponent<RectTransform>();
        labelRect.anchorMin = Vector2.zero;
        labelRect.anchorMax = Vector2.one;
        labelRect.offsetMin = Vector2.zero;
        labelRect.offsetMax = Vector2.zero;

        Text label = labelObject.GetComponent<Text>();
        label.text = intent.ToString().ToUpperInvariant();
        label.alignment = TextAnchor.MiddleCenter;
        label.fontSize = 22;
        label.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        button.targetGraphic = buttonObject.GetComponent<Image>();
    }
}
