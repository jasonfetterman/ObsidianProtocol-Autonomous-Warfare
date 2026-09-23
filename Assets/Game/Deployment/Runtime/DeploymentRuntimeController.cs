using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ObsidianProtocol.Game.Deployment
{
    public class DeploymentRuntimeController : MonoBehaviour
    {
        private const int MAX_DEPLOYMENT_POINTS = 10000;

        private readonly Dictionary<string, int> unitCosts =
            new Dictionary<string, int>
            {
                { "WARDEN", 1200 },
                { "BEACON", 850 },
                { "BULLDOG", 1400 },
                { "FORGE", 1700 },
                { "SENTINEL", 1100 }
            };

        private readonly List<string> selectedUnits = new List<string>();
        private readonly List<int> selectedCosts = new List<int>();

        private readonly string[] windowNames =
        {
            "[WINDOW] SELECT OPERATION",
            "[WINDOW] SELECT FORCE",
            "[WINDOW] FLEET SELECTION",
            "[WINDOW] FORMATION",
            "[WINDOW] COMMAND STRUCTURE",
            "[WINDOW] COMMAND INTENT",
            "[WINDOW] RULES OF ENGAGEMENT",
            "[WINDOW] DEPLOYMENT BUDGET",
            "[WINDOW] FINAL REVIEW"
        };

        private int remainingBudget = MAX_DEPLOYMENT_POINTS;
        private GameObject currentWindow;

        private void Awake()
        {
            HideAllWindows();

            BindButton("[BUTTON] SELECT OPERATION",
                () => OpenWindow("[WINDOW] SELECT OPERATION"));

            BindButton("[BUTTON] CONFIGURE FORMATION",
                () => OpenWindow("[WINDOW] FORMATION"));

            BindButton("[BUTTON] CONFIGURE COMMAND",
                () => OpenWindow("[WINDOW] COMMAND STRUCTURE"));

            BindButton("[BUTTON] CONFIGURE INTENT",
                () => OpenWindow("[WINDOW] COMMAND INTENT"));

            BindButton("[BUTTON] CONFIGURE ROE",
                () => OpenWindow("[WINDOW] RULES OF ENGAGEMENT"));

            BindButton("[BUTTON] FINAL REVIEW",
                () => OpenWindow("[WINDOW] FINAL REVIEW"));

            BindButton("[BUTTON] OPEN FLEET",
                () => OpenWindow("[WINDOW] FLEET SELECTION"));

            BindButton("[BUTTON] WARDEN",
                () => AddUnit("WARDEN"));

            BindButton("[BUTTON] BEACON",
                () => AddUnit("BEACON"));

            BindButton("[BUTTON] BULLDOG",
                () => AddUnit("BULLDOG"));

            BindButton("[BUTTON] FORGE",
                () => AddUnit("FORGE"));

            BindButton("[BUTTON] SENTINEL",
                () => AddUnit("SENTINEL"));

            BindButton("[BUTTON] ADD UNIT",
                AddLastAvailableUnit);

            BindButton("[BUTTON] REMOVE UNIT",
                RemoveLastUnit);

            BindButton("[BUTTON] CLEAR FORCE",
                ClearForce);

            UpdateDisplays();

            Debug.Log("[DEPLOYMENT] Window system initialized.");
        }

        private void BindButton(string objectName,
            UnityEngine.Events.UnityAction action)
        {
            GameObject buttonObject = FindSceneObject(objectName);

            if (buttonObject == null)
            {
                Debug.LogWarning(
                    "[DEPLOYMENT] Missing button: " + objectName);
                return;
            }

            Button button = buttonObject.GetComponent<Button>();

            if (button == null)
            {
                Debug.LogWarning(
                    "[DEPLOYMENT] No Button component: " + objectName);
                return;
            }

            button.onClick.AddListener(action);
        }

        private void HideAllWindows()
        {
            foreach (string windowName in windowNames)
            {
                GameObject window = FindSceneObject(windowName);

                if (window != null)
                    window.SetActive(false);
            }

            currentWindow = null;
        }

        private void OpenWindow(string windowName)
        {
            HideAllWindows();

            GameObject window = FindSceneObject(windowName);

            if (window == null)
            {
                Debug.LogWarning(
                    "[DEPLOYMENT] Window not found: " + windowName);
                return;
            }

            window.SetActive(true);
            currentWindow = window;

            Debug.Log(
                "[DEPLOYMENT] OPENED " + windowName);
        }

        private GameObject FindSceneObject(string objectName)
        {
            GameObject[] objects =
                UnityEngine.Resources.FindObjectsOfTypeAll<GameObject>();

            GameObject best = null;
            int bestScore = int.MinValue;

            foreach (GameObject obj in objects)
            {
                if (obj.name != objectName)
                    continue;

                if (obj.hideFlags != HideFlags.None)
                    continue;

                int score = 0;

                RectTransform rect =
                    obj.GetComponent<RectTransform>();

                if (rect != null)
                {
                    if (rect.parent != null)
                        score += 100;

                    if (rect.childCount > 0)
                        score += 100;

                    if (rect.anchorMin != new Vector2(0.5f, 0.5f))
                        score += 50;

                    if (rect.anchorMax != new Vector2(0.5f, 0.5f))
                        score += 50;

                    if (rect.sizeDelta.x != 100f ||
                        rect.sizeDelta.y != 100f)
                        score += 25;
                }

                if (score > bestScore)
                {
                    bestScore = score;
                    best = obj;
                }
            }

            return best;
        }

        private void AddUnit(string unit)
        {
            if (!unitCosts.ContainsKey(unit))
                return;

            int cost = unitCosts[unit];

            if (remainingBudget < cost)
            {
                Debug.LogWarning(
                    "[DEPLOYMENT] Insufficient deployment budget for " +
                    unit);
                return;
            }

            selectedUnits.Add(unit);
            selectedCosts.Add(cost);
            remainingBudget -= cost;

            UpdateDisplays();

            Debug.Log(
                "[DEPLOYMENT] " + unit +
                " ADDED // " + cost + " DP");
        }

        private void AddLastAvailableUnit()
        {
            foreach (KeyValuePair<string, int> pair in unitCosts)
            {
                if (remainingBudget >= pair.Value)
                {
                    AddUnit(pair.Key);
                    return;
                }
            }
        }

        private void RemoveLastUnit()
        {
            if (selectedUnits.Count == 0)
                return;

            int index = selectedUnits.Count - 1;

            remainingBudget += selectedCosts[index];

            selectedUnits.RemoveAt(index);
            selectedCosts.RemoveAt(index);

            UpdateDisplays();
        }

        private void ClearForce()
        {
            selectedUnits.Clear();
            selectedCosts.Clear();

            remainingBudget = MAX_DEPLOYMENT_POINTS;

            UpdateDisplays();

            Debug.Log(
                "[DEPLOYMENT] FORCE CLEARED // 10,000 DP");
        }

        private void UpdateDisplays()
        {
            SetText(
                "[STAT] 10,000 / 10,000",
                remainingBudget.ToString("N0") +
                " / " +
                MAX_DEPLOYMENT_POINTS.ToString("N0"));

            SetText(
                "REMAINING BUDGET",
                remainingBudget.ToString("N0") + " DP");

            SetText(
                "BUDGET SUMMARY",
                remainingBudget.ToString("N0") + " DP REMAINING");

            SetText(
                "UNIT COUNT // 0",
                "UNIT COUNT // " + selectedUnits.Count);

            SetText(
                "UNIT COST VALUE",
                selectedUnits.Count > 0
                    ? selectedCosts[selectedCosts.Count - 1]
                        .ToString("N0") + " DP"
                    : "0 DP");
        }

        private void SetText(string objectName, string value)
        {
            GameObject obj = FindSceneObject(objectName);

            if (obj == null)
                return;

            TMPro.TMP_Text tmp =
                obj.GetComponent<TMPro.TMP_Text>();

            if (tmp != null)
            {
                tmp.text = value;
                return;
            }

            Text legacy =
                obj.GetComponent<Text>();

            if (legacy != null)
                legacy.text = value;
        }
    }
}
