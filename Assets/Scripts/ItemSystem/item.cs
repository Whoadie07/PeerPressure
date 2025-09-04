public enum ItemState
{
    /// The item is not in the player's possession.
    NotInInventory, // 0

    /// The item is in the player's inventory.
    InInventory,    // 1

    /// The item has been used or is no longer relevant for game progression.
    NoLongerNeeded  // 2
}

[System.Serializable]
public class Item
{
    public string itemName;
    public int itemId;
    public int quantity = 0;
    public ItemState itemState = ItemState.NotInInventory;

    /// Default constructor.
    public Item() { }

    /// Creates a copy of an existing item.
    public Item(Item itemToCopy) {
        this.itemName = itemToCopy.itemName;
        this.itemId = itemToCopy.itemId;
    }
}

