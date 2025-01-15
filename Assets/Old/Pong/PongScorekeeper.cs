using TMPro;
using UnityEngine;

public class PongScorekeeper : MonoBehaviour
{
    public TextMeshProUGUI player1Score;
    public TextMeshProUGUI player2Score;

    private void Update()
    {
        player1Score.text = PongBallBehavior.score.x.ToString();
        player2Score.text = PongBallBehavior.score.y.ToString();
    }
}
