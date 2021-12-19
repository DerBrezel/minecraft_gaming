using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Avatar : MonoBehaviour {
    private PlayerControls playerControls;
    private InputAction movement;
    private CharacterController myCC;

    public float moveSpeed = 2;
    [SerializeField]
    private float jumpHeight;
    [SerializeField]
    private float gravity;
    private Vector2 moveInput;
    private Vector2 mouseInput;
    private bool grounded;
    [SerializeField]
    MouseLook mouseLook;



    private void Awake() {
        playerControls = new PlayerControls();
        myCC = GetComponent<CharacterController>();

        playerControls.Player.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
        playerControls.Player.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();
    }

    private void OnEnable() {
        movement = playerControls.Player.Movement;
        movement.Enable();

        playerControls.Player.Jump.performed += DoJump;
        playerControls.Player.Jump.Enable();
    }

    private void DoJump(InputAction.CallbackContext obj) {
        Debug.Log("Jump!");
    }

    private void OnDisable() {
        movement.Disable();
        playerControls.Player.Jump.Disable();
    }

    private void FixedUpdate() {
        Debug.Log("Movement Values " + movement.ReadValue<Vector2>());
    }

    private void Update() {
        grounded = myCC.isGrounded;

        if (grounded && moveInput.y < 0) {
            moveInput.y = 0f;
        }

        Vector2 input = movement.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        myCC.Move(move * Time.deltaTime * moveSpeed);

        //jump
        if (grounded && playerControls.Player.Jump.triggered) {
            moveInput.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }

        moveInput.y += gravity * Time.deltaTime;
        myCC.Move(moveInput * Time.deltaTime);

        mouseLook.ReceiveInput(mouseInput);

    }
}
