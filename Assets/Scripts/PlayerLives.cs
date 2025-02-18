using Unity.VisualScripting;
using UnityEngine;

public class PlayerLives : MonoBehaviour
{
    public int maxLives = 3;
    public int currentLives; 

    private void Start()
    {
        if (GameManager.gameMode == GameManager.GameMode.freeForAll)
        {
            currentLives = maxLives;
        }
        if (GameManager.gameMode == GameManager.GameMode.keepAway)
        {
            currentLives = 1;
        }
        if (GameManager.gameMode == GameManager.GameMode.obstacleCourse)
        {
            currentLives = 1;
        }
        //add more gamemodes and their lives here
    }
    public void PlayerDied()
    {
        if (GameManager.gameMode == GameManager.GameMode.freeForAll)
        {
            currentLives--;
            if (currentLives <= 0)
            {
                //add Game over sequence;
            }
            else
            {
                RespawnPlayer();
            }
        }
        if (GameManager.gameMode == GameManager.GameMode.keepAway)
        {

        }
        if (GameManager.gameMode == GameManager.GameMode.obstacleCourse)
        {

        }
    }
    private void RespawnPlayer()
    {
        RespawnOnTriggerEnter respawnScript = GetComponent<RespawnOnTriggerEnter>();
        if (respawnScript != null)
        {
            transform.position = respawnScript.spawnPoint;
        }
    }
}
