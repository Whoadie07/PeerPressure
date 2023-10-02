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
        if(MoveOver)
        {
            if(Input.GetMouseButtonDown(0))
            {
                if (ObjectInSlot != null && s_inventory.isSelect)
                {
                    GameObject tmpObject = ObjectInSlot;
                    ObjectInSlot = s_inventory.tmpHolder.GetComponent<Slot>().ObjectInSlot;
                    s_inventory.tmpHolder.GetComponent<Slot>().selectIcon.enabled = false;
                    s_inventory.tmpHolder.GetComponent<Slot>().ObjectInSlot = tmpObject;
                    s_inventory.tmpHolder = null;
                    s_inventory.currectSelect = 0;
                    s_inventory.isSelect = false;
                    selectIcon.enabled = true;
                }
                else if (ObjectInSlot != null && !s_inventory.isSelect)
                {
                    s_inventory.isSelect = true;
                    s_inventory.currectSelect = SlotNumber;
                    s_inventory.tmpHolder = this.gameObject;
                    selectIcon.enabled = true;
                } 
                else if (ObjectInSlot == null && s_inventory.isSelect)
                {
                    
                    if(s_inventory.tmpHolder.GetComponent<Slot>() != null)
                    {
                        Debug.Log("it work");
                        ObjectInSlot = s_inventory.tmpHolder.GetComponent<Slot>().ObjectInSlot;
                        s_inventory.tmpHolder.GetComponent<Slot>().ObjectInSlot = null;
                        s_inventory.tmpHolder.GetComponent<Slot>().selectIcon.enabled = false;
                    }
                    s_inventory.currectSelect = 0;
                    s_inventory.isSelect = false;
                    s_inventory.tmpHolder = null;

                }
                else if(!s_inventory.isSelect)
                {
                    s_inventory.isSelect = true;
                    s_inventory.currectSelect = SlotNumber;
                    s_inventory.tmpHolder = this.gameObject;
                    selectIcon.enabled = true;
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
