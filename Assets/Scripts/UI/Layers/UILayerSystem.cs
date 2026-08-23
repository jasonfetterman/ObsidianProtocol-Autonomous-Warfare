using UnityEngine;

public class UILayerSystem : MonoBehaviour
{
    public static UILayerSystem Instance { get; private set; }

    public enum UILayer
    {
        Background,
        Main,
        Window,
        Popup,
        Overlay,
        Notification,
        System
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public int GetLayerOrder(UILayer layer)
    {
        return (int)layer;
    }
}
