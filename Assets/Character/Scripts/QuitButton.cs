using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour
{
    void Start() {
        
    }
    void OnMouseDown() {
        Application.Quit();
    }
    void Update () {
        
    }
}
