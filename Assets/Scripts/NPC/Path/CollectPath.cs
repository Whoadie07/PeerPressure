using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "Collect Path", menuName = "Collect Path")]
public class CollectPath : PathObject
{
    public static int numItem = 1;
    public string[] itemlist= new string[numItem];
    public int[] itemlistNeed = new int[numItem];
    private int[] itemlistCorrect = new int[numItem];

    //NPC Dialogue
    public NarrativeNode dialogueEndPath;
    
    public void begin(GameObject obj, Inventory pathInventory)
    {
        pathComplete = false;
        if (obj.GetComponent<Inventory>() == null)
        {
            return;
        }
        pathInventory = obj.GetComponent<Inventory>();
        for(int i = 0; i  < numItem; i++)
        {
            itemlistCorrect[i] = 0;
        }
    }
    public void checkPath(Inventory pathInventory)
    {
        if (pathInventory == null)
        {
            Debug.Log("there not inventory");
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
            end(pathInventory);
        }
    }
    public void update(GameObject obj)
    {
        if(obj == null) { return; }
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
    public void end(Inventory pathInventory)
    {
        pathComplete = true;
    }
}
