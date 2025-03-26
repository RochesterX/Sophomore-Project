using UnityEngine;

public class UseItem : MonoBehaviour
{
    [SerializeField] private string itemTag;
    private GameObject heldItem;
    private bool isHoldingItem = false;
    private float holdStartTime;

    void Update()
    {
        if (isHoldingItem)
        {
            heldItem.transform.position = transform.position + Vector3.up;
            if (GameManager.gameMode == GameManager.GameMode.keepAway)
            {
                float holdTime = Time.time - holdStartTime;
                GameManager.Instance.UpdatePlayerHoldTime(gameObject, holdTime);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hat") && !isHoldingItem)
        {
            PickUpItem(collision.gameObject);
        }
    }

    private void PickUpItem(GameObject item)
    {
        heldItem = item;
        isHoldingItem = true;
        holdStartTime = Time.time;
        item.GetComponent<Collider2D>().enabled = false;
        item.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        item.transform.rotation = Quaternion.identity;
        if (!GameManager.playerHoldTimes.ContainsKey(gameObject))
        {
            GameManager.playerHoldTimes[gameObject] = 0f;
        }
    }

    public void DropItem()
    {
        if (isHoldingItem)
        {
            heldItem.GetComponent<Collider2D>().enabled = true;
            heldItem.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            heldItem.transform.position += Vector3.up * 3f;
            heldItem = null;
            isHoldingItem = false;
            if (GameManager.playerHoldTimes.ContainsKey(gameObject))
            {
                GameManager.playerHoldTimes.Remove(gameObject);
            }
        }
    }

    private void OnEnable()
    {
        //Punch.OnPlayerPunched += HandlePlayerPunched;
    }

    private void OnDisable()
    {
        //Punch.OnPlayerPunched -= HandlePlayerPunched;
    }
    /*
        private void HandlePlayerPunched(GameObject punchedPlayer)
        {
            if (punchedPlayer == gameObject)
            {
                DropItem();
            }
        }*/
}
