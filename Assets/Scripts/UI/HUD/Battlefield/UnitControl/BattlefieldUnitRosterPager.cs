using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace ObsidianProtocol.UI
{
    public sealed class BattlefieldUnitRosterPager : MonoBehaviour
    {
        [SerializeField] private RectTransform[] cardRoots = new RectTransform[5];
        [SerializeField] private Button previousButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private Text pageLabel;
        [SerializeField] private Text[] cardLabels = new Text[5];
        [SerializeField] private Image[] cardImages = new Image[5];

        private readonly List<Component> units = new List<Component>();
        private Component selectionManager;
        private int page;
        private const int PageSize = 5;

        private void Awake()
        {
            if (previousButton != null) previousButton.onClick.AddListener(PreviousPage);
            if (nextButton != null) nextButton.onClick.AddListener(NextPage);
            RefreshRoster();
            RenderPage();
        }

        private void OnEnable()
        {
            RefreshRoster();
            RenderPage();
        }

        public void RefreshRoster()
        {
            units.Clear();
            foreach (MonoBehaviour item in FindObjectsOfType<MonoBehaviour>())
            {
                if (item != null && item.gameObject.activeInHierarchy &&
                    item.gameObject.scene.IsValid() && item.GetType().Name == "SelectableUnit")
                    units.Add(item);
            }

            units.Sort((a, b) => string.Compare(a.gameObject.name, b.gameObject.name,
                StringComparison.OrdinalIgnoreCase));
            page = Mathf.Clamp(page, 0, Mathf.Max(0, (units.Count - 1) / PageSize));
            if (selectionManager == null)
                selectionManager = FindComponent("UnitSelectionManager");
        }

        public void NextPage()
        {
            RefreshRoster();
            int lastPage = Mathf.Max(0, (units.Count - 1) / PageSize);
            if (page < lastPage) page++;
            RenderPage();
        }

        public void PreviousPage()
        {
            if (page > 0) page--;
            RenderPage();
        }

        private void RenderPage()
        {
            int start = page * PageSize;
            for (int i = 0; i < PageSize; i++)
            {
                int index = start + i;
                bool valid = index < units.Count && units[index] != null;

                if (cardRoots != null && i < cardRoots.Length && cardRoots[i] != null)
                    cardRoots[i].gameObject.SetActive(true);

                if (cardLabels != null && i < cardLabels.Length && cardLabels[i] != null)
                    cardLabels[i].text = valid ? units[index].gameObject.name : "EMPTY";

                if (cardImages != null && i < cardImages.Length && cardImages[i] != null)
                    cardImages[i].color = valid
                        ? (IsSelected(units[index])
                            ? new Color(0.0f, 0.72f, 0.82f, 0.95f)
                            : new Color(0.035f, 0.105f, 0.16f, 0.92f))
                        : new Color(0.035f, 0.055f, 0.075f, 0.45f);

                if (cardRoots != null && i < cardRoots.Length && cardRoots[i] != null)
                {
                    Button button = cardRoots[i].GetComponent<Button>();
                    if (button != null)
                    {
                        button.interactable = valid;
                        button.onClick.RemoveAllListeners();
                        if (valid)
                        {
                            Component selectedUnit = units[index];
                            button.onClick.AddListener(() => SelectUnit(selectedUnit));
                        }
                    }
                }
            }

            int pageCount = Mathf.Max(1, (units.Count + PageSize - 1) / PageSize);
            if (pageLabel != null)
                pageLabel.text = (page + 1) + " / " + pageCount + "     " + units.Count + " UNITS";
            if (previousButton != null) previousButton.interactable = page > 0;
            if (nextButton != null) nextButton.interactable = (page + 1) * PageSize < units.Count;
        }

        private bool IsSelected(Component unit)
        {
            if (selectionManager == null) return false;
            PropertyInfo property = selectionManager.GetType().GetProperty(
                "SelectedUnits", BindingFlags.Instance | BindingFlags.Public);
            System.Collections.IEnumerable selected = property == null
                ? null : property.GetValue(selectionManager, null) as System.Collections.IEnumerable;
            if (selected == null) return false;

            foreach (object entry in selected)
            {
                if (ReferenceEquals(entry, unit)) return true;
                Component component = entry as Component;
                if (component != null && component.gameObject == unit.gameObject) return true;
                GameObject go = entry as GameObject;
                if (go != null && go == unit.gameObject) return true;
            }
            return false;
        }

        private void SelectUnit(Component unit)
        {
            if (unit == null) return;
            if (selectionManager == null) selectionManager = FindComponent("UnitSelectionManager");
            if (selectionManager == null)
            {
                Debug.LogWarning("[UnitRosterPager] UnitSelectionManager not found.");
                return;
            }

            string[] methodNames = { "TrySingleSelect", "SelectUnit", "Select" };
            foreach (string methodName in methodNames)
            {
                MethodInfo[] methods = selectionManager.GetType().GetMethods(
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (MethodInfo method in methods)
                {
                    if (method.Name != methodName) continue;
                    ParameterInfo[] parameters = method.GetParameters();
                    if (parameters.Length != 1) continue;
                    object argument;
                    if (parameters[0].ParameterType.IsInstanceOfType(unit)) argument = unit;
                    else if (parameters[0].ParameterType == typeof(GameObject)) argument = unit.gameObject;
                    else continue;

                    try
                    {
                        method.Invoke(selectionManager, new object[] { argument });
                        RenderPage();
                        return;
                    }
                    catch (Exception exception)
                    {
                        Debug.LogException(exception);
                    }
                }
            }

            Debug.LogWarning("[UnitRosterPager] No compatible single-unit selection method found.");
        }

        private static Component FindComponent(string typeName)
        {
            foreach (MonoBehaviour item in FindObjectsOfType<MonoBehaviour>())
                if (item != null && item.GetType().Name == typeName) return item;
            return null;
        }
    }
}