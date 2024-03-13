using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //public varaibles

    //The Image will show when the mouse pointer hover over the button.
    public RawImage selectButton;
    //Bool to check if the mouse pointer is hover. 
    protected bool MoveOver;

    // Update is called once per frame
    void Update()
    {
        //Check if the mouse is hover over the button.
        if (MoveOver)
        {
            selectButton.enabled = true;
        }
        else
        {
            selectButton.enabled = false;
        }
    }
    //Set true if the mouse pointer is about to hover over the button.
    public void OnPointerEnter(PointerEventData eventData)
    {
        MoveOver = true;
    }
    //Set false if the mouse pointer is about to exit the button area. 
    public void OnPointerExit(PointerEventData eventData)
    {
        MoveOver = false;
    }
}
