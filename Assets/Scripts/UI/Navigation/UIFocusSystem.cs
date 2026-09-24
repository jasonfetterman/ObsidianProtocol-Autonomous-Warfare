using UnityEngine;
using UnityEngine.EventSystems;

public class UIFocusSystem : MonoBehaviour
{
    public static UIFocusSystem Instance { get; private set; }

    private GameObject currentFocusedObject;

    public GameObject CurrentFocusedObject => currentFocusedObject;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetFocus(GameObject target)
    {
        if (target == null)
            return;

        currentFocusedObject = target;

        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(target);
    }

    public void ClearFocus()
    {
        currentFocusedObject = null;

        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);
    }

    public bool HasFocus()
    {
        return currentFocusedObject != null;
    }
}
