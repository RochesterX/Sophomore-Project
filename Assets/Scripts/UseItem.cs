using UnityEngine;

public class UseItem : MonoBehaviour
{
    [SerializeField] private string itemTag;
    private GameObject heldItem;
    private bool isHoldingItem = false;

    void Update()
    {
        if (isHoldingItem)
        {
            heldItem.transform.position = transform.position + Vector3.up;
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
        item.GetComponent<Collider2D>().enabled = false;
        item.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        item.transform.rotation = Quaternion.identity;
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
