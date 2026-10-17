using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public sealed class CommanderProfileController : MonoBehaviour
{
    private readonly List<GameObject> pages = new List<GameObject>();

    private GameObject doctrineWindow;
    private GameObject protocolPopup;
    private readonly List<Button> doctrineCards = new List<Button>();
    private readonly string[] doctrineModeButtonNames =
    {
        "DOCTRINE_MODE_BALANCED",
        "DOCTRINE_MODE_AGGRESSIVE",
        "DOCTRINE_MODE_DEFENSIVE",
        "DOCTRINE_MODE_RECONNAISSANCE",
        "DOCTRINE_MODE_LOGISTICS",
        "DOCTRINE_MODE_AUTONOMOUS",
        "DOCTRINE_MODE_HOLD_POSITION",
        "DOCTRINE_MODE_OBJECTIVE_FOCUSED"
    };
    private readonly string[] doctrineNames =
    {
        "BALANCED",
        "AGGRESSIVE",
        "DEFENSIVE",
        "RECONNAISSANCE",
        "LOGISTICS",
        "AUTONOMOUS",
        "HOLD POSITION",
        "OBJECTIVE FOCUSED"
    };

    private readonly string[] doctrineDescriptions =
    {
        "A flexible combined-arms doctrine emphasizing adaptability, controlled risk, and balanced employment of combat, reconnaissance, and sustainment forces.",

        "An offensive doctrine emphasizing initiative, operational tempo, concentration of combat power, and rapid exploitation of enemy weaknesses.",

        "A force-protection doctrine emphasizing defensive positions, controlled engagement ranges, survivability, and deliberate counteraction.",

        "An information-focused doctrine emphasizing reconnaissance, surveillance, sensor coverage, target identification, and preservation of situational awareness.",

        "A sustainment-focused doctrine emphasizing supply security, repair capacity, recovery operations, ammunition management, and operational endurance.",

        "A delegated-command doctrine emphasizing autonomous unit decision-making within defined rules of engagement, mission boundaries, and commander intent.",

        "A positional doctrine emphasizing preservation of designated terrain, defensive sectors, controlled engagement, and resistance to displacement.",

        "A mission-focused doctrine emphasizing completion of assigned objectives while reducing unnecessary engagement with targets that do not affect mission success."
    };

    private readonly string[][] doctrinePrinciples =
    {
        new string[]
        {
            "ADAPTABILITY // Adjust force employment to changing conditions.",
            "COMBINED ARMS // Coordinate combat capabilities toward a common effect.",
            "CONTROLLED RISK // Preserve combat power while pursuing mission objectives.",
            "SITUATIONAL AWARENESS // Maintain a current operational picture.",
            "SUSTAINMENT // Preserve the force through reliable support.",
            "COMMANDER INTENT // Maintain flexibility within defined mission boundaries."
        },

        new string[]
        {
            "INITIATIVE // Seize and retain operational initiative.",
            "TEMPO // Maintain pressure and compress enemy reaction time.",
            "CONCENTRATION // Focus combat power against decisive objectives.",
            "EXPLOITATION // Expand opportunities created by successful action.",
            "FORWARD MOMENTUM // Minimize unnecessary pauses during offensive operations.",
            "RISK ACCEPTANCE // Accept calculated exposure to achieve decisive effects."
        },

        new string[]
        {
            "FORCE PROTECTION // Preserve personnel and critical systems.",
            "POSITION // Use terrain and prepared locations to increase survivability.",
            "CONTROLLED ENGAGEMENT // Engage when conditions favor the defending force.",
            "RESERVE // Preserve combat power for decisive response.",
            "COUNTERACTION // Conduct deliberate counterattacks when required.",
            "ENDURANCE // Maintain defensive capability over extended operations."
        },

        new string[]
        {
            "RECONNAISSANCE // Locate and identify relevant forces and conditions.",
            "SURVEILLANCE // Maintain persistent observation of areas of interest.",
            "IDENTIFICATION // Reduce uncertainty before committing combat power.",
            "INFORMATION // Protect and distribute actionable battlefield data.",
            "CONTACT DISCIPLINE // Avoid unnecessary exposure while gathering information.",
            "DECISION ADVANTAGE // Convert information into timely action."
        },

        new string[]
        {
            "SUPPLY // Maintain reliable movement of essential resources.",
            "RECOVERY // Restore damaged systems and recover disabled units.",
            "MAINTENANCE // Preserve equipment readiness through planned servicing.",
            "DISTRIBUTION // Position resources where they are required.",
            "RESERVE STOCKS // Maintain capacity for unexpected demand.",
            "ENDURANCE // Extend operational capability through sustained support."
        },

        new string[]
        {
            "DELEGATED AUTHORITY // Permit units to act within assigned limits.",
            "RULES OF ENGAGEMENT // Constrain autonomous action through explicit controls.",
            "COMMANDER INTENT // Provide the decision framework for autonomous behavior.",
            "HUMAN OVERSIGHT // Preserve commander authority over critical decisions.",
            "ADAPTIVE RESPONSE // Allow local decisions when communications are limited.",
            "FAILSAFE // Return autonomous behavior to defined safe conditions."
        },

        new string[]
        {
            "POSITION CONTROL // Preserve the assigned area of responsibility.",
            "TERRAIN HOLD // Maintain control of designated ground.",
            "DEFENSIVE SECTOR // Establish clear boundaries for local defense.",
            "ENGAGEMENT CONTROL // Avoid unnecessary movement or exposure.",
            "RESERVE CAPACITY // Preserve the ability to respond to threats.",
            "DENIAL // Prevent opposing forces from achieving positional objectives."
        },

        new string[]
        {
            "MISSION OBJECTIVE // Prioritize the assigned operational objective.",
            "OBJECTIVE PROGRESS // Measure actions against mission completion.",
            "TARGET DISCIPLINE // Engage targets that affect mission success.",
            "RESOURCE FOCUS // Direct available capability toward the objective.",
            "ADAPTATION // Adjust execution when conditions change.",
            "COMPLETION // Transition from combat activity when the objective is achieved."
        }
    };
    private readonly string[] doctrineCardNames =
    {
        "BALANCED",
        "AGGRESSIVE",
        "DEFENSIVE",
        "RECONNAISSANCE",
        "LOGISTICS",
        "AUTONOMOUS",
        "HOLD POSITION",
        "OBJECTIVE FOCUSED"
    };

    private readonly string[] doctrineCardStates =
    {
        "AVAILABLE // ADAPTIVE",
        "AVAILABLE // OFFENSIVE",
        "AVAILABLE // DEFENSIVE",
        "AVAILABLE // RECON",
        "AVAILABLE // SUSTAINMENT",
        "AVAILABLE // AUTONOMY",
        "AVAILABLE // POSITION",
        "AVAILABLE // MISSION"
    };
    private readonly string[] doctrineStatuses =
    {
        "ACTIVE // ADAPTIVE FORCE EMPLOYMENT",
        "ACTIVE // OFFENSIVE TEMPO",
        "ACTIVE // FORCE PROTECTION",
        "ACTIVE // INFORMATION SUPERIORITY",
        "ACTIVE // OPERATIONAL SUSTAINMENT",
        "ACTIVE // DELEGATED AUTONOMY",
        "ACTIVE // POSITION PRESERVATION",
        "ACTIVE // MISSION PRIORITY"
    };


    private readonly string[] pageNames =
    {
        "PAGE_PROFILE",
        "PAGE_RANK",
        "PAGE_PROGRESSION",
        "PAGE_DOCTRINE",
        "PAGE_STATISTICS",
        "PAGE_ACHIEVEMENTS",
        "PAGE_CAREER"
    };

    private readonly string[] navigationButtons =
    {
        "BUTTON_NAV_PROFILE",
        "BUTTON_NAV_RANK",
        "BUTTON_NAV_PROGRESSION",
        "BUTTON_NAV_DOCTRINE",
        "BUTTON_NAV_STATISTICS",
        "BUTTON_NAV_ACHIEVEMENTS",
        "BUTTON_NAV_CAREER"
    };

    private readonly string[] doctrineModeButtons =
    {
        "DOCTRINE_MODE_BALANCED",
        "DOCTRINE_MODE_AGGRESSIVE",
        "DOCTRINE_MODE_DEFENSIVE",
        "DOCTRINE_MODE_RECONNAISSANCE",
        "DOCTRINE_MODE_LOGISTICS",
        "DOCTRINE_MODE_AUTONOMOUS",
        "DOCTRINE_MODE_HOLD_POSITION",
        "DOCTRINE_MODE_OBJECTIVE_FOCUSED"
    };

    private readonly string[] doctrineModeNames =
    {
        "BALANCED",
        "AGGRESSIVE",
        "DEFENSIVE",
        "RECONNAISSANCE",
        "LOGISTICS",
        "AUTONOMOUS",
        "HOLD POSITION",
        "OBJECTIVE FOCUSED"
    };

    private int activePage;
    private int activeDoctrineMode;

    private void Awake()
    {
        CachePages();
        CacheDoctrineWindow();
        CacheDoctrineCards();

        WireNavigation();
        WireDoctrineModes();
        WireProtocolButton();

        ShowPage(0);
        CloseDoctrine();
        CloseProtocolPopup();

        SetDoctrineMode(0);
    }

    private void CachePages()
    {
        pages.Clear();

        foreach (string pageName in pageNames)
        {
            GameObject page = GameObject.Find(pageName);

            if (page != null)
                pages.Add(page);
            else
                Debug.LogWarning("[COMMANDER PROFILE] Missing page: " + pageName);
        }
    }

    private void CacheDoctrineWindow()
    {
        doctrineWindow = GameObject.Find("WINDOW_DOCTRINE_LIBRARY");

        if (doctrineWindow == null)
            Debug.LogWarning("[COMMANDER PROFILE] Missing WINDOW_DOCTRINE_LIBRARY");
    }

    private void CacheDoctrineCards()
    {
        doctrineCards.Clear();

        GameObject canvas = GameObject.Find("Canvas_CommanderProfileHUD");

        if (canvas == null)
        {
            Debug.LogError("[COMMANDER PROFILE] Canvas_CommanderProfileHUD not found.");
            return;
        }

        Transform window = null;

        Transform[] allTransforms =
            canvas.GetComponentsInChildren<Transform>(true);

        foreach (Transform t in allTransforms)
        {
            if (t.name == "WINDOW_DOCTRINE_LIBRARY")
            {
                window = t;
                break;
            }
        }

        if (window == null)
        {
            Debug.LogError("[COMMANDER PROFILE] WINDOW_DOCTRINE_LIBRARY not found.");
            return;
        }

        /*
         * Disable raycast blocking on every graphic inside the
         * doctrine library except the actual card images and the
         * Close button.
         *
         * The original hierarchy contains text and decorative
         * images as siblings of the DOCTRINE_CARD objects.
         * Those can otherwise consume the mouse click.
         */
        Graphic[] graphics =
            window.GetComponentsInChildren<Graphic>(true);

        foreach (Graphic graphic in graphics)
        {
            bool keepRaycast =
                graphic.gameObject.name == "DOCTRINE_CARD" ||
                graphic.gameObject.name == "BUTTON_DOCTRINE_CLOSE";

            graphic.raycastTarget = keepRaycast;
        }

        Transform[] children =
            window.GetComponentsInChildren<Transform>(true);

        int cardIndex = 0;

        List<Transform> doctrineNameLabels = new List<Transform>();
        List<Transform> doctrineStateLabels = new List<Transform>();

        foreach (Transform t in children)
        {
            if (t.name == "DOCTRINE_NAME")
                doctrineNameLabels.Add(t);

            if (t.name == "DOCTRINE_STATE")
                doctrineStateLabels.Add(t);
        }

        int labelCount = Mathf.Min(
            doctrineCardNames.Length,
            Mathf.Min(
                doctrineNameLabels.Count,
                doctrineStateLabels.Count
            )
        );

        for (int i = 0; i < labelCount; i++)
        {
            SetTextTransform(
                doctrineNameLabels[i],
                doctrineCardNames[i]
            );

            SetTextTransform(
                doctrineStateLabels[i],
                doctrineCardStates[i]
            );
        }

        Debug.Log(
            "[COMMANDER PROFILE] Doctrine card labels wired: " +
            labelCount
        );


        foreach (Transform t in children)
        {
            if (t.name != "DOCTRINE_CARD")
                continue;

            Image cardImage = t.GetComponent<Image>();

            if (cardImage == null)
            {
                Debug.LogWarning(
                    "[COMMANDER PROFILE] DOCTRINE_CARD has no Image: " +
                    t.name
                );

                continue;
            }

            Button button = t.GetComponent<Button>();

            if (button == null)
                button = t.gameObject.AddComponent<Button>();

            button.targetGraphic = cardImage;
            button.transition = Selectable.Transition.None;

            int selectedIndex = cardIndex;

            button.onClick.RemoveAllListeners();

            button.onClick.AddListener(delegate
            {
                Debug.Log(
                    "[COMMANDER PROFILE] DOCTRINE CARD CLICKED: " +
                    selectedIndex
                );

                SetDoctrineMode(selectedIndex);
                CloseDoctrine();
            });

            doctrineCards.Add(button);

            cardIndex++;
        }

        Debug.Log(
            "[COMMANDER PROFILE] Doctrine cards wired: " +
            doctrineCards.Count
        );
    }
    private void SetTextTransform(Transform target, string value)
    {
        if (target == null)
            return;

        TMPro.TMP_Text tmp = target.GetComponent<TMPro.TMP_Text>();

        if (tmp != null)
        {
            tmp.text = value;
            return;
        }

        UnityEngine.UI.Text legacy =
            target.GetComponent<UnityEngine.UI.Text>();

        if (legacy != null)
        {
            legacy.text = value;
            return;
        }

        Debug.LogWarning(
            "[COMMANDER PROFILE] No text component found on: " +
            target.name
        );
    }
    private void WireNavigation()
    {
        WireButton("BUTTON_NAV_PROFILE", delegate { ShowPage(0); });
        WireButton("BUTTON_NAV_RANK", delegate { ShowPage(1); });
        WireButton("BUTTON_NAV_PROGRESSION", delegate { ShowPage(2); });
        WireButton("BUTTON_NAV_DOCTRINE", delegate { ShowPage(3); });
        WireButton("BUTTON_NAV_STATISTICS", delegate { ShowPage(4); });
        WireButton("BUTTON_NAV_ACHIEVEMENTS", delegate { ShowPage(5); });
        WireButton("BUTTON_NAV_CAREER", delegate { ShowPage(6); });

        WireButton("BUTTON_DOCTRINE_LIBRARY", OpenDoctrine);
        WireButton("BUTTON_DOCTRINE_CLOSE", CloseDoctrine);

        WireButton("BUTTON_BACK", delegate
        {
            ShowPage(0);
            CloseDoctrine();
            CloseProtocolPopup();
        });
    }

    private void WireDoctrineModes()
    {
        for (int i = 0; i < doctrineModeButtons.Length; i++)
        {
            int modeIndex = i;

            WireButton(
                doctrineModeButtons[i],
                delegate
                {
                    SetDoctrineMode(modeIndex);
                });
        }
    }

    private void WireProtocolButton()
    {
        WireButton(
            "BUTTON_PROTOCOL_DROPDOWN",
            ToggleProtocolPopup);
    }

    private void WireButton(string objectName, UnityAction action)
    {
        GameObject go = GameObject.Find(objectName);

        if (go == null)
        {
            Debug.LogWarning("[COMMANDER PROFILE] Missing button: " + objectName);
            return;
        }

        Button button = go.GetComponent<Button>();

        if (button == null)
        {
            Debug.LogWarning(
                "[COMMANDER PROFILE] Object is missing Button component: " +
                objectName);

            return;
        }

        button.onClick.RemoveListener(action);
        button.onClick.AddListener(action);
    }

    public void ShowPage(int index)
    {
        if (index < 0 || index >= pages.Count)
            return;

        activePage = index;

        for (int i = 0; i < pages.Count; i++)
        {
            if (pages[i] != null)
                pages[i].SetActive(i == index);
        }

        UpdateNavigationState(index);

        CloseProtocolPopup();
    }

    private void UpdateNavigationState(int activeIndex)
    {
        for (int i = 0; i < navigationButtons.Length; i++)
        {
            GameObject go = GameObject.Find(navigationButtons[i]);

            if (go == null)
                continue;

            Image image = go.GetComponent<Image>();

            if (image == null)
                continue;

            image.color = i == activeIndex
                ? new Color(0.05f, 0.34f, 0.42f, 1f)
                : new Color(0.025f, 0.045f, 0.058f, 1f);
        }
    }

    private void OpenDoctrine()
    {
        if (doctrineWindow != null)
            doctrineWindow.SetActive(true);

        CloseProtocolPopup();
    }

    private void CloseDoctrine()
    {
        if (doctrineWindow != null)
            doctrineWindow.SetActive(false);
    }

    private void SetDoctrineMode(int index)
    {
        if (index < 0 || index >= doctrineNames.Length)
            return;

        string doctrineName = doctrineNames[index];
        string description = doctrineDescriptions[index];
        string status = doctrineStatuses[index];

        SetTextObject("DOCTRINE_NAME", doctrineName);
        SetTextObject("DOCTRINE_DESCRIPTION", description);
        SetTextObject("DOCTRINE_STATUS", status);

        if (index < doctrinePrinciples.Length)
        {
            for (int i = 0; i < 6 && i < doctrinePrinciples[index].Length; i++)
            {
                SetTextObject(
                    "PRINCIPLE_" + (i == 0 ? "215" :
                                    i == 1 ? "181" :
                                    i == 2 ? "147" :
                                    i == 3 ? "113" :
                                    i == 4 ? "79" :
                                    "45"),
                    doctrinePrinciples[index][i]
                );
            }
        }

        for (int i = 0; i < 8; i++)
        {
            string buttonName = doctrineModeButtonNames[i];
            GameObject buttonObject = GameObject.Find(buttonName);

            if (buttonObject == null)
                continue;

            Image image = buttonObject.GetComponent<Image>();

            if (image == null)
                continue;

            image.color =
                i == index
                    ? new Color(0.08f, 0.35f, 0.45f, 1f)
                    : new Color(1f, 1f, 1f, 1f);
        }

        Debug.Log(
            "[COMMANDER PROFILE] ACTIVE DOCTRINE: " +
            doctrineName
        );
    }
    private void SetTextObject(string objectName, string value)
    {
        GameObject go = GameObject.Find(objectName);

        if (go == null)
            return;

        TMPro.TMP_Text tmp = go.GetComponent<TMPro.TMP_Text>();

        if (tmp != null)
        {
            tmp.text = value;
            return;
        }

        Text legacy = go.GetComponent<Text>();

        if (legacy != null)
            legacy.text = value;
    }

    private void ToggleProtocolPopup()
    {
        if (protocolPopup == null)
            BuildProtocolPopup();

        if (protocolPopup == null)
            return;

        protocolPopup.SetActive(!protocolPopup.activeSelf);
    }

    private void CloseProtocolPopup()
    {
        if (protocolPopup != null)
            protocolPopup.SetActive(false);
    }

    private void BuildProtocolPopup()
    {
        GameObject canvasObject = GameObject.Find("Canvas_CommanderProfileHUD");

        if (canvasObject == null)
        {
            Debug.LogWarning(
                "[COMMANDER PROFILE] Cannot create protocol popup. " +
                "Canvas_CommanderProfileHUD not found.");

            return;
        }

        protocolPopup = new GameObject(
            "RUNTIME_PROTOCOL_DROPDOWN",
            typeof(RectTransform),
            typeof(Image));

        protocolPopup.transform.SetParent(
            canvasObject.transform,
            false);

        RectTransform rect =
            protocolPopup.GetComponent<RectTransform>();

        rect.anchorMin = new Vector2(1f, 1f);
        rect.anchorMax = new Vector2(1f, 1f);
        rect.pivot = new Vector2(1f, 1f);
        rect.anchoredPosition = new Vector2(-20f, -125f);
        rect.sizeDelta = new Vector2(330f, 180f);

        Image background =
            protocolPopup.GetComponent<Image>();

        background.color =
            new Color(0.018f, 0.035f, 0.045f, 0.98f);

        CreateProtocolText(
            protocolPopup.transform,
            "PROTOCOL",
            new Vector2(18f, -18f),
            20,
            true);

        CreateProtocolText(
            protocolPopup.transform,
            "COMMAND PROFILE // ACTIVE",
            new Vector2(18f, -55f),
            15,
            false);

        CreateProtocolText(
            protocolPopup.transform,
            "PROFILE ACCESS: AUTHORIZED",
            new Vector2(18f, -85f),
            13,
            false);

        CreateProtocolText(
            protocolPopup.transform,
            "AUTONOMY SYSTEM: ONLINE",
            new Vector2(18f, -112f),
            13,
            false);

        CreateProtocolText(
            protocolPopup.transform,
            "NETWORK LINK: SECURE",
            new Vector2(18f, -139f),
            13,
            false);

        protocolPopup.SetActive(false);
    }

    private void CreateProtocolText(
        Transform parent,
        string value,
        Vector2 position,
        int fontSize,
        bool bold)
    {
        GameObject textObject =
            new GameObject(
                "TEXT",
                typeof(RectTransform));

        textObject.transform.SetParent(parent, false);

        RectTransform rect =
            textObject.GetComponent<RectTransform>();

        rect.anchorMin = new Vector2(0f, 1f);
        rect.anchorMax = new Vector2(0f, 1f);
        rect.pivot = new Vector2(0f, 1f);
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(290f, 24f);

        Component tmp =
            textObject.AddComponent(
                Type.GetType("TMPro.TextMeshProUGUI, Unity.TextMeshPro"));

        if (tmp != null)
        {
            Type type = tmp.GetType();

            var textProperty = type.GetProperty("text");
            if (textProperty != null)
                textProperty.SetValue(tmp, value, null);

            var fontSizeProperty = type.GetProperty("fontSize");
            if (fontSizeProperty != null)
                fontSizeProperty.SetValue(tmp, (float)fontSize, null);
        }
    }
}















