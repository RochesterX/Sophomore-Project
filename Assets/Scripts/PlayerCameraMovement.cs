using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraMovement : MonoBehaviour
{
    private Vector3 start;
    private Vector3 target;
    public float weight;
    public float speed;

    private void Start()
    {
        start = transform.position;
    }

    private void Update()
    {
        List<GameObject> players = GameManager.players;

        if (players.Count == 0) return;

        Vector3 playerAverage = Vector3.zero;
        foreach (GameObject player in players)
        {
            playerAverage += player.transform.position;
        }
        playerAverage /= players.Count;

        target = start * weight + playerAverage * (1 - weight);
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.x, target.y, transform.position.z), speed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }
}
