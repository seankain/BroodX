using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextDisplay : MonoBehaviour
{
    [SerializeField]
    private Text DisplayText;

    private float Elapsed = 0;

    private string Explanation = "Get inside!";
    private string DeadMessage = "Gross!";
    private string SuccessMessage = "You did it!";
    private bool Showing = false;
    private float ShowTime = 3;

    private void ShowText(string message,float showTime=3) 
    {
        DisplayText.enabled = true;
        DisplayText.text = message;
        Showing = true;
        ShowTime = showTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        //DisplayText = GetComponent<Text>();
        ShowText(Explanation);
    }

    // Update is called once per frame
    void Update()
    {
        if (Showing)
        {
            Elapsed += Time.deltaTime;
            if (Elapsed >= ShowTime) { DisplayText.enabled = false;Elapsed = 0; Showing = false; }
        }   
    }
}
