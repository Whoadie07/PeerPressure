using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class Server : MonoBehaviour
{
    /*
     The Server for quick and access to Gameobject, Camera, and etc;
     Type Objects:
     1) GameObject with Scripts that are important, but cannot fit anywhere else.
     2) Camera
     3) Character
     4) Environment
     5) Sound Dictionary
     */

    //GameObject Data
    public static int go_size;
    public GameObject[] go_data_server = new GameObject[go_size];

    //Camera
    public static int cam_size;
    public Camera[] cam_data_server = new Camera[cam_size];

    //

    //Sound Date
    public static int sd_size;
    public Sound_Dict[] sound_Dicts_Server = new Sound_Dict[sd_size];

    //Connect to 
    public Object Connect(Object obj, int typeObject, int index)
    {
        switch(typeObject)
        {
            case 1:
                GameObject tmp = go_data_server[index];
                obj = tmp as Object;
                break;
            case 2:
                Camera tmp2 = cam_data_server[index];
                obj = tmp2 as Camera;
                break;
            case 3:

                break;
            default:
                Debug.Log("This Object is not one Object Types");
                return null;
        }
        return obj;
    }
    [SerializeField] private string note = "";
}
