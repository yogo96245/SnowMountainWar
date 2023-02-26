using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    public float runSpeed;
    public float turnSmooth;
    public GameObject snowBall_prefab;
    public float throw_velocity;
    // public GameObject snowBall_spawn;
    private float  speed;
    private float turn; 
    private float lastShotTime;
    private float fireDelay;
    //public float jumpForce;
    Animator jelly;
    Rigidbody rigid;
    Vector3 targetDir;
    Vector3 jumpSpeed;
    Quaternion targetRotate;
    Quaternion smoothRotate;
    // Start is called before the first frame update
    void Start() {
        jelly = GetComponent<Animator>();
        rigid  = GetComponent<Rigidbody>();
        speed = 0.0f;
        lastShotTime = 0.0f;
        fireDelay = 0.5f;
    }

    // Update is called once per frame
    void Update() {
        speed = Input.GetAxis ("Vertical_" + jelly.tag);
        turn  = Input.GetAxis ("Horizontal_" + jelly.tag);

        if (Input.GetKey(KeyCode.UpArrow)) {
            jelly.SetBool("Run", true);
        }
        else {  
            jelly.SetBool("Run", false);
        }

        if (Input.GetKey(KeyCode.LeftArrow)) {
            jelly.SetBool("RunLeft", true);
        }
        else {
            jelly.SetBool("RunLeft", false);
        }

        if (Input.GetKey(KeyCode.RightArrow)) {
            jelly.SetBool("RunRight", true);
        }
        else {
            jelly.SetBool("RunRight", false);
        }

        if (Input.GetKey(KeyCode.DownArrow)) {
            jelly.SetBool("RunBack", true);
            jelly.SetFloat ("speed", speed);
            // 後退
            transform.Translate ((-1)*transform.forward * runSpeed * Time.deltaTime, Space.World);
        }
        else {
            jelly.SetBool("RunBack", false);
        }

        if (Input.GetKey(KeyCode.Keypad0) && Time.time > lastShotTime+fireDelay) {
            // jelly.SetBool ("Attack", true);
            jelly.SetTrigger("Attack_Trigger");
            lastShotTime = Time.time;
            Invoke("Fire", 0.5f);
        }
        else {
            jelly.SetBool ("Attack", false);
        }
    }
    // nuye_rigid.AddForce ();
    private void FixedUpdate() {
        if (turn != 0) {
            RotateControl (Math.Abs(speed), turn);
        }
        if (speed > 0.5) {  
            jelly.SetFloat ("speed", speed);
            // 前進
            transform.Translate (transform.forward * runSpeed * Time.deltaTime, Space.World);
            // rigid.velocity = transform.forward * runSpeed;
        }
        else {
            jelly.SetFloat("speed", 0.0f);
            // rigid.velocity = new Vector3 (0.0f, rigid.velocity.y, 0.0f);  
        }
    }
    private void Fire() {
        /*
        float jellyPos_x = jelly.transform.position.x;
        float jellyPos_z = jelly.transform.position.z;
        int jellyRotate_y = (int)jelly.transform.localRotation.eulerAngles.y;
        float shot_x = 1.0f;
        float shot_y = 1.3f;
        float shot_z = 1.0f;
        Debug.Log (jellyRotate_y);
        if (jellyRotate_y >  180 && jellyRotate_y <  360) shot_x *= -1.0f;
        if (jellyRotate_y >   90 && jellyRotate_y <  270) shot_z *= -1.0f;
        if (jellyRotate_y == 180 || jellyRotate_y ==   0) shot_x = 0.0f;
        if (jellyRotate_y ==  90 || jellyRotate_y == 270) shot_z = 0.0f;

        Debug.Log ("shut " + shot_x + " " + shot_z);
        // Debug.Log (jellyPos_x + " " + jellyPos_y + " " + jellyPos_z);

        snowBall_spawn.transform.position = new Vector3 (jellyPos_x+shot_x, shot_y, jellyPos_z+shot_z);
        snowBall_spawn.transform.rotation = jelly.transform.rotation;*/
        Vector3 pos = transform.position; pos.y += 1.3f;
        Quaternion rot = transform.rotation;
        GameObject snowBall = (GameObject)Instantiate (snowBall_prefab, pos, rot);
        snowBall.tag = "player2";
        snowBall.GetComponent<Rigidbody>().velocity = snowBall.transform.forward * throw_velocity;
        Destroy (snowBall.gameObject, 10);
    }

    private void RotateControl (float v, float h) {
        //targetDir = new Vector3 (h, 0.0f, v);
        targetDir = transform.TransformDirection (new Vector3 (h, 0.0f, v));
        targetRotate = Quaternion.LookRotation (targetDir, Vector3.up);
        // 急速轉向 transform.rotation = targetRotate;
        // 漸進轉向
        smoothRotate = Quaternion.Slerp (transform.rotation, targetRotate, turnSmooth * Time.deltaTime);
        transform.rotation  = smoothRotate;
    }  
}
