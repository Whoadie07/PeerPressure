using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.XR;

public class Player_Movement : MonoBehaviour
{
    //Public variables

    //The player movement controller 
    public NavMeshAgent agent;

    //Player inventory
    public GameObject PlayerHand;

    //Player interactable gameobject 
    public GameObject PlayerDataServer;
    public GameObject interactable;
    public GameObject tempInteractable;

    //The camera variable 
    public Camera cam; //Main character camera
    public GameObject cam1; // The Cinamachine camera

    //Player Animator
    public Animator player_animotr;

    //Player Menu
    public GameObject playerMenu;

    //Private variables.
    private float playerSpeed = 5.0f;
    private float interactRange = 100.0f;
    private float interactRadius = 5.0f;
    private float m_camSpeed_XAxis = 300.0f;
    private float m_camSpeed_YAxis = 4.0f;

    //Protected Variables and Refereneses
    protected RaycastHit hitinfo;
    protected Ray rayinfo;
    protected Vector2 turn;
    protected int isZoom = -1;

    // Start is called before the first frame update
    void Start()
    {
        //Set the Navigation Mesh Agent to Player Speed;
        agent.speed = playerSpeed;
    }

    // Update is called once per frame
    void Update()
    {

        //Player Interactable and Movement by left-click
        if (Input.GetMouseButtonDown(0) && !PlayerHand.GetComponent<Inventory>().OpenMainInventory && !playerMenu.activeSelf)
        {
            rayinfo = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(rayinfo, out hitinfo, interactRange))
            {
                tempInteractable = hitinfo.transform.gameObject;
                Transform m_tempPosition = tempInteractable.GetComponent<Transform>();
                float p_x = this.transform.position.x - m_tempPosition.position.x;
                float p_y = this.transform.position.y - m_tempPosition.position.y;
                float p_z = this.transform.position.z - m_tempPosition.position.z;
                float m_tempRadius = Mathf.Sqrt(Mathf.Pow(p_x, 2) + Mathf.Pow(p_y, 2) + Mathf.Pow(p_z, 2));

                //NPC interact

                if (m_tempRadius <= interactRadius && tempInteractable.GetComponent<NPC_Movement>() != null)
                {
                    //Debug.Log("I have a NPC_Movement Script");
                    tempInteractable.GetComponent<NPC_Movement>().IsInteracting = true;
                    tempInteractable.GetComponent<Transform>().rotation = new Quaternion(-1, -1, -1, 1)*gameObject.transform.rotation;
                    tempInteractable.GetComponent<NPC_Movement>().UpdateNPC();
                }
                else if (m_tempRadius <= interactRadius && tempInteractable.GetComponent<Object_Data>() != null)
                {   //Player interact with object
                    //Debug.Log("I have a Object_data Script");
                    loadObjecttoHand(tempInteractable);
                }
                else
                {
                    //Character movement
                    if (Physics.Raycast(rayinfo, out hitinfo))
                    {
                        agent.SetDestination(hitinfo.point);
                    }
                }
            }
        }
        int inf = PlayerHand.GetComponent<Inventory>().HotbarInventory.Length;
        for (int i = 1; i <= inf; i++)
        {
            if ((Input.GetKeyDown(i.ToString())))
            {
                Debug.Log("Press: " + i.ToString());
                if (PlayerHand.GetComponent<Inventory>().HotbarInventory[i - 1] == null)
                {
                    Debug.Log("Not item found");
                }
                else
                {
                    PlayerHand.GetComponent<Inventory>().setSlot((i - 1));
                }
            }
        }
        if (Input.GetKeyDown("q"))
        {
            int hand = PlayerHand.GetComponent<Inventory>().NumberItemCurrentlyHolding;
            if (PlayerHand.GetComponent<Inventory>() != null)
            {
                if (PlayerHand.GetComponent<Inventory>().HotbarInventory[hand].GetComponent<Object_Data>() != null)
                {
                    PlayerHand.GetComponent<Inventory>().HotbarInventory[hand].GetComponent<Object_Data>().isContain = false;
                    PlayerHand.GetComponent<Inventory>().HotbarInventory[hand].GetComponent<Object_Data>().isHold = false;
                }
                
                PlayerHand.GetComponent<Inventory>().HotbarInventory[hand] = null;
                PlayerHand.GetComponent<Inventory>().HotbarInventory_UI[hand].GetComponent<RawImage>().texture = null;
                PlayerHand.GetComponent<Inventory>().CurrentlyHolding = null;
            }
            

        }
        if (Input.GetKeyDown("e"))
        {
            if (!PlayerHand.GetComponent<Inventory>().OpenMainInventory)
            {
                PlayerHand.GetComponent<Inventory>().OpenMainInventory = true;
            }
            else
            {
                PlayerHand.GetComponent<Inventory>().OpenMainInventory = false;
            }
            
        }
        if (Input.GetKeyDown("w"))
        {
            playerMenu.SetActive(!playerMenu.activeSelf);
        }

    }
    void OnGUI()
    {
        //Rotation the Camera Function with change is speed
        Event cur = Event.current;
        if (cur.button == 1 && isZoom == -1)
        {
            cam1.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = m_camSpeed_XAxis;
            cam1.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = m_camSpeed_YAxis;
        }
        else
        {
            cam1.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = 0;
            cam1.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = 0;
        }
        //Zoom in Function
        if (cur.button == 1 && isZoom == 1)
        {
            turn.x += Input.GetAxis("Mouse X");
            turn.y += Input.GetAxis("Mouse Y");

        }
        if (cur.isScrollWheel || cur.button == 2)
        {
            if (cur.delta.y > 0.0f)
            {
                PlayerHand.GetComponent<Inventory>().setCurItemHold(1);
                PlayerHand.GetComponent<Inventory>().setItemHold();
            }
            else if (cur.delta.y < 0.0f)
            {
                PlayerHand.GetComponent<Inventory>().setCurItemHold((-1));
                PlayerHand.GetComponent<Inventory>().setItemHold();
            }
            else
            {
                Debug.Log("Problem with scroll Wheel");
            }
        }
    }
    //Set the Object to Player Hand 
    public void loadObjecttoHand(GameObject instance_Object)
    {
        int numberSlot = 0;
        PlayerHand.GetComponent<Inventory>().setItem(instance_Object);
        numberSlot = PlayerHand.GetComponent<Inventory>().NumberItemCurrentlyHolding;
        instance_Object.transform.position = PlayerHand.GetComponent<Transform>().position;
        instance_Object.transform.rotation = PlayerHand.GetComponent<Transform>().rotation;
    }
}
