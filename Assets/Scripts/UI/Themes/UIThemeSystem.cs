using UnityEngine;
using System.Collections.Generic;

namespace ObsidianProtocol.UI
{
    /// <summary>
    /// Centralized theme controller for colors, fonts, materials, and style tokens.
    /// This system allows global UI restyling without touching individual prefabs.
    /// </summary>
    public sealed class UIThemeSystem : MonoBehaviour
    {
        public static UIThemeSystem Instance { get; private set; }

        [Header("Color Palette")]
        public Color primaryColor = new Color(0.85f, 0.85f, 0.85f);
        public Color accentColor = new Color(0.25f, 0.65f, 1f);
        public Color dangerColor = new Color(1f, 0.25f, 0.25f);
        public Color successColor = new Color(0.25f, 1f, 0.45f);

        [Header("Typography")]
        public Font mainFont;
        public Font headerFont;

        [Header("Materials")]
        public Material panelMaterial;
        public Material overlayMaterial;

        private readonly List<IThemeReceiver> receivers = new List<IThemeReceiver>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        // ---------------------------------------------------------
        // REGISTRATION
        // ---------------------------------------------------------

        public void Register(IThemeReceiver receiver)
        {
            if (!receivers.Contains(receiver))
                receivers.Add(receiver);
        }

        public void Unregister(IThemeReceiver receiver)
        {
            if (receivers.Contains(receiver))
                receivers.Remove(receiver);
        }

        // ---------------------------------------------------------
        // APPLY THEME
        // ---------------------------------------------------------

        public void ApplyTheme()
        {
            foreach (var r in receivers)
                r.ApplyTheme(this);
        }
    }

    /// <summary>
    /// Interface for UI elements that respond to theme changes.
    /// </summary>
    public interface IThemeReceiver
    {
        void ApplyTheme(UIThemeSystem theme);
    }
}
