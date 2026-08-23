using UnityEngine;
using UnityEngine.UI;

public class UIMainMenuVROperator : MonoBehaviour
{
    public static UIMainMenuVROperator Instance { get; private set; }

    [SerializeField] private Button vrOperatorButton;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (vrOperatorButton == null)
            vrOperatorButton = GetComponent<Button>();
    }

    public void VROperator()
    {
        Debug.Log("VR Operator selected.");
    }

    public void SetInteractable(bool interactable)
    {
        if (vrOperatorButton != null)
            vrOperatorButton.interactable = interactable;
    }
}
