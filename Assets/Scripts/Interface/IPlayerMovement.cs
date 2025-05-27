using UnityEngine;

public interface IMovable {
    void SetMoveInput(Vector3 input);
    void Move();
    void Rotate();
}

public interface IJumpable {
    void Jump();
}

public interface IDashable {
    void Dash();
}