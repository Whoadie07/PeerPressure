using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This is the pressure level meter in the game.
public class PressureLevel : MonoBehaviour
{
    [SerializeField]
    private ThePressure Peer;
    [SerializeField]
    private RectTransform rt;
    [SerializeField]
    private Image image;

    private void Update()
    {
        rt.sizeDelta = new Vector2(Peer.Pressure, rt.sizeDelta.y);
    }
}
