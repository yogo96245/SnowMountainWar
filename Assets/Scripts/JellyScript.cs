using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyScript : MonoBehaviour {   

    public float runSpeed;
    public GameObject snowBall_prefab;
    // 準心紋理
    public Texture2D crosshairTexture; 
    public float throw_velocity;
    public  Camera fpsCamera;
    private float lastShotTime = 0.0f;
    private float fireDelay = 0.1f;
    
    Animator jelly;
    Rigidbody rigid;
    Vector3 m_Movement;

    void Start() {
        jelly = GetComponent<Animator>();
        rigid  = GetComponent<Rigidbody>();
    }

    void Update() {
        float horizontal  = Input.GetAxis ("Horizontal_" + jelly.tag);
        float vertical = Input.GetAxis ("Vertical_" + jelly.tag);

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();
        m_Movement = transform.TransformDirection (m_Movement);
        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        transform.position += m_Movement * runSpeed * (isWalking ? 1f : 0f) * Time.deltaTime;

        if (Input.GetKey(KeyCode.W)) {
            jelly.SetBool("Run", true);
        }
        else {  
            jelly.SetBool("Run", false);
        }

        if (Input.GetKey(KeyCode.A)) {
            jelly.SetBool("RunLeft", true);
        }
        else {
            jelly.SetBool("RunLeft", false);
        }

        if (Input.GetKey(KeyCode.D)) {
            jelly.SetBool("RunRight", true);
        }
        else {
            jelly.SetBool("RunRight", false);
        }

        if (Input.GetKey(KeyCode.S)) {
            jelly.SetBool("RunBack", true);
        }
        else {
            jelly.SetBool("RunBack", false);
        }

        if (Input.GetMouseButtonDown(0) && Time.time > lastShotTime+fireDelay) {
            jelly.SetTrigger("Attack_Trigger");
            lastShotTime = Time.time;
            Invoke("Fire", fireDelay);
        }
        else {
            jelly.SetBool ("Attack", false);
        }

    }
   
    private void Fire() {
        Vector3 pos = fpsCamera.transform.position + fpsCamera.transform.TransformDirection (new Vector3 (0f, 0f, 1f)); 
        Quaternion rot = fpsCamera.transform.rotation;
        GameObject snowBall = Instantiate (snowBall_prefab, pos, rot);
        snowBall.tag = "player1";
        snowBall.GetComponent<Rigidbody>().velocity = snowBall.transform.forward * throw_velocity;
        Destroy (snowBall.gameObject, 10);
    }
    void OnGUI() {
        // 繪製準心紋理
        GUI.DrawTexture(new Rect(Screen.width / 2 - crosshairTexture.width / 2,
                                 Screen.height / 2 - crosshairTexture.height / 2,
                                 crosshairTexture.width, crosshairTexture.height),
                        crosshairTexture);
    }
}
