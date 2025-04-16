using TMPro;
using UnityEngine; using Game; using Music; using Player;
namespace Game
{

public class PlayerJoinCard : MonoBehaviour
{
    public GameObject playerPreview;
    public int playerNumber;
    public TextMeshProUGUI playerNumberText;

    void Start() // Sets player number
    {
        playerNumberText.text = playerNumber.ToString();
    }
}
}