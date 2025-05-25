//// Author: Sadikur Rahman ////
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour {
    [SerializeField] private FixedJoystick joystick;
    private IPlayerMovement movement;

    private void Awake() {
        movement = GetComponent<IPlayerMovement>();
    }

    private void Update() {
        Vector3 input = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        movement?.SetMoveInput(input);
    }
}
