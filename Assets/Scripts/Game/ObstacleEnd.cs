using Game;
using UnityEngine;

public class ObstacleEnd : MonoBehaviour
{
    void Start()
    {
        UpdateVisibility();
    }

    void Update()
    {
        UpdateVisibility();
    }

    private void UpdateVisibility() // Sets object active if playing obstacle course
    {
        if (GameManager.gameMode == GameManager.GameMode.obstacleCourse)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}