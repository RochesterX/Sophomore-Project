using System.Collections.Generic;
using UnityEngine;

public class PlatformerCameraMovement : MonoBehaviour
{
    public List<GameObject> players;

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
        if (players.Count == 0) return;

        Vector3 playerAverage = Vector3.zero;
        foreach (GameObject player in players)
        {
            playerAverage += player.transform.position;
        }
        playerAverage /= players.Count;

        target = start * weight + playerAverage * (1 - weight);
        transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }
}
