using Cinemachine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
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

    public PathList playerPath;
    public Text PathName = null;
    public Text PathDescription = null;

    // This is the link variable which allows the script to access data from other NPCs
    public Link link;

    // These buttons are for traversing the path_objects list in the objective UI
    [SerializeField]
    private Button back;
    [SerializeField]
    private Button next;

    // These buttons are for traversing through the hotbar inventory in the inventory UI
    [SerializeField]
    private Button left;
    [SerializeField] 
    private Button right; 

    // This is the pause button to pause the game and display the pause menu
    [SerializeField]
    private Button menu;

    // This button opened the inventory
    [SerializeField]
    private Button backpack;

    // If a player is holding an object that's selected in the hotbar inventory, this button gives them the option of dropping it
    [SerializeField]
    private Button drop;

    // This is the index of the objective path_objects array
    public int index = 0;

    // This is the index of the hotbar inventory array
    public int item = 0;

    // This image is to show what item in the hotbar is selected
    [SerializeField]
    private RectTransform selection;

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
        index = 0;
        back.onClick.AddListener(PressLeftButton);
        next.onClick.AddListener(PressRightButton);
        left.onClick.AddListener(LeftObject);
        right.onClick.AddListener(RightObject);
    }

    void stopPlayerMovement ()
    {
        agent.speed = 0.0f;
    }

    void continuePlayerMovement ()
    {
        agent.speed = playerSpeed;
    }
    // Update is called once per frame
    void Update()
    {
        if (DialogueManager.GetInstance().IsDialoguePlaying)
        {
            stopPlayerMovement();
        }
        else
        {
            continuePlayerMovement();
        }
        
        //Player Interactable and Movement by left-click
        // The player can't move when the mouse is over a button
        if (Input.GetMouseButtonDown(0) && !PlayerHand.GetComponent<Inventory>().OpenMainInventory && !playerMenu.activeSelf && !NpcInteracting &&
            (back.GetComponent<UIButtons>().IsMouseOverButton() == false && next.GetComponent<UIButtons>().IsMouseOverButton() == false
            && menu.GetComponent<UIButtons>().IsMouseOverButton() == false && backpack.GetComponent<UIButtons>().IsMouseOverButton() == false
            && (drop.IsActive() == false || (drop.IsActive() == true && drop.GetComponent<UIButtons>().IsMouseOverButton() == false)) && left.GetComponent<UIButtons>().IsMouseOverButton() == false
            && right.GetComponent<UIButtons>().IsMouseOverButton() == false))
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
                    for (int i = 0; i < playerPath.pathObjects.Length; i++)
                    {
                        // This determines if the path does not exist, is a collect path, or an interaction path
                        if (playerPath.pathObjects.ElementAt(i) == null) { continue; }
                        else if (playerPath.pathObjects.ElementAt(i).GetType() == typeof(CollectPath))
                        {
                            CollectPath a = (CollectPath)playerPath.pathObjects[i];
                            if (playerPath.pathObjects.ElementAt(i).NPC_Name.Equals(tempInteractable.GetComponent<NPC_Movement>().NPC_name))
                            {
                                // In a collect path, the player constantly checks if the player has collected all items so that they can give the items to the NPC
                                if (a.obtainedAll)
                                {
                                    a.takeItem(PlayerHand.GetComponent<Inventory>(), link);
                                    // The path is removed.
                                    playerPath.pathObjects[i] = null;
                                    continue;
                                }
                            }
                        }
                        else if (playerPath.pathObjects.ElementAt(i).GetType() == typeof(InteractionPath))
                        {
                            InteractionPath a = (InteractionPath)playerPath.pathObjects[i];
                            if (playerPath.pathObjects.ElementAt(i).NPC_Name.Equals(tempInteractable.GetComponent<NPC_Movement>().NPC_name))
                            {
                                if (a.GetInteracted())
                                {
                                    a.CompletedList(link);
                                    // The path is removed.
                                    playerPath.pathObjects[i] = null;
                                    continue;
                                }
                            }
                        }
                    }
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
        int inf = PlayerHand.GetComponent<Inventory>().HotbarInventory.Length;
        for (int i = 1; i <= inf; i++)
        {
            if ((Input.GetKeyDown(i.ToString())))
            {
                item = i-1;
            }
        }
        if (selection != null) selection.anchoredPosition = new Vector2(-150 + (100 * item), -400);
        PlayerHand.GetComponent<Inventory>().setSlot((item));
        //Drop item.
        if (Input.GetKeyDown("q"))
        {
            int hand = PlayerHand.GetComponent<Inventory>().NumberItemCurrentlyHolding;
            if (PlayerHand.GetComponent<Inventory>() != null)
            {
                if (PlayerHand.GetComponent<Inventory>().HotbarInventory != null)
                {
                    if (PlayerHand.GetComponent<Inventory>().HotbarInventory[hand] != null)
                    {
                        PlayerHand.GetComponent<Inventory>().HotbarInventory[hand].GetComponent<Object_Data>().isContain = false;
                        PlayerHand.GetComponent<Inventory>().HotbarInventory[hand].GetComponent<Object_Data>().isHold = false;
                        PlayerHand.GetComponent<Inventory>().HotbarInventory[hand] = null;
                        PlayerHand.GetComponent<Inventory>().HotbarInventory_UI[hand].GetComponent<RawImage>().texture = null;
                        PlayerHand.GetComponent<Inventory>().CurrentlyHolding = null;
                    }
                }
            }
        }
        // If the player is holding an object, then they have the option of dropping it using the drop button
        if (PlayerHand.GetComponent<Inventory>().getCurHold() != null) drop.GameObject().SetActive(true);
        else drop.GameObject().SetActive(false);
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
        PathName.text = "";
        PathDescription.text = "";
        // The Objective UI is changed when a player gains a new path or completes a path
        for (int i = 0; i < playerPath.pathObjects.Length; i++)
        {
            // This determines if the path does not exist, is a collect path, or an interaction path
            if (playerPath.pathObjects.ElementAt(i) == null) { continue; }
            else if (playerPath.pathObjects.ElementAt(i).GetType() == typeof(CollectPath))
            {
                CollectPath a = (CollectPath)playerPath.pathObjects[i];
                if (!playerPath.pathObjects.ElementAt(i).pathBegin)
                {
                    a.pathBegin = true;
                    a.begin(PlayerHand, PlayerHand.GetComponent<Inventory>());
                    a.checkPath(PlayerHand.GetComponent<Inventory>(), link);
                    playerPath.pathObjects[i] = a;
                }
                else
                {
                    a.checkPath(PlayerHand.GetComponent<Inventory>(), link);
                    if (a.pathComplete) ResetIndex();
                }
            }
            else if (playerPath.pathObjects.ElementAt(i).GetType() == typeof(InteractionPath))
            {
                InteractionPath a = (InteractionPath)playerPath.pathObjects[i];
                if (!playerPath.pathObjects.ElementAt(i).pathBegin)
                {
                    a.pathBegin = true;
                    a.Begin();
                    playerPath.pathObjects[i] = a;
                }
                else
                {
                    if (a.pathComplete) ResetIndex();
                }
            }
            if (i == index)
            {
                PathName.text += playerPath.pathObjects.ElementAt(i).path_name;
                PathDescription.text += playerPath.pathObjects.ElementAt(i).path_description;
            }
        }
        UpdateList();
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
    private void PressRightButton()
    {
        if (playerPath.pathObjects[index + 1] != null) index++;
    }
    private void PressLeftButton()
    {
        if (index > 0) index--;
    }

    private void ResetIndex()
    {
        index = 0;
    }

    // The path_object list is changed so that there would be no empty spaces in between paths.
    private void UpdateList()
    {
        for (int i = 0; i < playerPath.pathObjects.Length; i++)
        {
            if (playerPath.pathObjects[i] != null) 
            {
                if (playerPath.pathObjects[i].pathComplete == true)
                {
                    for (int j = i; j < playerPath.pathObjects.Length; j++)
                    {
                        if (playerPath.pathObjects[j] != null)
                        {
                            playerPath.pathObjects[j] = playerPath.pathObjects[j + 1];
                        }
                    }
                }
            }
        }
    }

    private void LeftObject()
    {
        if (item > 0) item--;
    }

    private void RightObject()
    {
        if (item < PlayerHand.GetComponent<Inventory>().HotbarInventory.Length - 1) item++;
    }
}
