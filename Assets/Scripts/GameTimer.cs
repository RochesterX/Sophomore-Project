using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public float startTime = 180f;
    private float timeRemaining;
    private bool timerRunning = false;

    public Text timerText;
    [SerializeField] private TextMeshProUGUI timer;

    private void Start()
    {
        timeRemaining = startTime;
        timer.text = "3:00.00";
        UpdateTimerDisplay();
    }

    private void Update()
    {
        if (timerRunning)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                timerRunning = false;
                OnTimerEnd();
            }

            UpdateTimerDisplay();
        }
    }

    public void StartTimer()
    {
        if (!timerRunning)
        {
            timeRemaining = startTime;
            timerRunning = true;
        }
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timer.text = string.Format("{0}:{1:D2}", minutes, seconds);
    }

    private void OnTimerEnd()
    {
        GameManager.Instance.GameOver();
    }
}
