using System.Collections.Generic;
using UnityEngine; using Game; using Music; using Player;
using TMPro;
using UnityEngine.UI;
namespace Game
{

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
        parentRectTransform.anchoredPosition = new Vector2(-10f, 10f);

        foreach (GameObject player in GameManager.players)
        {
            Transform parent = Instantiate(playerPrefab, playersParent.transform).transform;
            GameObject leaderboardIcon = Instantiate(leaderboardIconPrefab, parent);
            leaderboardIcon.GetComponentInChildren<Image>().color = GameManager.playerColors[GameManager.players.IndexOf(player)];
            playerIcons[player] = parent.gameObject;
        }
    }

    public void UpdateLeaderboard()
    {
        List<KeyValuePair<GameObject, float>> sortedList = new List<KeyValuePair<GameObject, float>>(GameManager.playerHoldTimes);
        sortedList.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));

        for (int i = 0; i < sortedList.Count; i++)
        {
            var player = sortedList[i];
            playerIcons[player.Key].transform.SetSiblingIndex(i);

            // Update the number text
            TextMeshProUGUI[] textComponents = playerIcons[player.Key].GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var textComponent in textComponents)
            {
                if (textComponent.name == "Position Text")
                {
                    textComponent.text = "#" + (i + 1).ToString();
                    break;
                }
            }
        }
    }

    public void UpdatePlayerHoldTimeText(GameObject player, float holdTime)
    {
        if (playerIcons.ContainsKey(player))
        {
            TextMeshProUGUI[] textComponents = playerIcons[player].GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var textComponent in textComponents)
            {
                if (textComponent.name == "Text (TMP)")
                {
                    int minutes = Mathf.FloorToInt(holdTime / 60F);
                    int seconds = Mathf.FloorToInt(holdTime % 60F);
                    textComponent.text = string.Format("{0:0}:{1:00}", minutes, seconds);
                    break;
                }
            }
        }
    }

}}
