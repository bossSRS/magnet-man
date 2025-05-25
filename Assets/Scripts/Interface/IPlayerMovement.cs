//// Author: Sadikur Rahman ////

using UnityEngine;

public interface IPlayerMovement {
    void SetMoveInput(Vector3 input);
    void Move();
    void Rotate();
    void Jump();
    void Dash();
    void ResetPosition(Vector3 pos);
}