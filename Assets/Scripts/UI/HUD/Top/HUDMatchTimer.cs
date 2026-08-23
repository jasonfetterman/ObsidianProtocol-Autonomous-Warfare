using UnityEngine;
using UnityEngine.UI;

public class HUDMatchTimer : MonoBehaviour
{
    [SerializeField] private Text timerText;
    [SerializeField] private bool countUp = true;

    public float ElapsedTime { get; private set; }
    public bool IsRunning { get; private set; }

    private void Update()
    {
        if (!IsRunning)
            return;

        if (countUp)
            ElapsedTime += Time.deltaTime;

        Refresh();
    }

    public void StartTimer()
    {
        IsRunning = true;
    }

    public void PauseTimer()
    {
        IsRunning = false;
    }

    public void ResetTimer()
    {
        ElapsedTime = 0f;
        Refresh();
    }

    public void SetTime(float seconds)
    {
        ElapsedTime = Mathf.Max(0f, seconds);
        Refresh();
    }

    private void Refresh()
    {
        int hours = Mathf.FloorToInt(ElapsedTime / 3600f);
        int minutes = Mathf.FloorToInt((ElapsedTime % 3600f) / 60f);
        int seconds = Mathf.FloorToInt(ElapsedTime % 60f);

        if (timerText != null)
        {
            timerText.text = hours > 0
                ? $"{hours:00}:{minutes:00}:{seconds:00}"
                : $"{minutes:00}:{seconds:00}";
        }
    }
}