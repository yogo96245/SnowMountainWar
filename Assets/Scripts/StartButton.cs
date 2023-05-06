using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour {

    [SerializeField]
    private GameObject panel;

    void Start() {
        
    }
    void Update () {
        
    }
    void OnMouseDown() {
        panel.SetActive (true);
    }
}
