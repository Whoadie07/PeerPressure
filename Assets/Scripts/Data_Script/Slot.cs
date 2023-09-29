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
                if (ObjectInSlot != null && s_inventory)
                {
                    GameObject tmpObject = ObjectInSlot;
                    ObjectInSlot = s_inventory.tmpHolder.GetComponent<Slot>().ObjectInSlot;
                    s_inventory.tmpHolder.GetComponent<Slot>().selectIcon.enabled = false;
                    s_inventory.tmpHolder.GetComponent<Slot>().ObjectInSlot = tmpObject;
                    s_inventory.tmpHolder = this.gameObject;
                    selectIcon.enabled = true;
                }
                else if (ObjectInSlot != null && !s_inventory)
                {
                    s_inventory.isSelect = true;
                    s_inventory.tmpHolder = this.gameObject;
                    selectIcon.enabled = true;
                } 
                else if (ObjectInSlot == null && s_inventory)
                {
                    ObjectInSlot = s_inventory.tmpHolder.GetComponent<Slot>().ObjectInSlot;
                    s_inventory.tmpHolder.GetComponent<Slot>().selectIcon.enabled = false;
                    s_inventory.tmpHolder.GetComponent<Slot>().ObjectInSlot = null;
                    s_inventory.tmpHolder = null;

                }
            }
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
