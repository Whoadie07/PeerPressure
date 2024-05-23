using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropItems : MonoBehaviour
{
    [SerializeField]
    private GameObject PlayerHand;
    [SerializeField]
    private Button click;

    // Start is called before the first frame update
    void Start()
    {
        click.onClick.AddListener(DropItem);
    }

    private void DropItem()
    {
        int hand = PlayerHand.GetComponent<Inventory>().NumberItemCurrentlyHolding;
        if (PlayerHand.GetComponent<Inventory>() != null)
        {
            if (PlayerHand.GetComponent<Inventory>().HotbarInventory != null)
            {
                if (PlayerHand.GetComponent<Inventory>().HotbarInventory[hand] != null)
                {
                    PlayerHand.GetComponent<Inventory>().HotbarInventory[hand].GetComponent<Object_Data>().isContain = false;
                    PlayerHand.GetComponent<Inventory>().HotbarInventory[hand].GetComponent<Object_Data>().isHold = false;
                    PlayerHand.GetComponent<Inventory>().HotbarInventory[hand] = null;
                    PlayerHand.GetComponent<Inventory>().HotbarInventory_UI[hand].GetComponent<RawImage>().texture = null;
                    PlayerHand.GetComponent<Inventory>().CurrentlyHolding = null;
                }
            }
        }
    }
}
