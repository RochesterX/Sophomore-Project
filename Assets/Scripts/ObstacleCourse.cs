using UnityEngine; using Game; using Music; using Player;

namespace Game
{

public class ObstacleCourse : MonoBehaviour
{
    public static GameObject playerWon;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerWon = collision.gameObject;
            GameManager.Instance.GameOver();
        }
    }
}
}