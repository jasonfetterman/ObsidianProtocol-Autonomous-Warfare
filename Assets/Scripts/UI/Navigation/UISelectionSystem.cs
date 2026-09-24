using UnityEngine;
using UnityEngine.EventSystems;

public class UISelectionSystem : MonoBehaviour
{
    public static UISelectionSystem Instance { get; private set; }

    public GameObject CurrentSelection { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Select(GameObject target)
    {
        if (target == null)
            return;

        CurrentSelection = target;

        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(target);
    }

    public void ClearSelection()
    {
        CurrentSelection = null;

        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);
    }

    public bool IsSelected(GameObject target)
    {
        return target != null && CurrentSelection == target;
    }
}
