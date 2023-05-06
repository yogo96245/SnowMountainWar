using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPerosonCamera : MonoBehaviour {
    
    public Transform cameraPoint;
    private Vector2 viewInput;
    private Camera localCamera;
    private NetworkCharacterControllerPrototypeCustom networkCharacterController;

    public float mouseSensitivity = 100f;
    // camera垂直旋轉
    private float canmeraRotationX = 0f;
    // camera水平旋轉
    private float canmeraRotationY = 0f;

    void Awake() {
        localCamera = GetComponent<Camera>();
        networkCharacterController = GetComponentInParent<NetworkCharacterControllerPrototypeCustom>();
    }

    void Start() {
        if (localCamera.enabled) {
            // 分割Camera
            localCamera.transform.parent = null;
        }
        // 隱藏鼠標並鎖定到屏幕中心
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
        if (Input.GetKey(KeyCode.Escape)) {
            Cursor.lockState = CursorLockMode.None;
        }
        if (Input.GetKey (KeyCode.Mouse0)) {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Update is called once per frame
    void LateUpdate() {

        if (!localCamera.enabled) {
            return;
        }

        if (cameraPoint == null) {
            return;
        }

        // Camera follow player
        localCamera.transform.position = cameraPoint.position;

         // 計算camera旋轉角度
        canmeraRotationX += viewInput.y * Time.deltaTime * networkCharacterController.rotationSpeed;
        canmeraRotationY += viewInput.x * Time.deltaTime * networkCharacterController.rotationSpeed;

        // 限制上下旋轉角度
        canmeraRotationX = Mathf.Clamp(canmeraRotationX, -90f, 90f);

        // 上下左右旋轉
        localCamera.transform.rotation = Quaternion.Euler(canmeraRotationX, canmeraRotationY, 0);
        
    }
    
    public void SetMouseInputVector (Vector2 viewInput) {
        this.viewInput = viewInput;
    }

}
