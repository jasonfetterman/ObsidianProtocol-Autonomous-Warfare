using UnityEngine;
using UnityEngine.EventSystems;

public class UIKeyboardMouseNavigation : MonoBehaviour
{
    public static UIKeyboardMouseNavigation Instance { get; private set; }

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

    private void Update()
    {
        if (EventSystem.current == null)
            return;

        if (EventSystem.current.currentSelectedGameObject == null &&
            defaultSelection != null)
        {
            EventSystem.current.SetSelectedGameObject(defaultSelection);
        }
    }

    public void SetDefaultSelection(GameObject target)
    {
        defaultSelection = target;
    }

    public void Select(GameObject target)
    {
        if (target == null || EventSystem.current == null)
            return;

        EventSystem.current.SetSelectedGameObject(target);
    }

    public void ClearSelection()
    {
        if (EventSystem.current == null)
            return;

        EventSystem.current.SetSelectedGameObject(null);
    }
}
