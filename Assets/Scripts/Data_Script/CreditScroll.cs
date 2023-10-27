using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CreditScroll : MonoBehaviour
{
    //Credit Box
    public GameObject creditbox;
    public float credit_speed = 100000.0f;

    //Home Button
    public GameObject creditHomeButton;

    //Fade-In
    public Image FadeCredit;
    public float FadeSpeed = 25f;

    // Start is called before the first frame update
    void Start()
    {
        creditbox.SetActive(true);
        creditHomeButton.SetActive(false);
        FadeCredit.color = new Color(FadeCredit.color.r, FadeCredit.color.g, FadeCredit.color.b, 1f);
        float y_position = creditbox.GetComponent<RectTransform>().sizeDelta.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (FadeCredit.color.a <= 0)
        {
            FadeCredit.enabled = false;
            if (creditbox.GetComponent<RectTransform>().position.y > 900)
            {
                creditHomeButton.SetActive(true);
                float y_position = creditbox.GetComponent<RectTransform>().sizeDelta.y;
                creditbox.GetComponent<RectTransform>().position = new(creditbox.GetComponent<RectTransform>().position.x, (-1 * y_position) + 400, creditbox.GetComponent<RectTransform>().position.z);
            }
            creditbox.GetComponent<RectTransform>().position = new(creditbox.GetComponent<RectTransform>().position.x, (creditbox.GetComponent<RectTransform>().position.y + credit_speed), creditbox.GetComponent<RectTransform>().position.z);
        }
        else
        {
            FadeCredit.color = new Color(FadeCredit.color.r, FadeCredit.color.g, FadeCredit.color.b, FadeCredit.color.a - FadeSpeed * Time.deltaTime);
        }
    }
}
