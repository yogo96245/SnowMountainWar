using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerController : NetworkBehaviour {

    private float horizontalMovement = 0.0f;

    private float verticalMovement = 0.0f;

    [SerializeField]
    private  float animationTransitionTime = 0.1f;

    private NetworkCharacterControllerPrototypeCustom networkCharacterController;

    [Networked]
    private NetworkButtons previousButtons {set; get;}

    [SerializeField]
    private Transform bulltSpawnTransform;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private SnowBall snowBallPrefab;

    [SerializeField]
    private GameObject muzzle;

    void Awake() {
        networkCharacterController = GetComponent<NetworkCharacterControllerPrototypeCustom>();
        // myCamera = GetComponentInChildren<Camera>();
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
            
            // Character movement animation with smooth transition
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

                // Shoot animation
                
                StartCoroutine(Fire(oldRotation));
                
            }
        }
    }

    private IEnumerator Fire(Quaternion oldRotation) {
        // Prevent firing while firing
        if (animator.GetBool ("hasFiring")) {
            yield break;
        }
        animator.Play ("Fire", 0, animationTransitionTime);
        animator.SetBool ("hasFiring", true);
        yield return new WaitForSeconds(0.3f);
        animator.SetBool ("hasFiring", false);
        Runner.Spawn (snowBallPrefab,
                      muzzle.transform.position,
                      oldRotation,
                      Object.InputAuthority
        );
    }
}


