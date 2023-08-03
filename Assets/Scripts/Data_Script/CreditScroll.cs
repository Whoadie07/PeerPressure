using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditScroll : MonoBehaviour
{
    //Credit Box
    public GameObject creditbox;
    public float credit_speed = 0.5f;

    //Home Button
    public GameObject creditHomeButton;



    // Start is called before the first frame update
    void Start()
    {
        creditbox.SetActive(true);
        creditHomeButton.SetActive(false);
        float y_position = creditbox.GetComponent<RectTransform>().sizeDelta.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (creditbox.GetComponent<RectTransform>().position.y > 900)
        {
            creditHomeButton.SetActive(true);
            float y_position = creditbox.GetComponent<RectTransform>().sizeDelta.y;
            creditbox.GetComponent<RectTransform>().position = new(creditbox.GetComponent<RectTransform>().position.x, (-1*y_position) + 400, creditbox.GetComponent<RectTransform>().position.z);
        }
        creditbox.GetComponent<RectTransform>().position = new (creditbox.GetComponent<RectTransform>().position.x, (creditbox.GetComponent<RectTransform>().position.y + credit_speed), creditbox.GetComponent<RectTransform>().position.z);
    }
}
