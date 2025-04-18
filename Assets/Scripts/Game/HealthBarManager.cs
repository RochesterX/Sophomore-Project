using System.Collections.Generic;
using UnityEngine; using Game; using Music; using Player;
namespace Game
{

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
                CreateHealthBar(player);

                // Subscribe to the player's death and respawn events
                var damageable = player.GetComponent<Damageable>();
                damageable.OnPlayerDeath += HandlePlayerDeath;
                damageable.OnPlayerRespawn += HandlePlayerRespawn;
            }
        }
    }

    private void HandlePlayerRespawn(GameObject player)
    {
        if (!playerHealthBars.ContainsKey(player))
        {
            CreateHealthBar(player);
        }
    }

    private void CreateHealthBar(GameObject player)
    {
        GameObject healthBar = Instantiate(healthBarPrefab);
        healthBar.transform.localScale *= 1.5f;
        healthBar.GetComponent<TerribleHealthBarScript>().SetPlayer(player);
        playerHealthBars[player] = healthBar;
    }

    private void HandlePlayerDeath(GameObject player)
    {
        if (playerHealthBars.TryGetValue(player, out GameObject healthBar))
        {
            Destroy(healthBar);
            playerHealthBars.Remove(player);
        }
    }

    private void OnGameEnd()
    {
        foreach (var kvp in playerHealthBars)
        {
            Destroy(kvp.Value);
        }
        playerHealthBars.Clear();

        // Unsubscribe from all player events
        foreach (GameObject player in GameManager.players)
        {
            if (player != null && player.TryGetComponent<Damageable>(out var damageable))
            {
                damageable.OnPlayerDeath -= HandlePlayerDeath;
                damageable.OnPlayerRespawn -= HandlePlayerRespawn;
            }
        }
    }
}
}