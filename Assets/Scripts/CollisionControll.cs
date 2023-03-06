using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CollisionControll : MonoBehaviour
{
    public  float rayPos;
    public  Image hpBar;
    private int hp;
     
    RaycastHit hitObject;
    
    void Start() {
        hp = 10;
    }

    void Update() { 
        Vector3 newPos = transform.position;
        newPos.y = newPos.y + rayPos;
        Vector3 newTarget = transform.forward * 5.0f;
        if (Physics.Raycast (newPos, transform.forward, out hitObject, 5.0f)) { 
            newTarget = hitObject.point - newPos;
            newTarget.y = 0.0f;
            // Debug.Log ("Ray hit " + hitObject.transform.name + "!");    
        }
        Debug.DrawRay (newPos, newTarget, Color.red);
    }
    
    private void OnTriggerEnter (Collider other) {
        Debug.Log (other.tag);
        if (other.tag != this.tag) {
            hp -= 2;
            hpBar.fillAmount -= 0.2f;
            Debug.Log (hpBar.fillAmount); 
            Debug.Log (this.tag + " was collided by " + other.tag);
            if (hp == 0) {
                EndText.end_text = "Winer  " + other.tag + " !!!";
                Cursor.lockState = CursorLockMode.None;
                SceneManager.LoadScene ("end");
            }
            Destroy (other.gameObject);
        }
    }
}
