using TMPro;
using UnityEngine; using Game; using Music; using Player;
using UnityEngine.UI;
namespace Game
{

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

    private void Update() // Updates the timer to show the time remaining
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

    public void StartTimer() // Starts the timer
    {
        if (!timerRunning)
        {
            timeRemaining = startTime;
            timerRunning = true;
        }
    }

    private void UpdateTimerDisplay() // Formats and sets the time remaining
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timer.text = string.Format("{0}:{1:D2}", minutes, seconds);
    }

    private void OnTimerEnd() // Ends the game when the time runs out
    {
        GameManager.Instance.GameOver();
    }
}
}