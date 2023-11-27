using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.XR;
using UnityEngine;
[CreateAssetMenu(fileName = "Collect Path", menuName = "Collect Path")]
public class CollectPath : PathObject
{
    public static int numItem = 1;
    public Inventory pathInventory;
    public string[] itemlist= new string[numItem];
    public int[] itemlistNeed = new int[numItem];
    private int[] itemlistCorrect = new int[numItem];
    
    public void begin(GameObject obj)
    {
        pathComplete = false;
        if (obj.GetComponent<Inventory>() == null)
        {
            return;
        }
        Debug.Log("There a inventory");
        pathInventory = obj.GetComponent<Inventory>();
        for(int i = 0; i  < numItem; i++)
        {
            itemlistCorrect[i] = 0;
        }
    }
    public void checkPath()
    {
        if (pathInventory == null)
        {
            return;
        }
        for (int i = 0; i < numItem; i++)
        {
            itemlistCorrect[i] = 0;
        }
        for (int i = 0;i < pathInventory.m_Inventory.Length;i++)
        {
            update(pathInventory.m_Inventory[i]);
        }
        for (int i = 0; i < pathInventory.HotbarInventory.Length; i++)
        {
            update(pathInventory.HotbarInventory[i]);
        }
        bool sf_setpathComplete = false;
        for(int i = 0; i < numItem; i++)
        {
            if (itemlistNeed[i] == itemlistCorrect[i])
            {
                sf_setpathComplete = true;
            }
            else
            {
                sf_setpathComplete = false;
                break;
            }
        }
        if (sf_setpathComplete)
        {
            end();
        }
    }
    public void update(GameObject obj)
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
