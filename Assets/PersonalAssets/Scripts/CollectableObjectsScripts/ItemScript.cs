using UnityEngine;
using System.Collections;

public class ItemScript : MonoBehaviour
{

    public GameObject item;

    public void Start()
    {
        InstantiateItem();
        RefreshItemSprite();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (item != null)
            {
                other.GetComponent<Inventory>().AddItem(item);
                item = null;
            }
            Destroy(gameObject);
        }
        else if (other.tag == "Ground")
        {
            Rigidbody2D rBody = GetComponent<Rigidbody2D>();
            rBody.isKinematic = true;
            rBody.velocity = Vector2.zero;
        }
    }

    public void RefreshItemSprite()
    {
        if (item != null)
        {
            if (item.tag == "Gem")
            {
                GetComponent<SpriteRenderer>().sprite = item.GetComponent<UpgradeScript>().upgradeSprite;
            } else
            {
                GetComponent<SpriteRenderer>().sprite = item.GetComponent<SpriteRenderer>().sprite;
            }
        }
    }

    private void InstantiateItem()
    {
        item = Instantiate(item);
        item.transform.SetParent(transform, false);
        item.SetActive(false);
    }
}
