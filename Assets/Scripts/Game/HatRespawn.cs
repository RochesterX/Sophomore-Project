using UnityEngine;
using Game;
using Music;
using Player;
using System.Collections;
namespace Game
{

    public class HatRespawn : MonoBehaviour
    {
        private float lastInteractionTime;
        public const float respawnTime = 10f;
        private bool isDropped;

        public Vector2 initialSubhatPosition;


        public static bool canBePickedUp = true; // Flag to check if the hat can be picked up

        public Vector2 initialScale;

        private void Awake()
        {
            initialScale = transform.lossyScale;
        }

        void Start()
        {
            //initialSubhatPosition = transform.GetChild(0).transform.localPosition;
            lastInteractionTime = Time.time;
            isDropped = false;
            transform.position = GameManager.Instance.hatSpawnPositions[Random.Range(0, GameManager.Instance.hatSpawnPositions.Count - 1)];
        }

        void Update() // Checks if the hat has been inactive for too long
        {
            if (GameManager.Instance.gameOver) GetComponent<BoxCollider2D>().enabled = false;
            if (isDropped && Time.time - lastInteractionTime > respawnTime)
            {
                StartCoroutine(RespawnHat());
            }
        }

        void OnTriggerEnter2D(Collider2D collision) // Respawns the hat if it falls out of bounds
        {
            if (collision.gameObject.CompareTag("Platformer Hazard"))
            {
                isDropped = true;
                StartCoroutine(RespawnHat());
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

        private IEnumerator RespawnHat() // Respawns the hat at the designated spawn position
        {
            GetComponentInChildren<Animator>().SetTrigger("respawn");

            yield return new WaitForSeconds(1f / 3f / 2f);

            transform.position = GameManager.Instance.hatSpawnPositions[Random.Range(0, GameManager.Instance.hatSpawnPositions.Count - 1)];
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            GetComponent<Rigidbody2D>().angularVelocity = 0f;
            transform.rotation = Quaternion.identity;
            lastInteractionTime = Time.time; // Reset the timer after respawning
            isDropped = false;
        }
    }
}