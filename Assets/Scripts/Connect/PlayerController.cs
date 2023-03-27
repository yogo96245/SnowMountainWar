using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerController : NetworkBehaviour {

    private NetworkCharacterControllerPrototype networkCharacterController;
    private Animator animator;
    private Camera myCamera;
    private float moveSpeed = 5f;
    private float cameraRotationX = 0f;
    
    void Awake() {
        networkCharacterController = GetComponent<NetworkCharacterControllerPrototype>();
        myCamera = GetComponentInChildren<Camera>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update() {
        
    }

    public override void FixedUpdateNetwork() {
        if (GetInput (out NetworkInputData data)) {

            Vector3 moveVector = transform.right * data.movementInput.x + transform.forward * data.movementInput.z;
            moveVector.Normalize();
            networkCharacterController.Move (moveVector * moveSpeed * Runner.DeltaTime);
            
            // if (moveVector.z > 0f) {
            //     animator.SetBool("Run", true);
            // }     

            // Camera 垂直旋轉
            cameraRotationX += data.rotateInput.x * networkCharacterController.rotationSpeed;
            cameraRotationX = Mathf.Clamp(cameraRotationX, -90f, 90f);
            myCamera.transform.localRotation = Quaternion.Euler (cameraRotationX , 0f, 0f);

            // 水平旋轉角色
            networkCharacterController.Rotate (data.rotateInput.y);
        }
    }
}


