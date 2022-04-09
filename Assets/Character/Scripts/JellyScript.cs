using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* 
    射出雪球 (O)
    雪球與角色碰撞 (打到玩家雪球消失 玩家扣血) (O)
    角色移動攻擊 (O)
    發射預測線 
    背景音樂

    分割畫面
    攝影機
    時間結束畫面 (顯示勝利玩家)
    限制玩家行動區域 (周圍有隱形牆壁)
*/
public class JellyScript : MonoBehaviour
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
            jelly.SetFloat ("speed", speed);
            // 後退
            transform.Translate ((-1)*transform.forward * runSpeed * Time.deltaTime, Space.World);
        }
        else {
            jelly.SetBool("RunBack", false);
        }

        if (Input.GetKey(KeyCode.Space) && Time.time > lastShotTime+fireDelay) {
            // jelly.SetBool ("Attack", true);
            jelly.SetTrigger("Attack_Trigger");
            lastShotTime = Time.time;
            Invoke("Fire", 0.5f);
        }
        else {
            jelly.SetBool ("Attack", false);
        }
        if (Input.GetKey (KeyCode.Escape)) {
            Application.Quit();
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
        Vector3 pos = transform.position; pos.y += 1.3f;
        Quaternion rot = transform.rotation;
        GameObject snowBall = (GameObject)Instantiate (snowBall_prefab, pos, rot);
        snowBall.tag = "player1";
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
