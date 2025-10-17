using UnityEngine;

/// <summary>
/// Makes a GameObject clickable to be picked up as an item.
/// The GameObject must have a Collider component for mouse detection to work.
/// </summary>
[RequireComponent(typeof(Collider))]
public class ClickableObject : MonoBehaviour
{
    [Tooltip("The ID of the item this object represents. This ID must correspond to an item in the ItemManager.")]
    [SerializeField] private int itemId;

    /// <summary>
    /// OnMouseDown is a Unity message that is called when the user has pressed the mouse button
    /// over any Collider on this GameObject.
    /// </summary>
    private void OnMouseDown()
    {
        if (ItemManager.Instance == null)
        {
            Debug.LogError("ItemManager instance not found. Cannot pick up item.");
            return;
        }

        // Retrieve the item from the central database.
        Item itemData = ItemManager.Instance.GetItemById(itemId);

        if (itemData != null)
        {
            // Change the state of the item in the central manager to "InInventory".
            ItemManager.Instance.ChangeItemState(itemId, ItemState.InInventory);


            // Deactivate the GameObject in the scene to represent it being picked up.
            // Using SetActive(false) is often better than Destroy() as it's less
            // performance-intensive and allows the object to be easily re-enabled.
            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning($"ClickableObject on '{gameObject.name}' has an invalid itemId: {itemId}. Item not found in ItemManager.", this);
        }
    }
}
