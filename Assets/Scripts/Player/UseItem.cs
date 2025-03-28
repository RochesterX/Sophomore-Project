using UnityEngine;

public class UseItem : MonoBehaviour
{
    [SerializeField] private string itemTag;
    private GameObject heldItem;
    private bool isHoldingItem = false;
    private float holdStartTime;
    public float holdTime;
    private Damageable damageable;

    private void Start()
    {
        damageable = GetComponent<Damageable>();
    }

    void Update()
    {
        if (isHoldingItem)
        {
            // Keeps hat on the player's head
            heldItem.transform.position = transform.position + Vector3.up;
            if (GameManager.gameMode == GameManager.GameMode.keepAway)
            {
                // Adds time to the player's leaderboard standing
                holdTime += Time.deltaTime;
                GameManager.Instance.UpdatePlayerHoldTime(gameObject, holdTime);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) // Player automatically picks up hat when touching it
    {
        if (collision.gameObject.CompareTag("Hat") && !isHoldingItem && !damageable.dying)
        {
            PickUpItem(collision.gameObject);
        }
    }

    private void PickUpItem(GameObject item) // Player picks up hat and starts hold counter 
    {
        if (damageable.dying) return; // Prevent picking up items if the player is dying

        heldItem = item;
        isHoldingItem = true;
        holdStartTime = Time.time;
        item.GetComponent<Collider2D>().enabled = false;
        item.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        item.transform.rotation = Quaternion.identity;
        item.GetComponent<HatRespawn>().Interact();
        if (!GameManager.playerHoldTimes.ContainsKey(gameObject))
        {
            GameManager.playerHoldTimes[gameObject] = 0f;
        }
    }

    public void DropItem() // Player drops hat when hit
    {
        if (isHoldingItem)
        {
            heldItem.GetComponent<Collider2D>().enabled = true;
            heldItem.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            heldItem.transform.position += Vector3.up * 3f;
            heldItem.GetComponent<HatRespawn>().OnHatDropped();
            heldItem = null;
            isHoldingItem = false;
            if (GameManager.playerHoldTimes.ContainsKey(gameObject))
            {
                GameManager.playerHoldTimes.Remove(gameObject);
            }
        }
    }

}
