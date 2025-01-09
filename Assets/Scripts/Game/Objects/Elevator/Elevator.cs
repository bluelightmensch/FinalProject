using UnityEngine;

public class Elevator : MonoBehaviour
{
    public float speed = 2f; // Speed of the elevator
    public Transform topPoint; // Top position of the elevator
    public Transform bottomPoint; // Bottom position of the elevator

    private bool movingUp = false;
    private bool movingDown = false;
    private bool isAtTop = false;
    private bool isAtBottom = true;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate; // Smooth visual movement
    }

    void FixedUpdate()
    {
        Vector3 targetPosition = transform.position;

        // Move the elevator up
        if (movingUp && transform.position.y < topPoint.position.y)
        {
            targetPosition += Vector3.up * speed * Time.fixedDeltaTime;
            targetPosition.y = Mathf.Min(targetPosition.y, topPoint.position.y); // Clamp to top point
        }
        // Move the elevator down
        else if (movingDown && transform.position.y > bottomPoint.position.y)
        {
            targetPosition += Vector3.down * speed * Time.fixedDeltaTime;
            targetPosition.y = Mathf.Max(targetPosition.y, bottomPoint.position.y); // Clamp to bottom point
        }

        // Use MovePosition for smooth physics-driven movement
        rb.MovePosition(Vector3.Lerp(transform.position, targetPosition, 0.5f)); // Smooth interpolation
    }

    public void StartMovingUp()
    {
        if (!isAtTop)
        {
            movingUp = true;
            movingDown = false;
        }
    }

    public void StartMovingDown()
    {
        if (!isAtBottom)
        {
            movingDown = true;
            movingUp = false;
        }
    }

    public void StopMovingUp()
    {
        movingUp = false;
        isAtTop = true;
        isAtBottom = false;
    }

    public void StopMovingDown()
    {
        movingDown = false;
        isAtBottom = true;
        isAtTop = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetButtonDown("Submit")) // "Submit" maps to A on Xbox
        {
            ToggleMovement();
        }
    }

    public void ToggleMovement()
    {
        if (isAtBottom)
        {
            StartMovingUp();
        }
        else if (isAtTop)
        {
            StartMovingDown();
        }
    }

        public bool IsAtTop()
    {
        return isAtTop; // Returns true if the elevator is at the top
    }

    public bool IsAtBottom()
    {
        return isAtBottom; // Returns true if the elevator is at the bottom
    }

    private void OnTriggerEnter(Collider other)
    {
        // Log a message when the player enters the trigger zone
        if (other.CompareTag("Player"))
        {
            LogElevatorMessage(); // Call the new method to display the message
        }
    }

    public void LogElevatorMessage()
    {
        Debug.Log("Press A to move elevator.");
    }

}
