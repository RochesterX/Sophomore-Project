using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance { get; private set; }

    [SerializeField] private GameObject playersParent;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject leaderboardIconPrefab;

    private Dictionary<GameObject, GameObject> playerIcons = new Dictionary<GameObject, GameObject>();

    private void Awake() // Ensures only one instance of LeaderboardManager exists
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

    private void InitializeLeaderboard() // Creates the leaderboard icons for each player
    {
        RectTransform parentRectTransform = playersParent.GetComponent<RectTransform>();
        parentRectTransform.anchoredPosition = new Vector2(-20f, 10f);

        foreach (GameObject player in GameManager.players)
        {
            Transform parent = Instantiate(playerPrefab, playersParent.transform).transform;
            GameObject leaderboardIcon = Instantiate(leaderboardIconPrefab, parent);
            leaderboardIcon.GetComponentInChildren<Image>().color = GameManager.playerColors[GameManager.players.IndexOf(player)];
            playerIcons[player] = parent.gameObject;
        }
    }

    public void UpdateLeaderboard() // Sorts the leaderboard based on player hold times
    {
        List<KeyValuePair<GameObject, float>> sortedList = new List<KeyValuePair<GameObject, float>>(GameManager.playerHoldTimes);
        sortedList.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));
        foreach (var player in sortedList)
        {
            playerIcons[player.Key].transform.SetSiblingIndex(sortedList.IndexOf(player));
            UpdatePlayerHoldTimeText(player.Key, player.Value);
        }
    }

    private void UpdatePlayerHoldTimeText(GameObject player, float holdTime) // Updates the hold times of each player shown on the leaderboard
    {
        if (playerIcons.ContainsKey(player))
        {
            TextMeshProUGUI holdTimeText = playerIcons[player].GetComponentInChildren<TextMeshProUGUI>();
            if (holdTimeText != null)
            {
                int minutes = Mathf.FloorToInt(holdTime / 60F);
                int seconds = Mathf.FloorToInt(holdTime % 60F);
                holdTimeText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
            }
        }
    }
}

