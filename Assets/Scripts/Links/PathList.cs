using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Node
{
    public CollectPath collect;
    public InteractionPath interact;
    public int identifier;
    public Node next;
}

[CreateAssetMenu]
public class PathList : ScriptableObject
{
    private Node head;
    private int size;
    private int baseID;

    void Start()
    {
        baseID = 0;
        size = 0;
        head = null;
    }
    public int AddCollect(CollectPath path)
    {
        int newID = 1000 + baseID;
        baseID += 10;
        if (head == null)
        {
            head = new Node();
            head.collect = path;
            head.interact = null;
            head.identifier = newID;
            head.next = null;
        }
        else
        {
            Node newNode = new Node();
            newNode.collect = path;
            newNode.interact = null;
            newNode.identifier = newID;
            newNode.next = head;
            head = newNode;
        }
        size++;
        return newID;
    }
    public int AddInteract(InteractionPath path)
    {
        int newID = 1000 + baseID;
        baseID += 10;
        if (head == null)
        {
            head = new Node();
            head.collect = null;
            head.interact = path;
            head.identifier = newID;
            head.next = null;
        }
        else
        {
            Node newNode = new Node();
            newNode.collect = null;
            newNode.interact = path;
            newNode.identifier = newID;
            newNode.next = head;
            head = newNode;
        }
        size++;
        return newID;
    }
    public void DeletePath(int id)
    {
        Node temp = head;
        Node prev = null;
        while ((temp != null)&&(temp.identifier != id))
        {
            prev = temp;
            temp = temp.next;
        }
        if (prev != null)
        {
            prev.next = temp.next;
            size--;
        }
        else
        {
            head = temp.next;
            size--;
        }
    }

    public Node CurrentPath(int id)
    {
        return head;
    }

    public void ResetList()
    {
        head = null;
    }
}
