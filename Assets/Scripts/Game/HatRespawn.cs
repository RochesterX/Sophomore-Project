using UnityEngine;

public class HatRespawn : MonoBehaviour
{
    private float lastInteractionTime;
    private const float respawnTime = 10f;
    private bool isDropped;

    void Start()
    {
        lastInteractionTime = Time.time;
        isDropped = false;
    }

    void Update() // Checks if the hat has been inactive for too long
    {
        if (isDropped && Time.time - lastInteractionTime > respawnTime)
        {
            Debug.Log("Hat has been inactive for too long. Respawning...");
            RespawnHat();
        }
    }

    void OnTriggerEnter2D(Collider2D collision) // Respawns the hat if it falls out of bounds
    {
        if (collision.gameObject.CompareTag("Platformer Hazard"))
        {
            Debug.Log("Hat collided with Platformer Hazard. Respawning...");
            RespawnHat();
        }
    }

    public void Interact() // Updates the player interaction time
    {
        lastInteractionTime = Time.time;
        isDropped = false;
        Debug.Log("Hat interacted with. Resetting timer.");
    }

    public void OnHatDropped() // Resets the timer when the hat is dropped
    {
        lastInteractionTime = Time.time;
        isDropped = true;
        Debug.Log("Hat dropped. Starting respawn timer.");
    }

    private void RespawnHat() // Respawns the hat at the designated spawn position
    {
        transform.position = GameManager.Instance.hatSpawnPosition;
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        transform.rotation = Quaternion.identity;
        lastInteractionTime = Time.time; // Reset the timer after respawning
        isDropped = false;
        Debug.Log("Hat respawned at designated position.");
    }
}
