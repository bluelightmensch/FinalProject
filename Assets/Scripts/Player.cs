using Mono.Cecil.Cil;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    private NewControls newControls;

    private void Awake(){
        newControls = new NewControls();
    }

    public void OnEnable(){
        newControls.Enable();
    }

    public void OnDisable(){
        newControls.Disable();
    }

    void Start(){

    }

    void Update()
    {
        Vector2 move = newControls.Movement.Move.ReadValue<Vector2>();

    }

}

