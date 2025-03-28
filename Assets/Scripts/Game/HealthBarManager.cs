using System.Collections.Generic;
using UnityEngine;

public class HealthBarManager : MonoBehaviour
{
    public GameObject healthBarPrefab;
    private Dictionary<GameObject, GameObject> playerHealthBars = new Dictionary<GameObject, GameObject>();

    void Start()
    {
        GameManager.Instance.StartGameEvent += OnGameStart;
        GameManager.Instance.EndGameEvent += OnGameEnd;
    }

    void OnDestroy()
    {
        GameManager.Instance.StartGameEvent -= OnGameStart;
        GameManager.Instance.EndGameEvent -= OnGameEnd;
    }

    void Update() // Updates position of health bars to follow each player
    {
        foreach (var kvp in playerHealthBars)
        {
            GameObject player = kvp.Key;
            if (player == null) continue;

            GameObject healthBar = kvp.Value;
            healthBar.transform.SetPositionAndRotation(new Vector3(player.transform.position.x, player.transform.position.y + 1.5f, player.transform.position.z), Quaternion.identity);
        }
    }

    private void OnGameStart() // Creates health bars for each player
    {
        foreach (GameObject player in GameManager.players)
        {
            if (!playerHealthBars.ContainsKey(player))
            {
                GameObject healthBar = Instantiate(healthBarPrefab);
                healthBar.transform.localScale *= 1.5f;
                healthBar.GetComponent<TerribleHealthBarScript>().SetPlayer(player);
                playerHealthBars[player] = healthBar;
            }
        }
    }

    private void OnGameEnd() // Destroys the health bars when the game ends
    {
        foreach (var kvp in playerHealthBars)
        {
            Destroy(kvp.Value);
        }
        playerHealthBars.Clear();
    }
}
