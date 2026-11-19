using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public static class DepartmentPanelPopulationBuilder
{
    private const string ScenePath = "Assets/Scenes/SCN02_COMMAND_CENTER/COMMAND CENTER HUD/Command_Center.unity";

    private class PanelSpec
    {
        public string ObjectName;
        public string Title;
        public string Subtitle;
        public string[][] Sections;

        public PanelSpec(string objectName, string title, string subtitle, string[][] sections)
        {
            ObjectName = objectName;
            Title = title;
            Subtitle = subtitle;
            Sections = sections;
        }
    }

    [MenuItem("Obsidian Protocol/Command Center/POPULATE ALL DEPARTMENT PANELS")]
    public static void Build()
    {
        var scene = SceneManager.GetActiveScene();
        if (scene.path != ScenePath)
            scene = EditorSceneManager.OpenScene(ScenePath);

        string backupPath = ScenePath + ".backup_before_department_population";
        if (!File.Exists(backupPath))
            File.Copy(ScenePath, backupPath);

        PanelSpec[] panels =
        {
            new PanelSpec("OPERATIONS_REAL_PANEL", "OPERATIONS", "MISSION PLANNING // OPERATIONAL OVERSIGHT", new string[][]
            {
                new[] { "OPERATION STATUS", "Active operation: Awaiting operation data", "Operation identifier: --", "Command authority: --", "Operational theater: --" },
                new[] { "AVAILABLE OPERATIONS", "Available operation records: Awaiting database connection", "Operation readiness: --", "Mission classification: --" },
                new[] { "MISSION BRIEFING", "Mission overview: No briefing loaded", "Strategic situation: --", "Rules of engagement: --", "Operational constraints: --" },
                new[] { "MISSION OBJECTIVES", "Primary objectives: Not assigned", "Secondary objectives: Not assigned", "Objective completion: --", "Mission success conditions: --" },
                new[] { "FORCE ASSIGNMENT", "Assigned units: None", "Force composition: --", "Deployment requirements: --", "Force readiness: --" },
                new[] { "OPERATIONAL PROGRESS", "Current phase: --", "Completed objectives: --", "Outstanding objectives: --", "Mission outcome: Pending" },
                new[] { "OPERATION HISTORY", "Completed operations: Awaiting records", "Mission reports: Awaiting records", "Historical performance: --" }
            }),
            new PanelSpec("FLEET_REAL_PANEL", "FLEET", "MILITARY ASSET REGISTER // FORCE READINESS", new string[][]
            {
                new[] { "FLEET OVERVIEW", "Total registered units: Awaiting unit database", "Operational units: --", "Unavailable units: --", "Deployed units: --" },
                new[] { "UNIT CLASSIFICATIONS", "Air units: Awaiting records", "Ground units: Awaiting records", "Sea units: Awaiting records", "Satellite units: Awaiting records", "Command units: Awaiting records", "Experimental units: Awaiting records" },
                new[] { "UNIT INVENTORY", "Unit designation: --", "Unit identifier: --", "Classification: --", "Service record: --", "Ownership: --" },
                new[] { "UNIT AVAILABILITY", "Readiness status: --", "Current assignment: --", "Deployment status: --", "Maintenance status: --" },
                new[] { "FLEET COMPOSITION", "Force distribution: Awaiting fleet data", "Class distribution: --", "Fleet readiness: --", "Assignment overview: --" },
                new[] { "UNIT RECORDS", "Individual service records: Awaiting database connection", "Unit history: --", "Assignment history: --" }
            }),
            new PanelSpec("DEPLOY_REAL_PANEL", "DEPLOYMENT", "FORCE PROJECTION // BATTLEFIELD INSERTION", new string[][]
            {
                new[] { "DEPLOYMENT OVERVIEW", "Deployment status: Standby", "Selected battlefield: None", "Deployment authorization: --", "Available deployment capacity: --" },
                new[] { "AVAILABLE FORCES", "Available units: Awaiting fleet connection", "Unit classification: --", "Unit readiness: --", "Deployment eligibility: --" },
                new[] { "DEPLOYMENT ROSTER", "Selected deployment units: None", "Assigned unit count: 0", "Force composition: --", "Roster readiness: --" },
                new[] { "BATTLEFIELD SELECTION", "Battlefield: None selected", "Environment: --", "Map dimensions: --", "Operational conditions: --" },
                new[] { "DEPLOYMENT ZONES", "Available insertion zones: Awaiting battlefield data", "Zone capacity: --", "Zone restrictions: --", "Deployment coordinates: --" },
                new[] { "REINFORCEMENTS & RETURNS", "Reinforcement availability: --", "Reinforcement capacity: --", "Return authorization: --", "Returning units: --" },
                new[] { "DEPLOYMENT RECORD", "Last deployment: --", "Deployment history: Awaiting records", "Current deployment status: --" }
            }),
            new PanelSpec("LOGISTICS_REAL_PANEL", "LOGISTICS", "SUPPLY NETWORK // RESOURCE DISTRIBUTION", new string[][]
            {
                new[] { "LOGISTICS OVERVIEW", "Supply network status: Awaiting logistics connection", "Active supply requests: --", "Units requiring support: --", "Distribution status: --" },
                new[] { "SUPPLY INVENTORY", "Fuel reserves: Awaiting resource data", "Energy reserves: Awaiting resource data", "Ammunition inventory: Awaiting resource data", "Spare parts: Awaiting resource data", "Repair materials: Awaiting resource data" },
                new[] { "UNIT SUPPLY STATUS", "Unit designation: --", "Fuel requirement: --", "Energy requirement: --", "Ammunition requirement: --", "Resupply status: --" },
                new[] { "ENERGY NETWORK", "Charging stations: Awaiting network data", "Stored energy: --", "Energy collection: --", "Active charging requests: --" },
                new[] { "TRANSPORT & DISTRIBUTION", "Active transport assignments: --", "Supply routes: --", "Distribution destinations: --", "Delivery status: --" },
                new[] { "LOGISTICS REQUESTS", "Pending requests: --", "Approved requests: --", "Fulfilled requests: --", "Outstanding requirements: --" },
                new[] { "LOGISTICS ACTIVITY", "Recent supply movements: Awaiting records", "Resource consumption: --", "Distribution history: --" }
            }),
            new PanelSpec("RESEARCH_REAL_PANEL", "RESEARCH", "TECHNOLOGY DEVELOPMENT // RESEARCH DIRECTORATE", new string[][]
            {
                new[] { "RESEARCH OVERVIEW", "Research network status: Awaiting technology database", "Active projects: --", "Available projects: --", "Completed technologies: --" },
                new[] { "RESEARCH CATEGORIES", "Weapons development: Awaiting records", "Armor development: Awaiting records", "Mobility systems: Awaiting records", "Energy systems: Awaiting records", "Sensors and intelligence: Awaiting records" },
                new[] { "AVAILABLE RESEARCH", "Research designation: --", "Technology classification: --", "Prerequisites: --", "Research requirements: --" },
                new[] { "ACTIVE PROJECTS", "Project designation: --", "Assigned research resources: --", "Current progress: --", "Estimated completion: --" },
                new[] { "TECHNOLOGY UNLOCKS", "Available unlocks: Awaiting technology data", "Unit systems unlocked: --", "Equipment unlocked: --", "Development dependencies: --" },
                new[] { "UNIT DEVELOPMENT", "Eligible unit upgrades: --", "Upgrade requirements: --", "Installation compatibility: --" },
                new[] { "RESEARCH HISTORY", "Completed projects: Awaiting records", "Research milestones: --", "Technology development history: --" }
            }),
            new PanelSpec("GARAGE_REAL_PANEL", "GARAGE", "UNIT SERVICE FACILITY // MAINTENANCE & CONFIGURATION", new string[][]
            {
                new[] { "GARAGE OVERVIEW", "Registered service units: Awaiting fleet data", "Units requiring repair: --", "Active maintenance jobs: --", "Service facility status: --" },
                new[] { "UNIT INSPECTION", "Selected unit: None", "Unit designation: --", "Unit classification: --", "Service record: --", "Overall condition: --" },
                new[] { "DAMAGE ASSESSMENT", "Hull condition: --", "Component condition: --", "Weapon system condition: --", "Mobility system condition: --", "Critical damage reports: --" },
                new[] { "REPAIR & MAINTENANCE", "Repair status: --", "Required repair materials: --", "Estimated service time: --", "Maintenance schedule: --", "Service cost: --" },
                new[] { "EQUIPMENT MANAGEMENT", "Installed equipment: Awaiting unit data", "Available equipment: --", "Weapon configuration: --", "Armor configuration: --", "Component compatibility: --" },
                new[] { "UPGRADES & CUSTOMIZATION", "Available upgrades: --", "Installed upgrades: --", "Configuration profile: --", "Upgrade requirements: --" },
                new[] { "SERVICE HISTORY", "Previous repairs: Awaiting records", "Maintenance history: --", "Component replacement history: --" }
            }),
            new PanelSpec("FINANCE_REAL_PANEL", "FINANCE", "MILITARY FINANCE // BUDGET & EXPENDITURE CONTROL", new string[][]
            {
                new[] { "FINANCIAL OVERVIEW", "Available funds: Awaiting finance system", "Allocated budget: --", "Committed expenditures: --", "Unallocated funds: --" },
                new[] { "INCOME & REVENUE", "Recorded income: Awaiting transaction data", "Revenue sources: --", "Pending receipts: --", "Income history: --" },
                new[] { "EXPENDITURE BREAKDOWN", "Unit acquisition: --", "Maintenance and repairs: --", "Research and development: --", "Logistics and supplies: --", "Operational expenses: --" },
                new[] { "PURCHASE APPROVALS", "Pending purchase requests: --", "Approval status: --", "Requesting department: --", "Requested amount: --" },
                new[] { "OPERATIONAL COSTS", "Active operation costs: --", "Deployment expenses: --", "Unit operating expenses: --", "Resource consumption costs: --" },
                new[] { "FINANCIAL TRANSACTIONS", "Recent transactions: Awaiting finance records", "Transaction references: --", "Transaction status: --" },
                new[] { "BUDGET REPORTS", "Budget utilization: --", "Department allocations: --", "Financial history: Awaiting records" }
            }),
            new PanelSpec("INTELLIGENCE_REAL_PANEL", "INTELLIGENCE", "BATTLEFIELD AWARENESS // RECONNAISSANCE & THREAT ANALYSIS", new string[][]
            {
                new[] { "INTELLIGENCE OVERVIEW", "Intelligence network: Awaiting sensor connection", "Active surveillance feeds: --", "Unresolved threats: --", "Latest intelligence update: --" },
                new[] { "BATTLEFIELD SURVEILLANCE", "Operational theater: --", "Surveillance coverage: --", "Known contacts: --", "Unobserved areas: --" },
                new[] { "ENEMY CONTACTS", "Contact designation: --", "Contact classification: --", "Last known position: --", "Movement status: --", "Identification confidence: --" },
                new[] { "THREAT ASSESSMENT", "Threat designation: --", "Threat classification: --", "Threat level: --", "Threat direction: --", "Recommended response: --" },
                new[] { "RECONNAISSANCE REPORTS", "Active reconnaissance tasks: --", "Completed reports: Awaiting records", "Reconnaissance coverage: --" },
                new[] { "SENSOR & SATELLITE NETWORK", "Active sensors: --", "Satellite feeds: --", "Detection range: --", "Communications status: --" },
                new[] { "INTELLIGENCE HISTORY", "Recent alerts: Awaiting records", "Threat activity history: --", "Analysis reports: --" }
            }),
            new PanelSpec("STORE_REAL_PANEL", "STORE", "PROCUREMENT CATALOGUE // EQUIPMENT & PHYSICAL SCALE MODELS", new string[][]
            {
                new[] { "STORE OVERVIEW", "Catalogue status: Awaiting product database", "Available listings: --", "Order status: --", "Procurement account: --" },
                new[] { "PHYSICAL UNIT SCALE MODELS", "Air unit scale models: Awaiting product listings", "Ground unit scale models: Awaiting product listings", "Sea unit scale models: Awaiting product listings", "Satellite scale models: Awaiting product listings", "Experimental unit scale models: Awaiting product listings", "All current unit models are physical scale models only. RC versions are not currently offered." },
                new[] { "EQUIPMENT & COMPONENTS", "Equipment listings: Awaiting product database", "Components: --", "Weapons and accessories: --", "Armor and upgrade components: --" },
                new[] { "PRODUCT DETAILS", "Product designation: --", "Product description: --", "Model scale: --", "Materials: --", "Availability: --" },
                new[] { "PRICING & PROCUREMENT", "Listed price: Awaiting product data", "Purchase eligibility: --", "Order quantity: --", "Procurement status: --" },
                new[] { "ORDER HISTORY", "Previous orders: Awaiting records", "Order references: --", "Fulfillment status: --", "Purchase history: --" }
            })
        };

        int built = 0;
        foreach (var spec in panels)
        {
            var panel = FindSceneObject(spec.ObjectName);
            if (panel == null)
            {
                Debug.LogWarning("[DEPARTMENT PANELS] Missing panel: " + spec.ObjectName);
                continue;
            }

            var content = panel.transform.Find("CONTENT");
            if (content == null)
            {
                Debug.LogWarning("[DEPARTMENT PANELS] Missing CONTENT: " + spec.ObjectName);
                continue;
            }

            BuildPanel(content, spec);
            built++;
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("[DEPARTMENT PANELS] Populated " + built + " of " + panels.Length + " department panels. No buttons were added or wired.");
    }

    private static void BuildPanel(Transform content, PanelSpec spec)
    {
        var oldRoot = content.Find("DEPARTMENT_PANEL_INTERFACE");
        if (oldRoot != null)
            Object.DestroyImmediate(oldRoot.gameObject);

        var oldText = content.GetComponent<Text>();
        if (oldText != null)
            Object.DestroyImmediate(oldText);

        var root = new GameObject("DEPARTMENT_PANEL_INTERFACE", typeof(RectTransform), typeof(ScrollRect));
        root.transform.SetParent(content, false);
        Stretch(root.GetComponent<RectTransform>());

        var viewport = new GameObject("DEPARTMENT_VIEWPORT", typeof(RectTransform), typeof(Image), typeof(Mask));
        viewport.transform.SetParent(root.transform, false);
        Stretch(viewport.GetComponent<RectTransform>());
        viewport.GetComponent<Image>().color = Color.white;
        viewport.GetComponent<Mask>().showMaskGraphic = false;

        var panelContent = new GameObject("DEPARTMENT_CONTENT", typeof(RectTransform), typeof(VerticalLayoutGroup), typeof(ContentSizeFitter));
        panelContent.transform.SetParent(viewport.transform, false);

        var contentRect = panelContent.GetComponent<RectTransform>();
        contentRect.anchorMin = new Vector2(0, 1);
        contentRect.anchorMax = new Vector2(1, 1);
        contentRect.pivot = new Vector2(0.5f, 1);
        contentRect.anchoredPosition = Vector2.zero;
        contentRect.sizeDelta = Vector2.zero;

        var layout = panelContent.GetComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(14, 14, 12, 16);
        layout.spacing = 12;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = false;

        panelContent.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        var scroll = root.GetComponent<ScrollRect>();
        scroll.viewport = viewport.GetComponent<RectTransform>();
        scroll.content = contentRect;
        scroll.horizontal = false;
        scroll.vertical = true;
        scroll.movementType = ScrollRect.MovementType.Clamped;

        var header = Section(panelContent.transform, spec.Title + "_HEADER");
        Label(header.transform, spec.Title, 23, true);
        Label(header.transform, spec.Subtitle, 11);

        foreach (var sectionData in spec.Sections)
        {
            var section = Section(panelContent.transform, sectionData[0]);
            for (int i = 1; i < sectionData.Length; i++)
                Label(section.transform, sectionData[i]);
        }
    }

    private static GameObject FindSceneObject(string objectName)
    {
        foreach (var candidate in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (candidate.name == objectName && candidate.scene.IsValid() && candidate.scene.isLoaded)
                return candidate;
        }
        return null;
    }

    private static void Stretch(RectTransform rect)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    private static GameObject Section(Transform parent, string title)
    {
        var section = new GameObject(title.Replace(" ", "_"), typeof(RectTransform), typeof(Image), typeof(VerticalLayoutGroup), typeof(ContentSizeFitter));
        section.transform.SetParent(parent, false);
        section.GetComponent<Image>().color = new Color(0.025f, 0.055f, 0.08f, 0.96f);

        var layout = section.GetComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(12, 12, 10, 10);
        layout.spacing = 7;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = false;

        section.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        Label(section.transform, title, 16, true);
        return section;
    }

    private static void Label(Transform parent, string value, int size = 12, bool bold = false)
    {
        var go = new GameObject("LABEL_" + parent.childCount, typeof(RectTransform), typeof(TextMeshProUGUI), typeof(LayoutElement));
        go.transform.SetParent(parent, false);

        var text = go.GetComponent<TextMeshProUGUI>();
        text.text = value;
        text.fontSize = size;
        text.fontStyle = bold ? FontStyles.Bold : FontStyles.Normal;
        text.color = new Color(0.55f, 0.88f, 0.96f);
        text.textWrappingMode = TextWrappingModes.Normal;

        var element = go.GetComponent<LayoutElement>();
        element.minHeight = size + 8;
        element.preferredHeight = size + 10;
    }
}
