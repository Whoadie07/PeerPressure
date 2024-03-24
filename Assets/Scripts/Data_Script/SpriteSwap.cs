using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpriteSwap : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private Sprite highlightSprite;

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.GetComponent<Image>().sprite = highlightSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.GetComponent<Image>().sprite = sprite;
    }
}
