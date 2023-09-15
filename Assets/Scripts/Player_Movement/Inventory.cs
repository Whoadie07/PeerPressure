using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private static int InventorySize = 9;
    public GameObject[] m_Inventory = new GameObject[InventorySize];
    public GameObject[] m_Inventory_UI = new GameObject[InventorySize];
    public GameObject CurruntlyHolding = null;
    public int NumberItemCurrentlyHolding = 0;

    public void setItem(GameObject item)
    {
        int avaSlot = avaiableSlot();
        if (avaSlot != -1)
        {
            m_Inventory[avaSlot] = item;
            m_Inventory[avaSlot].GetComponent<Object_Data>().isContain = true;
            m_Inventory_UI[avaSlot].GetComponent<RawImage>().enabled = true;
            
        }
        else
        {
            Console.WriteLine("Not enough space");
            //Something.
        }
        if (CurruntlyHolding == null)
        {
            NumberItemCurrentlyHolding = avaSlot;
            setItemHold();
            m_Inventory[avaSlot].GetComponent<Object_Data>().isHold = true;

        }
    }
    public int avaiableSlot()
    {
        for (int i = 0; i < InventorySize; i++)
        {
            if (m_Inventory[i] == null)
            {
                return i;
            }
        }
        return -1;
    }
    public GameObject GetGameObject(int item_num)
    {
        if (item_num < 0 || item_num >= InventorySize) { return null; }
        return m_Inventory[item_num];
    }
    public void setItemHold()
    {
        setSlot(NumberItemCurrentlyHolding);
    }
    public void setSlot(int num)
    {
        if (CurruntlyHolding != null)
        {
            CurruntlyHolding.GetComponent<BoxCollider>().enabled = false;
            CurruntlyHolding.GetComponent<MeshRenderer>().enabled = false;
            CurruntlyHolding.GetComponent<Object_Data>().isHold = false;
        }
        CurruntlyHolding = m_Inventory[(int)num];
        NumberItemCurrentlyHolding = num;
        CurruntlyHolding.GetComponent<BoxCollider>().enabled = true;
        CurruntlyHolding.GetComponent<MeshRenderer>().enabled = true;
        CurruntlyHolding.GetComponent<Object_Data>().isHold = true;


    }
    public void setCurItemHold(int num)
    {
        NumberItemCurrentlyHolding += num;
        if (NumberItemCurrentlyHolding < 0)
        {
            NumberItemCurrentlyHolding = InventorySize - 1;
        }
        if (NumberItemCurrentlyHolding >= InventorySize)
        {
            NumberItemCurrentlyHolding = 0;
        }
    }
    public GameObject getCurHold()
    {
        return CurruntlyHolding;
    }
}
