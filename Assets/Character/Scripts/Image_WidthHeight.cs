using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Image_WidthHeight : MonoBehaviour {
    private Canvas Gaming_UI;
    private Image image;
    // x軸左邊(-1)或右邊(1)
    public int x = 1;
    private GameObject frame;
    private RectTransform frame_RT;

    // Start is called before the first frame update
    void Start() {
        Gaming_UI = FindObjectOfType<Canvas>();
        image = GetComponent<Image>();
        // 將其他遊戲物件引入
        frame = GameObject.Find ("selfBloodFrame_Image");
        frame_RT = (RectTransform)frame.transform;
    }

    // Update is called once per frame  
    void Update() {
        float canvas_height = Gaming_UI.GetComponent<Canvas>().pixelRect.height;
        float canvas_width = Gaming_UI.GetComponent<Canvas>().pixelRect.width;
        float frame_height = frame_RT.rect.height;
        float frame_width = frame_RT.rect.width;
        /* 調整image的大小(Height.Weight)
          image.rectTransform.sizeDelta = new Vector2 (); */
        
        image.rectTransform.anchoredPosition  = new Vector3 
                                                (x * ((canvas_width/2)-(frame_width/2)), (canvas_height/2)-(frame_height/2), 0);
    }
}
