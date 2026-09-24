using UnityEngine;
using UnityEngine.EventSystems;

public class UINavigationSystem : MonoBehaviour
{
    public static UINavigationSystem Instance { get; private set; }

    [SerializeField] private GameObject defaultSelection;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetDefaultSelection(GameObject target)
    {
        defaultSelection = target;
    }

    public void SelectDefault()
    {
        if (defaultSelection == null)
            return;

        EventSystem eventSystem = EventSystem.current;

        if (eventSystem == null)
            return;

        eventSystem.SetSelectedGameObject(defaultSelection);
    }

    public void ClearSelection()
    {
        EventSystem eventSystem = EventSystem.current;

        if (eventSystem == null)
            return;

        eventSystem.SetSelectedGameObject(null);
    }
}
