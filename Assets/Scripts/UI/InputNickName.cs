using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class InputNickName : MonoBehaviour {

    [SerializeField]
    private TMP_InputField inputField;

    void Start() {

        if (PlayerPrefs.HasKey ("PlayerNickName")) {

            print ("已填入玩家名稱");
            inputField.text = PlayerPrefs.GetString("PlayerNickName");
        }
        
    }

    public void OnJoinGameClicked() {

        PlayerPrefs.SetString("PlayerNickName", inputField.text);
        PlayerPrefs.Save();

        SceneManager.LoadScene("Gaming");

    }
}
