using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuOption : MonoBehaviour
{
    [SerializeField]
    private GameObject UI;
    [SerializeField]
    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        UI.SetActive(false);
        button.onClick.AddListener(ToggleMenu);
    }

    private void ToggleMenu()
    {
        if (UI.activeSelf == false)
        {
            UI.SetActive(true);
        }
        else if (UI.activeSelf == true)
        {
            UI.SetActive(false);
        }
        
    }
}
