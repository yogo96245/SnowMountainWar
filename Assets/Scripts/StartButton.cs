using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    void Start() {
        
    }
    void Update () {
        
    }
    void OnMouseDown() {
        SceneManager.LoadScene ("Gaming");
    }
}
