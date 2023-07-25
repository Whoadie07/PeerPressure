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
     2) Camera: [0] is main camera [1] is and [2] is zoom function camera
     3) Character
     4) Environment
     5) Sound Dictionary
     */
    [TextArea(3,1000)][SerializeField] protected string note = "The Server for quick and access to Gameobject, Camera, and etc;\r\n     Type Objects:\r\n     1) GameObject with Scripts that are important, but cannot fit anywhere else.\r\n     2) Camera: [0] is main camera and [1] is zoom function camera\r\n     3) Character\r\n     4) Environment\r\n     5) Sound Dictionary";
    //GameObject Data
    public static int GameobjectSize;
    public GameObject[] GameobjectDataServer = new GameObject[GameobjectSize];

    //Camera Data
    public static int CameraSize;
    public GameObject[] CameraDataServer = new GameObject[CameraSize];

    //Character Data
    public static int CharacterSize;
    public GameObject[] CharacterDataServer = new GameObject[CharacterSize];

    //Environment Data
    public static int EnvironmentSize;
    public GameObject[] EnvironmentDataServer = new GameObject[EnvironmentSize];

    //Sound Dictionary Date
    public static int SoundDictionarySize;
    public Sound_Dict[] SoundDictsServer = new Sound_Dict[SoundDictionarySize];

    //Connect a object from the server to the object by passing it the obj 
    public Object Connect(int typeObject, int index)
    {
        Object obj = null;
        switch(typeObject)
        {
            case 1:
                GameObject tmp = GameobjectDataServer[index];
                obj = (Object)tmp;
                break;
            case 2:
                GameObject tmp2 = CameraDataServer[index];
                obj = (Object)tmp2;
                break;
            case 3:
                GameObject tmp3 = CharacterDataServer[index];
                obj = (Object)tmp3;
                break;
            case 4:
                GameObject tmp4 = EnvironmentDataServer[index];
                obj = (Object) tmp4;
                break;
            case 5:
                Sound_Dict tmp5 = SoundDictsServer[index];
                obj = (Object) tmp5;
                break;
            default:
                Debug.Log("This Object is not one Object Types");
                return null;
        }
        return obj;
    }
    //Connection object to other object in the server but simple.
    public GameObject ConnectGameObject(int index)
    {
        return GameobjectDataServer[index];
    }
    public GameObject ConnectCamera(int index)
    {
        return CameraDataServer[index];
    }
    public GameObject ConnectCharacter(int index)
    {
        return CharacterDataServer[index];
    }
    public GameObject ConnectEnvironment(int index)
    {
        return EnvironmentDataServer[index];
    }
    public Sound_Dict ConnectSoundDict(int index)
    {
        return SoundDictsServer[index];
    }
}
