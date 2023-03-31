using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPerosonCamera : MonoBehaviour {
    
    public GameObject player;
    public float mouseSensitivity = 100f;
    private Vector3 offset;
    private Transform playerTransform;
    // camera垂直旋轉
    private float xRotation = 0f;
    // camera水平旋轉
    private float yRotation = 0f;

    void Start() {
        playerTransform = player.transform;
        offset = new Vector3 (0f, 2f, 1f);
        transform.position = playerTransform.position + (playerTransform.TransformDirection (offset));

        // 隱藏鼠標並鎖定到屏幕中心
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update() {
        // camera follow player
        transform.position = playerTransform.position + (playerTransform.TransformDirection (offset));

        // 獲取鼠標輸入
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

         // 計算camera旋轉角度
        xRotation -= mouseY;
        yRotation += mouseX;

        // 限制上下旋轉角度
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);    

        // 上下左右旋轉
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        
        // 角色隨camera左右旋轉
        playerTransform.localRotation = Quaternion.Euler(0f, yRotation, 0f);
    }


}
