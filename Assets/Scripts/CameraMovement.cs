using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    private InputAction movement;
    private InputAction verticalMovement;

    private void Start()
    {
        movement = new InputAction("PlayerMovement", binding: "<Gamepad>/leftStick");
        movement.AddCompositeBinding("Dpad")
            .With("Up", "<Keyboard>/w")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/s")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/a")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/d")
            .With("Right", "<Keyboard>/rightArrow");
        
        verticalMovement = new InputAction("VerticalMovement");
        verticalMovement.AddCompositeBinding("1DAxis")
            .With("Positive", "<Keyboard>/space")
            .With("Negative", "<Keyboard>/shift");

        verticalMovement.Enable();
        movement.Enable();
    }

    // Update is called once per frame
    private void Update()
    {
        float x;
        float z;

        var verticalMovementValue = verticalMovement.ReadValue<int>();

        if (verticalMovementValue > 0)
        {
            
        }  else if (verticalMovementValue < 0)
        {
            
        }

        var delta = movement.ReadValue<Vector2>();
        x = delta.x;
        z = delta.y;

        var move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);
    }
}
