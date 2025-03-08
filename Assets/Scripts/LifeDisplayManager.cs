using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeDisplayManager : MonoBehaviour
{
    public GameObject players;
    public GameObject playerPrefab;
    public GameObject lifePrefab;

    public Dictionary<Damageable, List<GameObject>> lifeDisplays = new Dictionary<Damageable, List<GameObject>>();

    private void Start()
    {
        foreach (GameObject player in GameManager.players)
        {
            Transform parent = Instantiate(playerPrefab, players.transform).transform;

            List<GameObject> lives = new List<GameObject>();
            for (int i = 0; i < player.GetComponent<Damageable>().lives; i++)
            {
                GameObject life = Instantiate(lifePrefab, parent);
                life.GetComponentInChildren<Image>().color = GameManager.playerColors[GameManager.players.IndexOf(player)];
                lives.Add(life);
            }

            lifeDisplays.Add(player.GetComponent<Damageable>(), lives);
        }
    }

    private void Update()
    {
        foreach (Damageable damageable in lifeDisplays.Keys)
        {
            foreach (GameObject life in lifeDisplays[damageable])
            {
                life.SetActive(lifeDisplays[damageable].IndexOf(life) < damageable.lives);
            }
        }
    }
}
