using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IMovementBehavior2
{
    void Initialize(Transform snakeTransform, List<Vector3> path);
    Vector3 GetNextDirection(Vector3 currentPosition, Vector3 currentDirection);
}

