using UnityEngine;
using UnityEngine.UI;

public class UIMainMenuVersionDisplay : MonoBehaviour
{
    public static UIMainMenuVersionDisplay Instance { get; private set; }

    [SerializeField] private Text versionText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        Refresh();
    }

    public void Refresh()
    {
        if (versionText != null)
            versionText.text = $"VERSION {Application.version}";
    }
}
