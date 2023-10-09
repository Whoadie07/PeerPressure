using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Object_Data : MonoBehaviour
{
    //private Variables
    private Vector3 o_original_size = new Vector3();

    //Public Variables
    public Vector3 o_change_size = new Vector3(); //Set the size of the object with in the player hand.
    public bool isContain = false; //Boolean variable for item to check if the it is in the player inventory.
    public bool isHold = false; //Boolean variable to check if the item is currently being hold in player hand.
    public GameObject play_hand = null; //The hand that holding the item.
    public GameObject Object = null;
    public Texture ObjectImage = null;


    // Start is called before the first frame update
    void Start()
    {
        //Get the original size of object.
        o_original_size = this.GetComponent<Transform>().localScale;

    }

    // Update is called once per frame
    void Update()
    {
        if (!isHold && !isContain)
        {
            //this.GetComponent<Transform>().localScale.Set(o_original_size.x, o_original_size.y, o_original_size.z);
            Object.GetComponent<MeshRenderer>().enabled = true;
            this.GetComponentInChildren<MeshRenderer>().enabled = true;
        }
        if (isContain && isHold)
        {
            this.GetComponent<Transform>().localScale.Set(o_change_size.x, o_change_size.y, o_change_size.z);
            this.transform.position = play_hand.GetComponent<Transform>().position;
            this.transform.rotation = play_hand.GetComponent<Transform>().rotation;
            Object.GetComponent<MeshRenderer>().enabled = true;
            this.GetComponentInChildren<MeshRenderer>().enabled = true;
        }
        if (isContain && !isHold)
        {
            this.GetComponent<Transform>().localScale.Set(o_change_size.x, o_change_size.y, o_change_size.z);
            this.transform.position = play_hand.GetComponent<Transform>().position;
            this.transform.rotation = play_hand.GetComponent<Transform>().rotation;
            Object.GetComponent<MeshRenderer>().enabled = false;
            this.GetComponentInChildren<MeshRenderer>().enabled = false;
        }

    }
}
