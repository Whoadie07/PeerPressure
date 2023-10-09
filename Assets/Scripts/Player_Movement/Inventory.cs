using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
    public GameObject CurruntlyHolding = null;
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
            }
           
        }
        else
        {
            Debug.Log("Not enough space");
            //Something.
        }
        if (CurruntlyHolding == null)
        {
            NumberItemCurrentlyHolding = avaSlot;
            setItemHold();
            HotbarInventory[avaSlot].GetComponent<Object_Data>().isHold = true;
            

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
        if (CurruntlyHolding != null)
        {
           // CurruntlyHolding.GetComponent<BoxCollider>().enabled = false;
            //CurruntlyHolding.GetComponent<MeshRenderer>().enabled = false;
            //CurruntlyHolding.GetComponent<Object_Data>().isHold = false;
        }
        CurruntlyHolding = m_Inventory[(int)num];
        NumberItemCurrentlyHolding = num;
        //CurruntlyHolding.GetComponent<BoxCollider>().enabled = true;
        //CurruntlyHolding.GetComponent<MeshRenderer>().enabled = true;
        //CurruntlyHolding.GetComponent<Object_Data>().isHold = true;


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
        return CurruntlyHolding;
    }
}
