using UnityEngine;

public class WayPanel : Panel
{
    private bool keepLit = false;

    protected override void OnPlayerStepOn()
    {
        Debug.Log($"{name} - WayPanel OnPlayerStepOn()");
        
        // Notify the SequenceManager which panel was stepped on
        WayPanelSequenceManager.instance.OnPanelSteppedOn(this);
    }

    protected override void OnPlayerStepOff()
    {
        // Only revert to 0 if NOT keepLit
        if (!keepLit)
        {
            Debug.Log($"{name} - WayPanel is not keepLit, fading to 0");
            StartFadeEmission(0f);
        }
        else
        {
            Debug.Log($"{name} - WayPanel is keepLit, staying at emission=1");
        }
    }

    public void SetKeepLit(bool value)
    {
        keepLit = value;
        if (keepLit)
        {
            // Immediately fade to 1
            StartFadeEmission(1f);
        }
        else
        {
            // Fade (or jump) to 0
            StartFadeEmission(0f);
        }
    }

    /// <summary>
    /// Called by the manager to restore default emission if the sequence resets.
    /// </summary>
    public void ResetToDefaultEmission()
    {
        keepLit = false;
        // The "default" for WayPanel might be 0, or you could do base logic
        StartFadeEmission(0f);
    }
}
