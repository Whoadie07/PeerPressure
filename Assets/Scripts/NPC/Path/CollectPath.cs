using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "Collect Path", menuName = "Collect Path")]
public class CollectPath : PathObject
{
    [SerializeField]
    private PathList list;

    public static int numItem = 1;
    public string[] itemlist= new string[numItem];
    public int[] itemlistNeed = new int[numItem];
    public int[] itemlistCorrect = new int[numItem];

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
        pathBegin = false;
        //list.DeletePath(ID);
    }
    public void takeItem(Inventory pathInventory)
    {
        if (pathInventory == null)
        {
            Debug.Log("there not inventory");
            return;
        }
        for(int j = 0; j < numItem; j++)
        {
            for (int i = 0; i < pathInventory.m_Inventory.Length; i++)
            {
                if (pathInventory.m_Inventory[i] == null) { continue; }
                
                if (pathInventory.m_Inventory[i].GetComponent<Object_Data>() != null)
                {
                    Object_Data temp_object = pathInventory.m_Inventory[i].GetComponent<Object_Data>();
                    if (temp_object.object_name.Equals(itemlist[j]))
                    {
                        itemlistCorrect[j] -= 1;
                        pathInventory.m_Inventory_UI[i].GetComponent<RawImage>().texture = null;
                        Destroy(pathInventory.m_Inventory[i]);
                        pathInventory.m_Inventory[i] = null;
                    }
                }
            }
            
            for (int i = 0; i < pathInventory.HotbarInventory.Length; i++)
            {
                if (pathInventory.HotbarInventory[i] == null) { continue; }
                
                if (pathInventory.HotbarInventory[i].GetComponent<Object_Data>() != null)
                {
                    Object_Data temp_object = pathInventory.HotbarInventory[i].GetComponent<Object_Data>();
                    if (temp_object.object_name.Equals(itemlist[j]))
                    {
                        itemlistCorrect[j] -= 1;
                        pathInventory.HotbarInventory_UI[i].GetComponent<RawImage>().texture = null;
                        Destroy(pathInventory.HotbarInventory[i]);
                        pathInventory.HotbarInventory[i] = null;
                    }
                }
            }
        }
    }
}
