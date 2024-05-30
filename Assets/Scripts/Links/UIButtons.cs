using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// This script is to install a mouse over button reader.
public class UIButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool isMouseOver = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
    }

    public bool IsMouseOverButton()
    {
        return isMouseOver;
    }
}
