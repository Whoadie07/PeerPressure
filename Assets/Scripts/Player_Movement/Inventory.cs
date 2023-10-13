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

    //What the character is hold
    public GameObject CurrentlyHolding = null;
    public GameObject tmpHolder = null;
    public int NumberItemCurrentlyHolding = 0;
    public bool isSelect;
    public int currectSelect;


    private void Start()
    {
        OpenMainInventory = false;
        mainInventory.SetActive(OpenMainInventory);
    }
    private void Update()
    {
        if (OpenMainInventory)
        {
            mainInventory.SetActive(true);
        }
        else
        {
            mainInventory.SetActive(false);
        }
        CurrentlyHolding = HotbarInventory[NumberItemCurrentlyHolding];
    }
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
    public GameObject GetGameObject(int item_num)
    {
        if (item_num < 0 || item_num >= HotBarSize) { return null; }
        return HotbarInventory[item_num];
    }
    public void setItemHold()
    {
        setSlot(NumberItemCurrentlyHolding);
    }
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
    public GameObject getCurHold()
    {
        return CurrentlyHolding;
    }
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
