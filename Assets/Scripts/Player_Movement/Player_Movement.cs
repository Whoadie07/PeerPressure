using Cinemachine;
using System.Linq;
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
    public bool NpcInteracting = false;
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

    //Player Path
    public PathObject[] playerPath = new PathObject[1];

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
        if (Input.GetMouseButtonDown(0) && !PlayerHand.GetComponent<Inventory>().OpenMainInventory && !playerMenu.activeSelf && !NpcInteracting)
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

                //Player can intereact with NPC.
                if (m_tempRadius <= interactRadius && tempInteractable.GetComponent<NPC_Movement>() != null)
                {
                    //Debug.Log("I have a NPC_Movement Script");
                    NpcInteracting = true;
                    tempInteractable.GetComponent<NPC_Movement>().IsInteracting = true;
                    tempInteractable.GetComponent<NPC_Movement>().InteractTarget = this.gameObject;
                    tempInteractable.GetComponent<NPC_Movement>().UpdateNPC();
                    tempInteractable = null;
                }
                //Player can intereact with Object.
                else if (m_tempRadius <= interactRadius && tempInteractable.GetComponent<Object_Data>() != null)
                {   //Player interact with object
                    //Debug.Log("I have a Object_data Script");
                    loadObjecttoHand(tempInteractable);
                }
                else
                {
                    //Player can move around the scene.
                    if (Physics.Raycast(rayinfo, out hitinfo))
                    {
                        agent.SetDestination(hitinfo.point);
                    }
                }
            }
        }
        //Switch item in the player hot bar. 
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
        //Drop item.
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
        //Open main inventory.
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
        //Open Player UI
        if (Input.GetKeyDown("w"))
        {
            playerMenu.SetActive(!playerMenu.activeSelf);
        }
        for(int i = 0; i < playerPath.Length; i++)
        {
            if (playerPath.ElementAt(i) == null) { continue; }
            if (playerPath.ElementAt(i).GetType() == typeof(CollectPath))
            {
                CollectPath a = (CollectPath)playerPath[i];
                if (!playerPath.ElementAt(i).pathBegin)
                {
                    a.pathBegin = true;
                    a.begin(PlayerHand, PlayerHand.GetComponent<Inventory>());
                    a.checkPath(PlayerHand.GetComponent<Inventory>());
                    playerPath[i]= a;
                    Debug.Log("Run 1");
                }
                else
                {
                    a.checkPath(PlayerHand.GetComponent<Inventory>());
                    Debug.Log("Run 2");
                }
            }
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
        //Zoom in Function (Not yet implement)
        if (cur.button == 1 && isZoom == 1)
        {
            turn.x += Input.GetAxis("Mouse X");
            turn.y += Input.GetAxis("Mouse Y");

        }
        //Switch item with Scroll wheel
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
