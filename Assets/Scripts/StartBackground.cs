using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartBackground : MonoBehaviour
{
    private Canvas Start_UI;
    private Image background;
    // Start is called before the first frame update
    void Start() {
        Start_UI = FindObjectOfType<Canvas>();
        background = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update() {
        float canvas_height = Start_UI.GetComponent<Canvas>().pixelRect.height;
        float canvas_width = Start_UI.GetComponent<Canvas>().pixelRect.width;
        background.rectTransform.sizeDelta = new Vector2 (canvas_width, canvas_height);
    }
}
