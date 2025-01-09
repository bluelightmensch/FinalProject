using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class WayPanelSequenceManager : MonoBehaviour
{
    // Singleton so WayPanels can call WayPanelSequenceManager.instance
    public static WayPanelSequenceManager instance;

    // Drag your WayPanels into this list in the desired stepping order
    public List<WayPanel> panelsInOrder;

    // The index of the next WayPanel the player must step on
    private int nextPanelIndex = 0;

    // Reference to the wall cube that will descend
    [Header("Wall Cube Descent")]
    public Transform wallCube;          // Assign in Inspector
    public TextMeshProUGUI gameOverText;
    [SerializeField] private float targetY = 0f;          // Where the wallCube should descend to
    [SerializeField] private float descendSpeed = 1f;     // How fast it moves down

    // For detecting Game Over (player falls off edge)
    [Header("Player & Game Over")]
    public Transform player;            // Assign the player's transform here
    [SerializeField] private Material material; 
    [SerializeField] private float fallThresholdY = -20f; // If player < -20, game over
    private bool hasGameOverTriggered = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        // Optional: Hide it on start if it's currently active
        if (gameOverText != null)
            gameOverText.gameObject.SetActive(false);
        material.SetInt( "_Bool", 0);
    }

    private void Start(){
        gameOverText.enabled = true;
    }

    private void Update()
    {
        // Continuously check if the player has fallen below the threshold
        if (!hasGameOverTriggered && player != null && player.position.y < fallThresholdY)
        {
            hasGameOverTriggered = true;
            TriggerGameOver();
        }
    }

    /// <summary>
    /// Called by each WayPanel when the Player steps on it
    /// to check if it matches the correct order.
    /// </summary>
    public void OnPanelSteppedOn(WayPanel panel)
    {
        // If we've already gone through all panels, do nothing
        if (nextPanelIndex >= panelsInOrder.Count) return;

        WayPanel correctPanel = panelsInOrder[nextPanelIndex];
        if (panel == correctPanel)
        {
            // Correct panel stepped on
            panel.SetKeepLit(true);
            
            nextPanelIndex++;

            // If we've reached the end of the sequence:
            if (nextPanelIndex == panelsInOrder.Count)
            {
                // Start descending the wall cube
                StartCoroutine(DescendWallCube());
            }
        }
        else
        {
            // Stepped on the wrong panel -> reset sequence
            ResetSequence();
        }
    }

    /// <summary>
    /// Resets the entire sequence: turn off all WayPanels and start from zero.
    /// </summary>
    public void ResetSequence()
    {
        nextPanelIndex = 0;
        foreach (WayPanel wp in panelsInOrder)
        {
            wp.SetKeepLit(false);
            wp.ResetToDefaultEmission();
        }
    }

    /// <summary>
    /// Coroutine to move the wallCube from its current Y down to targetY.
    /// </summary>
    private IEnumerator DescendWallCube()
    {
        if (wallCube == null) yield break;

        // Move the wallCube down over time
        Vector3 startPos = wallCube.position;
        Vector3 targetPos = new Vector3(startPos.x, targetY, startPos.z);

        // We'll do a simple linear move 
        while (wallCube.position.y > targetY)
        {
            float step = descendSpeed * Time.deltaTime;
            wallCube.position = Vector3.MoveTowards(wallCube.position, targetPos, step);
            yield return null;
        }

    }

    /// <summary>
    /// Handle the Game Over event. Could load a screen, scene, UI, etc.
    /// </summary>
    private void TriggerGameOver()
    {
        if (gameOverText != null)
            gameOverText.gameObject.SetActive(true);
            material.SetInt( "_Bool", 1);
        // Example approach: If you have a UI Manager, you could do:
        // UIManager.instance.ShowGameOverScreen();

        // Or if you want to reload a scene:
        // SceneManager.LoadScene("GameOverScene");

        // For now, just log and freeze time:
        // Time.timeScale = 0f;
    }
}
