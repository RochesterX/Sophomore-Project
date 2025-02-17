using TMPro;
using UnityEngine;

public class PlayerJoinCard : MonoBehaviour
{
    public GameObject playerPreview;
    public int playerNumber;
    public TextMeshProUGUI playerNumberText;

    void Start()
    {
        playerNumberText.text = playerNumber.ToString();
    }
}
