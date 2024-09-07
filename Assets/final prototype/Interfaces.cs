using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollectible
{
    void Collect();
}

public interface IMovementBehavior
{
    void Initialize(Transform snakeTransform, float moveSpeed, float size);
    Vector3 GetNextDirection(Vector3 currentPosition, Vector3 currentDirection, List<Transform> segments);
}
