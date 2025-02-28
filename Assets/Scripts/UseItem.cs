using UnityEngine;

public class UseItem : MonoBehaviour
{
    private GameObject heldItem;
    private bool isHoldingItem = false;

    void Update()
    {
        if (isHoldingItem)
        {
            heldItem.transform.position = transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item") && !isHoldingItem)
        {
            PickUpItem(collision.gameObject);
        }
    }

    private void PickUpItem(GameObject item)
    {
        heldItem = item;
        isHoldingItem = true;
        item.GetComponent<Collider2D>().enabled = false;
    }

    public void DropItem()
    {
        if (isHoldingItem)
        {
            heldItem.GetComponent<Collider2D>().enabled = true;
            heldItem = null;
            isHoldingItem = false;
        }
    }

    private void OnEnable()
    {
        Punch.OnPlayerPunched += HandlePlayerPunched;
    }

    private void OnDisable()
    {
        Punch.OnPlayerPunched -= HandlePlayerPunched;
    }

    private void HandlePlayerPunched(GameObject punchedPlayer)
    {
        if (punchedPlayer == gameObject)
        {
            DropItem();
        }
    }
}
