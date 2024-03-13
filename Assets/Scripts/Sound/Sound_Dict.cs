using UnityEngine;

/*
 * An asset object that allow user to store Sound_Track and use in 
 * the Sound Player.
 */

[CreateAssetMenu(menuName = "Sound Dictionary",fileName = "Sound Dictionary need a Name", order = 0)]
public class Sound_Dict : ScriptableObject
{
    //The size of the Dictionary
    public static int s_Dict_Size;

    //The array will store the track in Dictinoary
    public Sound_Track[] tracks = new Sound_Track[s_Dict_Size];

    //For Comment 
    [TextArea (4, 10000)] public string comment = string.Empty;
    
    //Get the the Dictionary Size
    public int getDictSize()
    {
        return s_Dict_Size;
    }
}
