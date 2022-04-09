using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
    public float timeRemaining = 10;
    private bool timerIsRunning = false;
    private Text timeText;
    private Canvas canvas;
    private GameObject clock_Image;
    private RectTransform clock_Image_RT;
    public Image player1_Hp;
    public Image player2_Hp;
  
    private void Start() {
        // Starts the timer automatically
        timerIsRunning = true;
        timeText = GetComponent<Text> ();
        canvas = FindObjectOfType<Canvas>();
        clock_Image = GameObject.Find ("alarmClock_Image");
        clock_Image_RT = (RectTransform)clock_Image.transform;
    }

    void Update() {
        float canvas_height = canvas.GetComponent<Canvas>().pixelRect.height;
        float canvas_weight = canvas.GetComponent<Canvas>().pixelRect.width;
        float clock_height = clock_Image_RT.rect.height;
        // +18 才可達到理想位置
        clock_Image_RT.anchoredPosition = new Vector3 (0, (canvas_height/2)-(clock_height/2)+18);
        timeText.rectTransform.anchoredPosition  = new Vector3 (7, clock_Image_RT.localPosition.y-5, 0);
        
        if (timerIsRunning) {
            if (timeRemaining > 0) {
                DisplayTime(timeRemaining);
                timeRemaining -= Time.deltaTime;
            }
            else {
                if (player1_Hp.fillAmount > player2_Hp.fillAmount) endText.end_text = "Winer  player1 !!!";
                if (player1_Hp.fillAmount < player2_Hp.fillAmount) endText.end_text = "Winer  player2 !!!";
                timeRemaining = 0;
                timerIsRunning = false;
                SceneManager.LoadScene ("end");
            }
        }
    }

    void DisplayTime(float timeToDisplay) {
        //timeToDisplay += 1;

        //float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        //float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        //timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        timeText.text = string.Format("{0:00}", timeToDisplay);
    }
}