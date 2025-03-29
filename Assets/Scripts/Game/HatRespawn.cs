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
            RespawnHat();
        }
    }

    void OnTriggerEnter2D(Collider2D collision) // Respawns the hat if it falls out of bounds
    {
        if (collision.gameObject.CompareTag("Platformer Hazard"))
        {
            RespawnHat();
        }
    }

    public void Interact() // Updates the player interaction time
    {
        lastInteractionTime = Time.time;
        isDropped = false;
    }

    public void OnHatDropped() // Resets the timer when the hat is dropped
    {
        lastInteractionTime = Time.time;
        isDropped = true;
    }

    private void RespawnHat() // Respawns the hat at the designated spawn position
    {
        transform.position = GameManager.Instance.hatSpawnPosition;
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        transform.rotation = Quaternion.identity;
        lastInteractionTime = Time.time; // Reset the timer after respawning
        isDropped = false;
    }
}
