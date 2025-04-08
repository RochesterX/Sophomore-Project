using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
    public static WinScreen Instance;
    public List<TextMeshProUGUI> playerTexts;

    private void Awake() // Ensures only one instance of WinScreen exists
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    
    public void ShowWinScreen(int player) // Triggers the win screen to appear
    {
        foreach (TextMeshProUGUI playerText in playerTexts)
        {
            playerText.text = "Player " + player;
            if (playerText.color != Color.black)
            {
                playerText.color = GameManager.playerColors[player - 1];
            }
        }

        GetComponent<Animator>().SetTrigger("win");
    }
}
