using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class endText : MonoBehaviour
{
    public static string end_text = "Tie";
    public GameObject text;
    // Start is called before the first frame update
    void Start() {
        text.GetComponent<TMP_Text>().text = end_text;
        // text.text = end_text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
