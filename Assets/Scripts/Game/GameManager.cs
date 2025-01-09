using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public bool itemDestroyed = false;

    private void Awake()
    {
        // Basic singleton pattern
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
}