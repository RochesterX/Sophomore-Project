using System.Collections;
using UnityEngine; using Game; using Music; using Player;
namespace Player
{

public class UseItem : MonoBehaviour
{
    [SerializeField] private string itemTag;
    private GameObject heldItem;
    private bool isHoldingItem = false;
    private float holdStartTime;
    public float holdTime;
    private Damageable damageable;

    [SerializeField] public Transform head;

    private void Start()
    {
        damageable = GetComponent<Damageable>();
    }

    void Update()
    {
        if (isHoldingItem)
        {
            // Keeps hat on the player's head
            heldItem.transform.localPosition = Vector3.zero;
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

    public void PickUpItem(GameObject item) // Player picks up hat and starts hold counter 
    {
        if (damageable.dying) return; // Prevent picking up items if the player is dying
        if (HatRespawn.canBePickedUp == false) return; // Prevent picking up items if they are not interactable

        heldItem = item;
        isHoldingItem = true;
        holdStartTime = Time.time;
        item.GetComponent<Collider2D>().enabled = false;
        item.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        item.GetComponent<HatRespawn>().Interact();
        item.transform.parent = head;
        item.transform.localRotation = Quaternion.identity;
        item.transform.localPosition = Vector3.zero;
        if (!GameManager.playerHoldTimes.ContainsKey(gameObject))
        {
            GameManager.playerHoldTimes[gameObject] = 0f;
        }
        AudioManager.Instance.PlaySound("Pickup Hat");
        GameManager.Instance.StopCoroutine("MoveHatToWinner");
    }

    public void DropItem() // Player drops hat when hit
    {
        if (GameManager.Instance.gameOver) return; // Prevent dropping items if the game is over

        if (isHoldingItem)
        {
            heldItem.GetComponent<Collider2D>().enabled = true;
            HatRespawn.canBePickedUp = false;
            StartCoroutine(WaitForInteractability());
            heldItem.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            heldItem.GetComponent<Rigidbody2D>().AddForce(Vector2.up * Random.Range(10f, 30f) + Vector2.right * Random.Range(-10, 10), ForceMode2D.Impulse);
            heldItem.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-5, 5), ForceMode2D.Impulse);
            heldItem.GetComponent<HatRespawn>().OnHatDropped();
            heldItem.transform.parent = GameManager.Instance.transform;
            heldItem = null;
            isHoldingItem = false;
            if (GameManager.playerHoldTimes.ContainsKey(gameObject))
            {
                GameManager.playerHoldTimes.Remove(gameObject);
            }
        }
    }

    private IEnumerator WaitForInteractability()
    {
        yield return new WaitForSeconds(0.1f);
        HatRespawn.canBePickedUp = true;
    }

    public bool IsHoldingItem()
    {
        return isHoldingItem;
    }
}
}