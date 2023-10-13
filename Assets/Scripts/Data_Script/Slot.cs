using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Inventory s_inventory;
    public GameObject ObjectInSlot;
    public RawImage selectIcon;
    public int SlotNumber;

    protected bool MoveOver;
    

    // Start is called before the first frame update
    void Start()
    {
        selectIcon.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(s_inventory.currectSelect == 0)
        {
            s_inventory.isSelect = false;
        }
        if(MoveOver)
        {
            if(Input.GetMouseButtonDown(0))
            {
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
    public void OnPointerEnter(PointerEventData eventData)
    {
        MoveOver = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        MoveOver = false;
    }
}
