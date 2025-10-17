using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTest : MonoBehaviour
{
    void Start()
    {
        // To access the singleton, you must use the CLASS NAME, which is "ItemManager".
        Item testItem = ItemManager.Instance.GetItemById(0);

        // It's good practice to check if the item was found before trying to use it.
        if (testItem != null)
        {
            Debug.Log($"ItemTest successfully found item: {testItem.itemName}");
        }
        ItemManager.Instance.ChangeItemState(testItem.itemId, ItemState.InInventory);
        Debug.Log(testItem.itemState);
    }
    

}
