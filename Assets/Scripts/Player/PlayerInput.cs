using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    private Vector2 moveInput = Vector2.zero;
    private Vector2 viewInput = Vector2.zero;
    private bool isShoot = false;

    private FirstPerosonCamera firstPerosonCamera;

    // Start is called before the first frame update
    void Awake() {
        firstPerosonCamera = GetComponentInChildren<FirstPerosonCamera>();
    }

    // Update is called once per frame
    void Update() {

        moveInput.x = Input.GetAxis ("Horizontal");
        moveInput.y = Input.GetAxis ("Vertical");

        viewInput.x = Input.GetAxis("Mouse X");
        viewInput.y = Input.GetAxis("Mouse Y") * -1;

        isShoot = Input.GetKey (KeyCode.Mouse0);

        // if cursor state is not None, then we want to rotate the camera
        if (Cursor.lockState != CursorLockMode.None) {
            firstPerosonCamera.SetMouseInputVector (viewInput);
        }
    }

    public NetworkInputData GetNetworkInput() {
        NetworkInputData networkInputData = new NetworkInputData();

        networkInputData.aimForwardVector = firstPerosonCamera.transform.forward;

        networkInputData.movementInput = moveInput;

        networkInputData.buttons.Set (InputButtons.FIRE, isShoot);

        // Reset isShoot state
        isShoot = false;

        return networkInputData;
    }
}
