using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This is the pressure level meter in the game.
public class PressureLevel : MonoBehaviour
{
    // These variables access the components of the bar.
    [SerializeField]
    private ThePressure Peer;
    [SerializeField]
    private RectTransform rt;
    [SerializeField]
    private Image image;

    private void Update()
    {
        // The bar adjusts based on the amount of Peer Pressure the player has.
        if (Peer.Pressure > 400)
        {   
            // The bar cannot go over 400
            rt.anchoredPosition = new Vector2(-200 + (400 / 2), 0);
            rt.sizeDelta = new Vector2(400, rt.sizeDelta.y);
        }
        else
        {
            rt.anchoredPosition = new Vector2(-200 + (Peer.Pressure / 2), 0);
            rt.sizeDelta = new Vector2(Peer.Pressure, rt.sizeDelta.y);
        }
    }
}
