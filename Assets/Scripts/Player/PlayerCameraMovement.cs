using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraMovement : MonoBehaviour
{
    private Vector3 start;
    private Vector3 target;
    public float weight;
    public float speed;
    private GameObject playerThatWon;
    public float lowerBound;
    public bool winScene = false;

    public bool staticCamera = false;

    private void Start()
    {
        start = transform.position;
    }

    private void Update()
    {
        if (winScene) // If the game is over, the camera will follow the player that won
        {
            if (playerThatWon == null || !playerThatWon.activeInHierarchy)
            {
                playerThatWon = FindWinner();
            }

            if (playerThatWon != null)
            {
                target = playerThatWon.transform.position;
                transform.position = Vector3.Lerp(transform.position, new Vector3(target.x, target.y, target.z - 10), speed * 12 * Time.deltaTime);
                if (transform.position.y < lowerBound)
                {
                    transform.position = new Vector3(transform.position.x, lowerBound, transform.position.z);
                }
            }

            return;
        }
        // Moves the camera to follow the players
        List<GameObject> players = GameManager.players;
        if (players.Count == 0) return;
        Vector3 playerAverage = Vector3.zero;
        int activePlayers = 0;
        foreach (GameObject player in players)
        {
            if (player == null || !player.activeInHierarchy) continue;
            Damageable damageable = player.GetComponent<Damageable>();
            if (damageable != null && damageable.dying) continue;
            playerAverage += player.transform.position;
            activePlayers++;
        }

        if (activePlayers == 0) return;
        if (staticCamera) return;

        playerAverage /= activePlayers;

        target = start * weight + playerAverage * (1 - weight);
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.x, target.y, transform.position.z), speed * Time.deltaTime);
        if (transform.position.y < lowerBound)
        {
            transform.position = new Vector3(transform.position.x, lowerBound, transform.position.z);
        }
    }

    public void WinScene(GameObject player)
    {
        winScene = true;
        playerThatWon = player;
    }

    private GameObject FindWinner() // Finds the player that won
    {
        foreach (GameObject player in GameManager.players)
        {
            if (player != null && player.activeInHierarchy)
            {
                return player;
            }
        }
        return null;
    }
}
