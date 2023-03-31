using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerController : NetworkBehaviour {

    private NetworkCharacterControllerPrototypeCustom networkCharacterController;
    private Animator animator;
    private Camera myCamera;
    private float cameraRotationX = 0f;
    private Vector3 offset = new Vector3 (0f, 2f, 1f);
    
    void Awake() {
        networkCharacterController = GetComponent<NetworkCharacterControllerPrototypeCustom>();
        myCamera = GetComponentInChildren<Camera>();
        animator = GetComponentInChildren<Animator>();
    }

    void Start() {
        // 隱藏鼠標並鎖定到屏幕中心
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
        // camera follow player
        myCamera.transform.position = transform.position + (transform.TransformDirection (offset));
    }

    public override void FixedUpdateNetwork() {
        if (GetInput (out NetworkInputData data)) {

            Vector3 moveVector = transform.right * data.movementInput.x + transform.forward * data.movementInput.z;
            moveVector.Normalize();
            networkCharacterController.Move (moveVector);   

            // Camera 垂直旋轉
            cameraRotationX += data.rotateInput.x * networkCharacterController.rotationSpeed;
            cameraRotationX = Mathf.Clamp(cameraRotationX, -90f, 90f);
            myCamera.transform.localRotation = Quaternion.Euler (cameraRotationX , 0f, 0f);

            // 水平旋轉角色
            networkCharacterController.Rotate (data.rotateInput.y);
        }
    }
}


