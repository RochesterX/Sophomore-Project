using System.Collections.Generic;
using TMPro;
using UnityEngine; using Game; using Music; using Player;
namespace Game
{

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
    
    public void ShowWinScreen(int player)
    {
        if (player - 1 < 0 || player - 1 >= GameManager.playerColors.Count)
        {
            return;
        }

        foreach (TextMeshProUGUI playerText in playerTexts)
        {
            playerText.text = "Player " + player;
            if (playerText.color != Color.black)
            {
                playerText.color = GameManager.playerColors[player - 1];
            }
        }

        Animator animator = GetComponent<Animator>();
        if (animator == null)
        {
            return;
        }

        animator.SetTrigger("win");
    }

}
}