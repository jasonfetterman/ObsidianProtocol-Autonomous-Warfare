using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ObsidianProtocol.Game.Campaign;

namespace ObsidianProtocol.UI
{
    public sealed class CampaignSetupController : MonoBehaviour
    {
        private static readonly string[] FactionIds =
        {
            "DOMINION_OF_ACHERON",
            "RIFTGUARD_COALITION",
            "OBSIDIAN_COMPACT",
            "ASHEN_SOVEREIGNTY",
            "OBSIDIAN_SIGNAL_CORPS",
            "BLACKGLASS_SHADOW_WING",
            "STONEFORGE_RUIN_GUARD",
            "OBSIDIAN_WAR_COUNCIL"
        };

        private static readonly string[] FactionNames =
        {
            "Dominion of Acheron",
            "Riftguard Coalition",
            "Obsidian Compact",
            "Ashen Sovereignty",
            "Obsidian Signal Corps",
            "Blackglass Shadow Wing",
            "Stoneforge Ruin Guard",
            "Obsidian War Council"
        };

        private InputField commanderNameInput;
        private Text factionInfo;
        private GameObject contentRoot;
        private int selectedFactionIndex;
        private Image[] factionOptionBackgrounds;
        private GameObject factionDropdownMenu;
        private Text factionDropdownCaption;
        private Button createButton;
        private bool isCreatingCampaign;


        public void Build()
        {
            if (contentRoot != null)
                return;

            contentRoot = new GameObject(
                "Campaign_Setup_Content",
                typeof(RectTransform),
                typeof(Image),
                typeof(Outline)
            );

            Image contentBackground = contentRoot.GetComponent<Image>();
            contentBackground.color = new Color(0.012f, 0.025f, 0.045f, 0.98f);
            contentBackground.raycastTarget = false;

            Outline contentOutline = contentRoot.GetComponent<Outline>();
            contentOutline.effectColor = new Color(0f, 0.72f, 0.95f, 0.9f);
            contentOutline.effectDistance = new Vector2(2f, 2f);

            contentRoot.transform.SetParent(transform, false);

            RectTransform root = contentRoot.GetComponent<RectTransform>();
            root.anchorMin = new Vector2(0.20f, 0.12f);
            root.anchorMax = new Vector2(0.80f, 0.78f);
            root.offsetMin = Vector2.zero;
            root.offsetMax = Vector2.zero;

            CreateLabel("Commander_Label", "COMMANDER NAME", 20, 0.76f, 0.88f);

            commanderNameInput = CreateInputField(
                "Commander_Name_Input",
                "COMMANDER",
                0.65f,
                0.75f
            );

            CreateLabel("Faction_Label", "SELECT FACTION", 18, 0.44f, 0.51f);

            CreateFactionDropdown();

            createButton = CreateButton(
                "CREATE_CAMPAIGN",
                "CREATE CAMPAIGN",
                0.08f,
                0.19f
            );
            createButton.onClick.AddListener(CreateCampaign);

            Button backButton = CreateButton(
                "BACK",
                "BACK",
                0.08f,
                0.19f
            );
            backButton.onClick.AddListener(Close);

            OnFactionSelected(0);
        }

        private Text CreateLabel(
            string objectName,
            string value,
            int fontSize,
            float yMin,
            float yMax)
        {
            GameObject obj = new GameObject(
                objectName,
                typeof(RectTransform),
                typeof(Text)
            );

            obj.transform.SetParent(contentRoot.transform, false);

            RectTransform rect = obj.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.08f, yMin);
            rect.anchorMax = new Vector2(0.92f, yMax);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            Text text = obj.GetComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.text = value;
            text.fontSize = fontSize;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = Color.white;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.raycastTarget = false;

            return text;
        }

        private InputField CreateInputField(
            string objectName,
            string defaultText,
            float yMin,
            float yMax)
        {
            GameObject obj = new GameObject(
                objectName,
                typeof(RectTransform),
                typeof(Image),
                typeof(InputField)
            );

            obj.transform.SetParent(contentRoot.transform, false);

            RectTransform rect = obj.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.18f, yMin);
            rect.anchorMax = new Vector2(0.82f, yMax);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            Image background = obj.GetComponent<Image>();
            background.color = new Color(0.015f, 0.025f, 0.035f, 0.90f);

            InputField input = obj.GetComponent<InputField>();

            GameObject textObject = new GameObject(
                "Text",
                typeof(RectTransform),
                typeof(Text)
            );
            textObject.transform.SetParent(obj.transform, false);

            RectTransform textRect = textObject.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(12f, 0f);
            textRect.offsetMax = new Vector2(-12f, 0f);

            Text text = textObject.GetComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 20;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleCenter;
            text.supportRichText = false;

            GameObject placeholderObject = new GameObject(
                "Placeholder",
                typeof(RectTransform),
                typeof(Text)
            );
            placeholderObject.transform.SetParent(obj.transform, false);

            RectTransform placeholderRect =
                placeholderObject.GetComponent<RectTransform>();
            placeholderRect.anchorMin = Vector2.zero;
            placeholderRect.anchorMax = Vector2.one;
            placeholderRect.offsetMin = new Vector2(12f, 0f);
            placeholderRect.offsetMax = new Vector2(-12f, 0f);

            Text placeholder = placeholderObject.GetComponent<Text>();
            placeholder.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            placeholder.fontSize = 20;
            placeholder.color = new Color(1f, 1f, 1f, 0.45f);
            placeholder.alignment = TextAnchor.MiddleCenter;
            placeholder.text = "COMMANDER";
            placeholder.raycastTarget = false;

            input.textComponent = text;
            input.placeholder = placeholder;
            input.text = defaultText;

            return input;
        }

        private void CreateFactionDropdown()
        {
            GameObject field = new GameObject(
                "Faction_Dropdown_Field",
                typeof(RectTransform),
                typeof(Image),
                typeof(Button)
            );
            field.transform.SetParent(contentRoot.transform, false);

            RectTransform fieldRect = field.GetComponent<RectTransform>();
            fieldRect.anchorMin = new Vector2(0.12f, 0.36f);
            fieldRect.anchorMax = new Vector2(0.88f, 0.43f);
            fieldRect.offsetMin = Vector2.zero;
            fieldRect.offsetMax = Vector2.zero;

            Image fieldImage = field.GetComponent<Image>();
            fieldImage.color = new Color(0.015f, 0.055f, 0.075f, 0.98f);

            Button fieldButton = field.GetComponent<Button>();
            fieldButton.targetGraphic = fieldImage;
            fieldButton.transition = Selectable.Transition.None;
            fieldButton.onClick.AddListener(ToggleFactionDropdown);

            GameObject captionObject = new GameObject(
                "Faction_Dropdown_Caption",
                typeof(RectTransform),
                typeof(Text)
            );
            captionObject.transform.SetParent(field.transform, false);

            RectTransform captionRect = captionObject.GetComponent<RectTransform>();
            captionRect.anchorMin = Vector2.zero;
            captionRect.anchorMax = Vector2.one;
            captionRect.offsetMin = new Vector2(12f, 0f);
            captionRect.offsetMax = new Vector2(-12f, 0f);

            factionDropdownCaption = captionObject.GetComponent<Text>();
            factionDropdownCaption.font =
                Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            factionDropdownCaption.fontSize = 17;
            factionDropdownCaption.alignment = TextAnchor.MiddleCenter;
            factionDropdownCaption.color = Color.white;
            factionDropdownCaption.raycastTarget = false;
            factionDropdownCaption.text = FactionNames[0] + "   ▼";

            factionDropdownMenu = new GameObject(
                "Faction_Dropdown_Menu",
                typeof(RectTransform),
                typeof(Image)
            );
            factionDropdownMenu.transform.SetParent(contentRoot.transform, false);

            RectTransform menuRect =
                factionDropdownMenu.GetComponent<RectTransform>();
            menuRect.anchorMin = new Vector2(0.12f, 0.43f);
            menuRect.anchorMax = new Vector2(0.88f, 0.83f);
            menuRect.offsetMin = Vector2.zero;
            menuRect.offsetMax = Vector2.zero;

            Image menuBackground = factionDropdownMenu.GetComponent<Image>();
            menuBackground.color = new Color(0.008f, 0.022f, 0.035f, 1f);
            menuBackground.raycastTarget = false;

            factionOptionBackgrounds = new Image[FactionNames.Length];

            for (int i = 0; i < FactionNames.Length; i++)
            {
                int index = i;

                GameObject option = new GameObject(
                    "Faction_Option_" + i,
                    typeof(RectTransform),
                    typeof(Image),
                    typeof(Button)
                );
                option.transform.SetParent(factionDropdownMenu.transform, false);

                RectTransform optionRect = option.GetComponent<RectTransform>();
                float rowTop = 1f - (i / (float)FactionNames.Length);
                float rowBottom = 1f - ((i + 1f) / FactionNames.Length);
                optionRect.anchorMin = new Vector2(0.015f, rowBottom);
                optionRect.anchorMax = new Vector2(0.985f, rowTop);
                optionRect.offsetMin = new Vector2(0f, 1f);
                optionRect.offsetMax = new Vector2(0f, -1f);

                Image optionImage = option.GetComponent<Image>();
                optionImage.color = i == 0
                    ? new Color(0f, 0.32f, 0.43f, 1f)
                    : new Color(0.015f, 0.055f, 0.075f, 1f);
                factionOptionBackgrounds[i] = optionImage;

                Button optionButton = option.GetComponent<Button>();
                optionButton.targetGraphic = optionImage;
                optionButton.transition = Selectable.Transition.None;
                optionButton.onClick.AddListener(
                    () => OnFactionSelected(index)
                );

                GameObject labelObject = new GameObject(
                    "Faction_Name",
                    typeof(RectTransform),
                    typeof(Text)
                );
                labelObject.transform.SetParent(option.transform, false);

                RectTransform labelRect =
                    labelObject.GetComponent<RectTransform>();
                labelRect.anchorMin = Vector2.zero;
                labelRect.anchorMax = Vector2.one;
                labelRect.offsetMin = new Vector2(8f, 0f);
                labelRect.offsetMax = new Vector2(-8f, 0f);

                Text label = labelObject.GetComponent<Text>();
                label.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                label.text = FactionNames[i];
                label.fontSize = 16;
                label.alignment = TextAnchor.MiddleCenter;
                label.color = Color.white;
                label.raycastTarget = false;
                label.horizontalOverflow = HorizontalWrapMode.Overflow;
                label.verticalOverflow = VerticalWrapMode.Overflow;
            }

            factionDropdownMenu.SetActive(false);
        }

        private void ToggleFactionDropdown()
        {
            if (factionDropdownMenu == null)
                return;

            bool showMenu = !factionDropdownMenu.activeSelf;
            factionDropdownMenu.SetActive(showMenu);

            if (showMenu)
                factionDropdownMenu.transform.SetAsLastSibling();
        }
        private void OnFactionSelected(int index)
        {
            if (index < 0 || index >= FactionNames.Length)
                return;

            selectedFactionIndex = index;

            if (factionDropdownCaption != null)
                factionDropdownCaption.text = FactionNames[index] + "   ▼";

            if (factionOptionBackgrounds != null)
            {
                for (int i = 0; i < factionOptionBackgrounds.Length; i++)
                {
                    Image background = factionOptionBackgrounds[i];
                    if (background == null)
                        continue;

                    background.color = i == selectedFactionIndex
                        ? new Color(0f, 0.32f, 0.43f, 1f)
                        : new Color(0.015f, 0.055f, 0.075f, 1f);
                }
            }

            if (factionDropdownMenu != null)
                factionDropdownMenu.SetActive(false);
        }
        private Button CreateButton(
            string objectName,
            string label,
            float yMin,
            float yMax)
        {
            GameObject obj = new GameObject(
                objectName,
                typeof(RectTransform),
                typeof(Image),
                typeof(Button)
            );

            obj.transform.SetParent(contentRoot.transform, false);

            RectTransform rect = obj.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(
                label == "CREATE CAMPAIGN" ? 0.08f : 0.56f,
                yMin
            );
            rect.anchorMax = new Vector2(
                label == "CREATE CAMPAIGN" ? 0.48f : 0.92f,
                yMax
            );
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            Image background = obj.GetComponent<Image>();
            background.color = new Color(0.015f, 0.025f, 0.035f, 0.90f);

            Button button = obj.GetComponent<Button>();
            button.targetGraphic = background;
            button.transition = Selectable.Transition.None;

            GameObject textObject = new GameObject(
                "Label",
                typeof(RectTransform),
                typeof(Text)
            );
            textObject.transform.SetParent(obj.transform, false);

            RectTransform textRect = textObject.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;

            Text text = textObject.GetComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.text = label;
            text.fontSize = 20;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = Color.white;
            text.raycastTarget = false;

            return button;
        }

        public void CreateCampaign()
        {
            string commanderName = commanderNameInput != null
                ? commanderNameInput.text
                : "COMMANDER";

            if (string.IsNullOrWhiteSpace(commanderName))
                commanderName = "COMMANDER";

            if (selectedFactionIndex < 0 ||
                selectedFactionIndex >= FactionIds.Length)
            {
                Debug.LogError("[CampaignSetup] Invalid faction selection.");
                return;
            }

            string factionId = FactionIds[selectedFactionIndex];

            if (isCreatingCampaign)
                return;

            isCreatingCampaign = true;
            if (createButton != null)
                createButton.interactable = false;

            CampaignState campaign = CampaignState.Instance;

            if (campaign == null)
            {
                GameObject campaignObject =
                    new GameObject("[SYSTEM] CAMPAIGN STATE");
                campaign = campaignObject.AddComponent<CampaignState>();
            }

            try
            {
                if (!campaign.CreateNewCampaign(commanderName, factionId))
                {
                    Debug.LogError("[CampaignSetup] Campaign initialization failed; scene transition cancelled.");
                    isCreatingCampaign = false;

                    if (createButton != null)
                        createButton.interactable = true;

                    return;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError(
                    "[CampaignSetup] Campaign initialization threw an exception; scene transition cancelled. " + ex
                );

                isCreatingCampaign = false;

                if (createButton != null)
                    createButton.interactable = true;

                return;
            }
            Debug.Log(
                "[CampaignSetup] CREATE CAMPAIGN COMPLETE | " +
                "COMMANDER=" + commanderName +
                " | FACTION=" + factionId
            );
            Debug.Log("[CampaignSetup] Loading Persistent Military building.");
            SceneManager.LoadScene(
                "Assets/Scenes/SCN17_PERSISTENT_MILITARY/SCN17_PERSISTENT_MILITARY.unity",
                LoadSceneMode.Single
            );
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}


