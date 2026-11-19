using TMPro;
using UnityEngine;

public class BattleTimerHUD : MonoBehaviour
{
    private TMP_Text timerText;
    private float elapsedTime;

    private void Awake()
    {
        timerText = GetComponent<TMP_Text>();
        elapsedTime = 0f;
    }

    private void Update()
    {
        if (timerText == null)
            return;

        elapsedTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        timerText.text = $"BATTLE TIME\n{minutes:00}:{seconds:00}";
    }
}