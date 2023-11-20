using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollectPath : PathObject
{
    public static int numItem = 1;
    public string[] itemlist= new string[numItem];
    public int[] itemlistNeed = new int[numItem];
    private int[] itemlistCorrect = new int[numItem];
    
    public void begin()
    {
        pathComplete = false;
        for(int i = 0; i  < numItem; i++)
        {
            itemlistCorrect[i] = 0;
        }
    }
    public void updateitem(GameObject obj)
    {
        if(obj.GetComponent<Object_Data>() != null)
        {
            int itemlocation = -1;
            for(int i = 0; i < itemlist.Length; i++)
            {
                if (obj.GetComponent<Object_Data>().object_name.Equals(itemlist[i]))
                {
                    itemlocation = i;
                    break;
                }
            }
            if(itemlocation != -1)
            {
                itemlistCorrect[itemlocation]++;
            }
        }
    }
    public void end()
    {
        pathComplete = true;
    }
}
