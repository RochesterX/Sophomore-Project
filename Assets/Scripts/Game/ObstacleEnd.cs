using Game;
using UnityEngine;

/// <summary>
/// This class controls the visibility of the obstacle end object based on the current game mode.
/// </summary>
public class ObstacleEnd : MonoBehaviour
{
    /// <summary>
    /// Initializes the visibility of the object when the game starts.
    /// </summary>
    private void Start()
    {
        UpdateVisibility();
    }

    /// <summary>
    /// Updates the visibility of the object every frame.
    /// </summary>
    private void Update()
    {
        UpdateVisibility();
    }

    /// <summary>
    /// Sets the object to be active only if the game mode is "Obstacle Course".
    /// </summary>
    private void UpdateVisibility()
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