using UnityEngine;
using System.Collections;

public class Panel : MonoBehaviour
{
    // Tracks the current emission value on this panel
    private float currentEmission = 0f;

    // We’ll store a reference to the active fade Coroutine
    private Coroutine fadeRoutine;

    [SerializeField] private float fadeDuration = 1f;

    private void OnCollisionEnter(Collision collision)
    {
        // Only respond if Player and the special item was destroyed
        if (collision.gameObject.CompareTag("Player") && GameManager.instance.itemDestroyed)
        {
            OnPlayerStepOn();
            Debug.Log("Player stepped on.");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // If Player leaves, fade back
        if (collision.gameObject.CompareTag("Player"))
        {
            OnPlayerStepOff();
        }
    }

    /// <summary>
    /// Called when the Player steps ON the panel.
    /// Base Panel: fade emission down to -1.
    /// </summary>
    protected virtual void OnPlayerStepOn()
    {
        StartFadeEmission(-1f);
    }

    /// <summary>
    /// Called when the Player steps OFF the panel.
    /// Base Panel: fade emission up to 0.
    /// </summary>
    protected virtual void OnPlayerStepOff()
    {
        StartFadeEmission(0f);
    }

    /// <summary>
    /// Helper method to start a coroutine that gradually fades our emission to a target value.
    /// </summary>
    protected void StartFadeEmission(float targetValue)
    {
        // If a fade is already in progress, stop it
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }
        // Begin a new fade
        fadeRoutine = StartCoroutine(FadeEmission(targetValue, fadeDuration));
    }

    /// <summary>
    /// Coroutine that smoothly transitions currentEmission to targetValue over 'duration' seconds.
    /// </summary>
    private IEnumerator FadeEmission(float targetValue, float duration)
    {
        float startValue = currentEmission;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            // Lerp between start and target
            currentEmission = Mathf.Lerp(startValue, targetValue, t);

            // Apply to the panel’s material
            SetEmission(currentEmission);

            yield return null;
        }

        // Ensure we end exactly at the target
        currentEmission = targetValue;
        SetEmission(currentEmission);

        fadeRoutine = null;
    }

    /// <summary>
    /// Updates the _EmissionStrength on this GameObject's Renderer
    /// using MaterialPropertyBlock.
    /// </summary>
    private void SetEmission(float value)
    {
        Renderer rend = GetComponent<Renderer>();
        if (!rend) return;

        MaterialPropertyBlock block = new MaterialPropertyBlock();
        rend.GetPropertyBlock(block);

        // Match this to your Shader Graph property name
        block.SetFloat("_EmissionChange", value);

        rend.SetPropertyBlock(block);
    }
}
