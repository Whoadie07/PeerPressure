using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenBackpack : MonoBehaviour
{
    [SerializeField]
    private GameObject PlayerHand;
    [SerializeField]
    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        PlayerHand.GetComponent<Inventory>().OpenMainInventory = false;
        button.onClick.AddListener(ToggleBackpack);
    }

    // Update is called once per frame
    private void ToggleBackpack()
    {
        if (!PlayerHand.GetComponent<Inventory>().OpenMainInventory)
        {
            PlayerHand.GetComponent<Inventory>().OpenMainInventory = true;
        }
        else
        {
            PlayerHand.GetComponent<Inventory>().OpenMainInventory = false;
        }
    }
}
