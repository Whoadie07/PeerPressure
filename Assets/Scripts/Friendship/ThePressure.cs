using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is the pressure data object.
[CreateAssetMenu]
public class ThePressure : ScriptableObject
{
    [SerializeField]
    private int _pressure;

    public int Pressure
    {
        get { return _pressure; }
        set { _pressure = value; }
    }
}
