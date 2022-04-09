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
    // Start is called before the first frame update
    void Start() {
        hp = 10;
    }

    // Update is called once per frame
    void Update() { 
        Vector3 newPos = transform.position;
        newPos.y = newPos.y + rayPos;
        // 撞到物件，調整射線長度
        Vector3 newTarget = transform.forward * 5.0f;
        // (起始位置, 方向, 射線碰撞物件, 射線距離)
        if (Physics.Raycast (newPos, transform.forward, out hitObject, 5.0f)) { 
            newTarget = hitObject.point - newPos;
            newTarget.y = 0.0f;
            // Debug.Log ("Ray hit " + hitObject.transform.name + "!");    
        }
        Debug.DrawRay (newPos, newTarget, Color.red);
    }
    /*
    private void OnCollisionEnter (Collision other) { 
        if (other.transform.name != "Plane")
            Debug.Log (this.name + " was collided by " + other.collider.name);
    }
    */
    private void OnTriggerEnter (Collider other) {
        Debug.Log (other.tag);
        if (other.tag != this.tag) {
            hp -= 2;
            hpBar.fillAmount -= 0.2f;
            Debug.Log (hpBar.fillAmount); 
            Debug.Log (this.tag + " was collided by " + other.tag);
            if (hp == 0) {
                endText.end_text = "Winer  " + other.tag + " !!!";
                SceneManager.LoadScene ("end");
            }
            Destroy (other.gameObject);
        }
    }
}
