using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance { get; private set; }

    [SerializeField] private GameObject playersParent;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject lifePrefab;

    private Dictionary<GameObject, GameObject> playerIcons = new Dictionary<GameObject, GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeLeaderboard();
    }

    private void InitializeLeaderboard()
    {
        foreach (GameObject player in GameManager.players)
        {
            Transform parent = Instantiate(playerPrefab, playersParent.transform).transform;
            GameObject life = Instantiate(lifePrefab, parent);
            life.GetComponentInChildren<Image>().color = GameManager.playerColors[GameManager.players.IndexOf(player)];
            playerIcons[player] = parent.gameObject;
        }
    }

    public void UpdateLeaderboard()
    {
        List<KeyValuePair<GameObject, float>> sortedList = new List<KeyValuePair<GameObject, float>>(GameManager.playerHoldTimes);
        sortedList.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));

        foreach (var player in sortedList)
        {
            Debug.Log(player.Key.name + " : " + player.Value);
        }
        // Less fancy sorting system

        foreach (var player in sortedList)
        {
            playerIcons[player.Key].transform.SetSiblingIndex(sortedList.IndexOf(player));
        }

        //foreach (var key in GameManager.playerHoldTimes)
        //{
         //   print(key.Key.name + " : " + key.Value);
       // }
    }
}
