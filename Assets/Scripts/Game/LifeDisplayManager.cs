using System.Collections.Generic;
using UnityEngine; using Game; using Music; using Player;
using UnityEngine.UI;
namespace Game
{

public class LifeDisplayManager : MonoBehaviour
{
    public GameObject players;
    public GameObject playerPrefab;
    public GameObject lifePrefab;
    public Dictionary<Damageable, List<GameObject>> lifeDisplays = new Dictionary<Damageable, List<GameObject>>();

    private void Start() // Creates life icons for each player
    {
        if (GameManager.gameMode == GameManager.GameMode.freeForAll)
        {
            foreach (GameObject player in GameManager.players)
            {
                Transform parent = Instantiate(playerPrefab, players.transform).transform;
                List<GameObject> lives = new List<GameObject>();
                for (int i = 0; i < player.GetComponent<Damageable>().lives; i++)
                {
                    GameObject life = Instantiate(lifePrefab, parent);
                    life.transform.Find("LIFE").GetComponent<Image>().color = GameManager.playerColors[GameManager.players.IndexOf(player)];
                    lives.Add(life);
                }
                lifeDisplays.Add(player.GetComponent<Damageable>(), lives);
            }
        }
    }

    private void Update() // Updates the lives displayed based on player lives
    {
        foreach (Damageable damageable in lifeDisplays.Keys)
        {
            foreach (GameObject life in lifeDisplays[damageable])
            {
                life.SetActive(lifeDisplays[damageable].IndexOf(life) < damageable.lives);
            }
        }
    }

    public void HideLifeDisplay() // Hides life display
    {
        players.SetActive(false);
    }
}
}