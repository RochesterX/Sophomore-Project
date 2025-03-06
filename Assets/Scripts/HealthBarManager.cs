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

    void Update()
    {
        foreach (var kvp in playerHealthBars)
        {
            GameObject player = kvp.Key;
            GameObject healthBar = kvp.Value;
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(player.transform.position);
            screenPosition.y += 15;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            healthBar.transform.position = worldPosition;
            healthBar.transform.rotation = Quaternion.identity;
        }
    }

    private void OnGameStart()
    {
        foreach (GameObject player in GameManager.players)
        {
            if (!playerHealthBars.ContainsKey(player))
            {
                GameObject healthBar = Instantiate(healthBarPrefab);
                healthBar.GetComponent<TerribleHealthBarScript>().SetPlayer(player);
                playerHealthBars[player] = healthBar;
            }
        }
    }

    private void OnGameEnd()
    {
        foreach (var kvp in playerHealthBars)
        {
            Destroy(kvp.Value);
        }
        playerHealthBars.Clear();
    }
}
