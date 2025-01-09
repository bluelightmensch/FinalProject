using UnityEngine;

public class ElevatorSensor : MonoBehaviour
{
    public Elevator elevator; // Reference to the Elevator script

    private void OnTriggerEnter(Collider other)
    {
        // Stop the elevator when it reaches this sensor
        if (other.CompareTag("Elevator"))
        {
            if (gameObject.name == "TopSensor")
            {
                elevator.StopMovingUp();
            }
            else if (gameObject.name == "BottomSensor")
            {
                elevator.StopMovingDown();
            }
        }
        else if (other.CompareTag("Player"))
        {
            Debug.Log("Player has activated the sensor.");

            // Determine if the elevator needs to move
            if (gameObject.name == "TopSensor")
            {
                if (!elevator.IsAtTop()) // If the elevator is not at the top, move it up
                {
                    Debug.Log("Moving elevator to the top.");
                    elevator.StartMovingUp();
                }
                else
                {
                    Debug.Log("Elevator is already at the top.");
                }
            }
            else if (gameObject.name == "BottomSensor")
            {
                if (!elevator.IsAtBottom()) // If the elevator is not at the bottom, move it down
                {
                    Debug.Log("Moving elevator to the bottom.");
                    elevator.StartMovingDown();
                }
                else
                {
                    Debug.Log("Elevator is already at the bottom.");
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Optionally allow continuous interaction while in the trigger area
        if (other.CompareTag("Player") && Input.GetButtonDown("Submit")) // "Submit" maps to the A button
        {
            if (gameObject.name == "TopSensor")
            {
                if (!elevator.IsAtTop())
                {
                    Debug.Log("Calling elevator to the top.");
                    elevator.StartMovingUp();
                }
            }
            else if (gameObject.name == "BottomSensor")
            {
                if (!elevator.IsAtBottom())
                {
                    Debug.Log("Calling elevator to the bottom.");
                    elevator.StartMovingDown();
                }
            }
        }
    }
}
