using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEditor.Experimental.GraphView;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    private Rigidbody rb; // Reference to the Rigidbody
    private NewControls newControls;

    private Vector3 movementDirection;
    private Transform cameraTransform;
    private Vector3 startPos;

    private void Awake(){
        newControls = new NewControls();
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; // Prevent tilting
    }


    void Start()
    {
        cameraTransform = Camera.main.transform; // Assumes the main camera is the FreeLook camera
        startPos = new Vector3(0,2,0);
        transform.position = startPos;
    }

    public void OnEnable(){
        newControls.Enable();
    }

    public void OnDisable(){
        newControls.Disable();
    }

    void Update()
    {
        HandleMovementInput();
    }

    void FixedUpdate()
    {
        MovePlayer();
        Respawn();
    }
    private void HandleMovementInput()
    {
        Vector2 move = newControls.Movement.Move.ReadValue<Vector2>();

        float horizontal = move.x;
        float vertical = move.y;

        // 2. Get camera forward and right directions (ignoring camera's vertical component)
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0f; 
        cameraForward.Normalize();

        Vector3 cameraRight = cameraTransform.right;
        cameraRight.y = 0f; 
        cameraRight.Normalize();

        // 3. Combine input with camera direction to determine movement direction
        movementDirection = (cameraForward * vertical + cameraRight * horizontal).normalized;
    }

    private void MovePlayer()
    {
        // Move the player using Rigidbody.MovePosition
        if (movementDirection.magnitude > 0.1f)
        {
            Vector3 targetPosition = rb.position + movementDirection * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(targetPosition);

            // Rotate the player to face the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    void Respawn(){
        if (transform.position.y < -20f){
            transform.position = startPos;
        }
    }

}
