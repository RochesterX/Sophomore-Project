using UnityEngine;

public class PlayerLives : MonoBehaviour
{
    public int maxLives = 3;
    public int currentLives; 
    public string gameMode = "free-for-all";

    private void Start()
    {
        if (gameMode == "free-for-all")
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
        if (gameMode == "free-for-all")
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
