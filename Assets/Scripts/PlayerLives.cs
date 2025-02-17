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
        else //add more gamemodes and their lives here
        {
            currentLives = 0; 
        }
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
