using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RawImage selectButton;
    protected bool MoveOver;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (MoveOver)
        {
            selectButton.enabled = true;
        }
        else
        {
            selectButton.enabled = false;
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        MoveOver = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        MoveOver = false;
    }
}
