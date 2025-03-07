using System.Collections.Generic;
using UnityEngine;

// This won scene thing is just duct taped on for the presentation.

public class PlayerCameraMovement : MonoBehaviour
{
    private Vector3 start;
    private Vector3 target;
    public float weight;
    public float speed;
    private GameObject playerThatWon;

    public bool winScene = false;

    private void Start()
    {
        start = transform.position;
    }

    private void Update()
    {
        if (winScene)
        {
            if (playerThatWon == null) playerThatWon = GameManager.players[0];
            target = playerThatWon.transform.position;
            transform.position = Vector3.Lerp(transform.position, new Vector3(target.x, target.y, target.z - 10), speed * Time.deltaTime);

            return;
        }

        List<GameObject> players = GameManager.players;

        if (players.Count == 0) return;

        Vector3 playerAverage = Vector3.zero;
        foreach (GameObject player in players)
        {
            if (player.GetComponent<Damageable>().dying) continue;
            playerAverage += player.transform.position;
        }
        playerAverage /= players.Count;

        target = start * weight + playerAverage * (1 - weight);
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.x, target.y, transform.position.z), speed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    public void WinScene(GameObject player)
    {
        winScene = true;
        playerThatWon = player;
    }
}
