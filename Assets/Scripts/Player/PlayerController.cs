using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerController : NetworkBehaviour {

    private NetworkCharacterControllerPrototypeCustom networkCharacterController;

    private Camera myCamera;

    private float horizontalMovement = 0.0f;

    private float verticalMovement = 0.0f;

    [SerializeField]
    private SnowBall snowBallPrefab;

    [SerializeField]
    private Transform bulltSpawnTransform;

    [SerializeField]
    private Transform cameraPoint;

    [SerializeField]
    private Animator animator;

    [Networked]
    private NetworkButtons previousButtons {set; get;}
    
    void Awake() {
        networkCharacterController = GetComponent<NetworkCharacterControllerPrototypeCustom>();
        myCamera = GetComponentInChildren<Camera>();
        // animator = GetComponentInChildren<Animator>();
    }

    void Start() {
        // 隱藏鼠標並鎖定到屏幕中心
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void FixedUpdateNetwork() {
        if (GetInput (out NetworkInputData data)) {
            
            // 角色移動
            Vector3 moveVector = transform.right * data.movementInput.x + transform.forward * data.movementInput.y;
            moveVector.Normalize();
            networkCharacterController.Move (moveVector);
            
            // 依aimForwardVector旋轉角色
            transform.forward = data.aimForwardVector;
            
            Quaternion oldRotation = transform.rotation;
            
            // 取消角色rotationX
            Quaternion rotation = transform.rotation;
            rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.y, rotation.eulerAngles.z);
            transform.rotation = rotation;
            
            // Character animation
            // Let the animation look smooth
            horizontalMovement = Mathf.Lerp (horizontalMovement, data.movementInput.x, Runner.DeltaTime * 10);
            verticalMovement = Mathf.Lerp (verticalMovement, data.movementInput.y, Runner.DeltaTime * 10);
            animator.SetFloat ("horizontalMovement", horizontalMovement);
            animator.SetFloat ("verticalMovement", verticalMovement);

            // Buttons 資料
            NetworkButtons buttons = data.buttons;
            var pressed = buttons.GetPressed (previousButtons);
            previousButtons = buttons;

            if (pressed.IsSet (InputButtons.FIRE)) {
                
                // Ray ray = myCamera.ScreenPointToRay (new Vector2 (Screen.width / 2, Screen.height / 2));
                
                // Vector3 aimPosition = ray.origin + ray.direction * 100;

                // Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 0.5f);

                // Vector3 shootDirector = (aimPosition - bulltSpawnTransform.position).normalized;

                Runner.Spawn (snowBallPrefab,
                              cameraPoint.position,
                              oldRotation,
                              Object.InputAuthority
                );
            }
        }
    }
}


