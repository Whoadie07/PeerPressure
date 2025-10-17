using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    // Public static property 
    public static ItemManager Instance { get; private set; }

    [Tooltip("The list of all item names to be loaded into the game. The index in this array will become the item's ID.")]
    [SerializeField] private string[] itemNames;

    private readonly Dictionary<int, Item> _itemDatabase = new Dictionary<int, Item>();

    private void Awake()
    {
        //Making Obj singleton
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Found more than one instance of ItemManager. Destroying duplicate.");
            Destroy(this.gameObject); // Destroy duplicate
            return;
        }
        Instance = this;
        InitializeItemDatabase();
    }


    /// Creates item database 
    private void InitializeItemDatabase()
    {
        for (int i = 0; i < itemNames.Length; i++)
        {
            // Create a new item instance for the database
            Item newItem = new Item
            {
                itemName = itemNames[i],
                itemId = i
                // Default state and quantity are set in the Item class definition
            };
            _itemDatabase.Add(i, newItem);
        }
        Debug.Log($"Item Database initialized with {_itemDatabase.Count} items.");
    }

    // Retrieves an item by its ID from the database
    public Item GetItemById(int id)
    {
        if (_itemDatabase.TryGetValue(id, out Item item))
        {
            return item;
        }

        Debug.LogWarning($"Item with ID {id} not found in the database.");
        return null;
    }

    public ItemState getItemState(int id)
    {
        Item item = GetItemById(id);
        return item.itemState;
    }

    // Changes item state
    public void ChangeItemState(int id, ItemState newState)
    {
        Item item = GetItemById(id);
        if (item != null)
        {
            item.itemState = newState;
        }
    }
}
