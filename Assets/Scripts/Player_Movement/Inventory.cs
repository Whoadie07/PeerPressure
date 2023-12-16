using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    //Main Inventory
    private static int InventorySize = 20;
    public bool OpenMainInventory = false;
    public GameObject[] m_Inventory = new GameObject[InventorySize];
    public GameObject[] m_Inventory_UI = new GameObject[InventorySize];
    public GameObject mainInventory = null;

    //HotBar  Inventory
    private static int HotBarSize = 4;
    public GameObject Hotbar = null;
    public GameObject[] HotbarInventory = new GameObject[HotBarSize];
    public GameObject[] HotbarInventory_UI = new GameObject[HotBarSize];

    //What the character inventory manager
    public GameObject CurrentlyHolding = null;
    public GameObject tmpHolder = null;
    public int NumberItemCurrentlyHolding = 0;
    public bool isSelect;
    public int currectSelect;

    //Start of the game.
    private void Start()
    {
        OpenMainInventory = false;
        mainInventory.SetActive(OpenMainInventory);
    }
    private void Update()
    {
        //Manage open of the main inventory.
        if (OpenMainInventory)
        {
            mainInventory.SetActive(true);
        }
        else
        {
            mainInventory.SetActive(false);
        }
        //If the characters have item that they are holding.
        CurrentlyHolding = HotbarInventory[NumberItemCurrentlyHolding];
    }
    //Set the item to character hotbar and then main inventory
    public void setItem(GameObject item)
    {
        int avaSlot = avaiableSlot();
        if (avaSlot != -1)
        {
            if (avaSlot < HotBarSize)
            {
                HotbarInventory[avaSlot] = item;
                HotbarInventory[avaSlot].GetComponent<Object_Data>().isContain = true;
                HotbarInventory_UI[avaSlot].GetComponent<Slot>().ObjectInSlot = item;
                HotbarInventory_UI[avaSlot].GetComponent<RawImage>().texture = item.GetComponent<Object_Data>().ObjectImage;

            }
            else if((avaSlot - HotBarSize) < InventorySize)
            {
                avaSlot -= HotBarSize;
                m_Inventory[avaSlot] = item;
                m_Inventory[avaSlot].GetComponent<Object_Data>().isContain = true;
                m_Inventory_UI[avaSlot].GetComponent<RawImage>().texture = item.GetComponent<Object_Data>().ObjectImage;
                m_Inventory_UI[avaSlot].GetComponent<Slot>().ObjectInSlot = item;

            }

        }
        else
        {
            Debug.Log("Not enough space");
            //Something.
        }
        if (CurrentlyHolding == null)
        {
            NumberItemCurrentlyHolding = avaSlot;
            HotbarInventory[avaSlot].GetComponent<Object_Data>().isHold = true;
            CurrentlyHolding = HotbarInventory[avaSlot];
            

        }
    }
    //Check if what slot in hot bar or main inventory is occupied.
    public int avaiableSlot()
    {
        for(int i = 0; i < HotBarSize; i++)
        {
            if (HotbarInventory[i] == null)
            {
                return i;
            }
        }
        for (int i = 0; i < InventorySize; i++)
        {
            if (m_Inventory[i] == null)
            {
                return HotBarSize+i;
            }
        }
        return -1;
    }
    //Return the item at a particular slot in hot bar. 
    public GameObject GetGameObject(int item_num)
    {
        if (item_num < 0 || item_num >= HotBarSize) { return null; }
        return HotbarInventory[item_num];
    }
    //Update the data of the Object_Data script of the currently slot.
    public void setItemHold()
    {
        setSlot(NumberItemCurrentlyHolding);
    }
    //Swap to the item that player want to hold.  
    public void setSlot(int num)
    {
        if (CurrentlyHolding != null)
        {
            CurrentlyHolding.GetComponent<Object_Data>().isHold = false;
        }
        if(HotbarInventory[(int)num] != null)
        {
            CurrentlyHolding = HotbarInventory[(int)num];
            NumberItemCurrentlyHolding = num;
            CurrentlyHolding.GetComponent<Object_Data>().isHold = true;
        }
        else
        {
            CurrentlyHolding = HotbarInventory[(int)num];
            NumberItemCurrentlyHolding = num;
        }


    }
    //Set the slot with the item the player want to hold. 
    public void setCurItemHold(int num)
    {
        NumberItemCurrentlyHolding += num;
        if (NumberItemCurrentlyHolding < 0)
        {
            NumberItemCurrentlyHolding = HotBarSize - 1;
        }
        if (NumberItemCurrentlyHolding >= HotBarSize)
        {
            NumberItemCurrentlyHolding = 0;
        }
    }
    //Return the item that player is currently holding. 
    public GameObject getCurHold()
    {
        return CurrentlyHolding;
    }
    //Managing UI

    /*
     * Add texture of the item when player is currently UI slot in Hot Bar or Main Inventory
     */
    public void AddTexture(int a)
    {
        if(a > 20)
        {
            a -= 21;
            if (HotbarInventory[a] != null)
            {
                HotbarInventory_UI[a].GetComponent<RawImage>().texture = HotbarInventory[a].GetComponent<Object_Data>().ObjectImage;
            }
            else
            {
                HotbarInventory_UI[a].GetComponent<RawImage>().texture = null;
            }
        }
        else
        {
            a -= 1;
            if (m_Inventory[a] != null)
            {
                m_Inventory_UI[a].GetComponent<RawImage>().texture = m_Inventory[a].GetComponent<Object_Data>().ObjectImage;
            }
            else
            {
                m_Inventory_UI[a].GetComponent<RawImage>().texture = null;
            }
        }
    }
    //Check if the hand is currently hold a item to turn off the UI texture at the slot. 
    public void checkhand(int a, int b)
    {
        if(a > 20 || b > 20)
        {
            if ((a - 21) == NumberItemCurrentlyHolding)
            { 
                if (CurrentlyHolding != null)
                {
                    CurrentlyHolding.GetComponent<Object_Data>().isHold = false;
                }
            }
            if ((b -21)  == NumberItemCurrentlyHolding)
            {
                if (CurrentlyHolding != null)
                {
                    CurrentlyHolding.GetComponent<Object_Data>().isHold = false;
                }
            }
           
        }
    }
    //Swap Item in UI. 
    public void swapItem(int a, int b)
    {
        GameObject tmpObject;
        int tmpb, tmpa;
        checkhand(a, b);
        if (b > 20)
        {
            tmpb =  b - (21);
            if(a > 20)
            {
                tmpa = a - (21);
                tmpObject = HotbarInventory[tmpa];
                HotbarInventory[tmpa] = HotbarInventory[tmpb];
                HotbarInventory[tmpb] = tmpObject;
                
            }
            else
            {
                tmpa = a - (1);
                tmpObject = m_Inventory[tmpa];
                m_Inventory[tmpa] = HotbarInventory[tmpb];
                HotbarInventory[tmpb] = tmpObject;

            }
        }
        else
        {
            tmpb = b - (1);
            if (a > 20)
            {
                tmpa = a - (21);
                tmpObject = HotbarInventory[tmpa];
                HotbarInventory[tmpa] = m_Inventory[tmpb];
                m_Inventory[tmpb] = tmpObject;
            }
            else
            {
                tmpa = a - (1);
                tmpObject = m_Inventory[tmpa];
                m_Inventory[tmpa] = m_Inventory[tmpb];
                m_Inventory[tmpb] = tmpObject;
            }
        }
        tmpb = b;
        tmpa = a;
        AddTexture(tmpa);
        AddTexture(tmpb);
        CurrentlyHolding = HotbarInventory[NumberItemCurrentlyHolding];
        if(CurrentlyHolding != null)
        {
            CurrentlyHolding.GetComponent<Object_Data>().isHold = true;
        }
        
    }
   
}
