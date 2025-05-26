//// Author: Sadikur Rahman ////
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour {
    [SerializeField] private FixedJoystick joystick;
    private IPlayerMovement movement;
    Vector3 input;
    [SerializeField] private bool isTesting;

    private void Awake() 
    {
        movement = GetComponent<IPlayerMovement>();
    }
    private void Update()
    {
        if (isTesting)
        {
            var horizontal = Keyboard.current.leftArrowKey.isPressed ? -1 : 0;
            horizontal = Keyboard.current.rightArrowKey.isPressed ? 1 : horizontal;
            var vertical = Keyboard.current.upArrowKey.isPressed ? 1 : 0;
            vertical = Keyboard.current.downArrowKey.isPressed ? -1 : vertical;
            input = new Vector3(horizontal, 0, vertical);
        }
        else
        {
            input = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        }
        movement?.SetMoveInput(input);
    }
}
