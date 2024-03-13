using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/*
 This script is develop for UI representation the slot.
 */
public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //Public Varaibles

    //Inventory Datas
    public Inventory s_inventory;

    //Slot Information
    public GameObject ObjectInSlot;
    public RawImage selectIcon;
    public int SlotNumber;


    //Protected Varaibles
    protected bool MoveOver;
    

    // Start is called before the first frame update
    void Start()
    {
        selectIcon.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Check if the player have not select any slot.
        if(s_inventory.currectSelect == 0)
        {
            s_inventory.isSelect = false;
        }
        //Check if mouse is hover the slot
        if (MoveOver)
        {
            //If the player left click
            if(Input.GetMouseButtonDown(0))
            {
                //Player can switch item throughout the other slots.
                if (ObjectInSlot != null && !s_inventory.isSelect)
                {
                    s_inventory.currectSelect = SlotNumber;
                    s_inventory.isSelect = true;
                    s_inventory.tmpHolder = this.gameObject;
                }
                else if (ObjectInSlot != null && s_inventory.isSelect)
                {
                    if (s_inventory.tmpHolder != null)
                    {
                        if (s_inventory.tmpHolder.GetComponent<Slot>() != null)
                        {
                            GameObject tmpObject = this.ObjectInSlot;
                            this.ObjectInSlot = s_inventory.tmpHolder.GetComponent<Slot>().ObjectInSlot;
                            s_inventory.tmpHolder.GetComponent<Slot>().ObjectInSlot = tmpObject;
                        }
                        s_inventory.swapItem(this.SlotNumber, s_inventory.currectSelect);
                    }
                    s_inventory.currectSelect = 0;
                    s_inventory.tmpHolder = this.gameObject;
                } 
                else if (ObjectInSlot == null && s_inventory.isSelect)
                {
                    
                    if(s_inventory.tmpHolder != null)
                    {
                        if (s_inventory.tmpHolder.GetComponent<Slot>() != null)
                        {
                            this.ObjectInSlot = s_inventory.tmpHolder.GetComponent<Slot>().ObjectInSlot;
                            s_inventory.tmpHolder.GetComponent<Slot>().ObjectInSlot = null;
                        }
                        s_inventory.swapItem(this.SlotNumber, s_inventory.currectSelect);
                    }
                    s_inventory.currectSelect = 0;
                    s_inventory.isSelect = false;
                    s_inventory.tmpHolder = null;

                }
                else if(!s_inventory.isSelect)
                {
                    s_inventory.currectSelect = SlotNumber;
                    s_inventory.isSelect = true;
                    s_inventory.tmpHolder = this.gameObject;
                }
            }
            selectIcon.enabled = true;
        }
        else if (!s_inventory.isSelect ||(SlotNumber != s_inventory.currectSelect))
        {
            selectIcon.enabled = false;
        }
    }
    //Functions deal with mouse point is hover over slot
    public void OnPointerEnter(PointerEventData eventData)
    {
        MoveOver = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        MoveOver = false;
    }
}
