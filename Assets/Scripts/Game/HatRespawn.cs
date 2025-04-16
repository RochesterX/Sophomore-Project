using UnityEngine; using Game; using Music; using Player;
namespace Game
{

public class HatRespawn : MonoBehaviour
{
    private float lastInteractionTime;
    public const float respawnTime = 10f;
    private bool isDropped;

    public static bool canBePickedUp = true; // Flag to check if the hat can be picked up

    void Start()
    {
        lastInteractionTime = Time.time;
        isDropped = false;
        transform.position = GameManager.Instance.hatSpawnPositions[Random.Range(0, GameManager.Instance.hatSpawnPositions.Count - 1)];
    }

    void Update() // Checks if the hat has been inactive for too long
    {
        if (GameManager.Instance.gameOver) GetComponent<BoxCollider2D>().enabled = false;
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
        transform.position = GameManager.Instance.hatSpawnPositions[Random.Range(0, GameManager.Instance.hatSpawnPositions.Count - 1)];
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        GetComponent<Rigidbody2D>().angularVelocity = 0f;
        transform.rotation = Quaternion.identity;
        lastInteractionTime = Time.time; // Reset the timer after respawning
        isDropped = false;
    }
}
}