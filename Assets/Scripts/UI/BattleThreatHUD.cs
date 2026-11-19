using TMPro;
using UnityEngine;

public class BattleThreatHUD : MonoBehaviour
{
    private TMP_Text threatText;

    private void Awake()
    {
        threatText = GetComponent<TMP_Text>();
        SetThreatLevel("NOMINAL");
    }

    public void SetThreatLevel(string level)
    {
        if (threatText == null)
            threatText = GetComponent<TMP_Text>();

        if (threatText != null)
            threatText.text = $"THREAT LEVEL\n{level}";
    }
}