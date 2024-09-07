using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolMovement2 : IMovementBehavior2
{
    private Transform snakeTransform;
    private List<Vector3> path;
    private int counter;

    public void Initialize(Transform snakeTransform, List<Vector3> path)
    {
        this.snakeTransform = snakeTransform;
        this.path = path;
        this.counter = 0;
    }

    public Vector3 GetNextDirection(Vector3 currentPosition, Vector3 currentDirection)
    {
        // ѕроверка совпадени€ текущей позиции и точки патрулировани€
        if (Vector3.Distance(this.path[this.counter], currentPosition) < 0.1f) // »зменено на использование рассто€ни€
        {
            // ќбновл€ем счетчик дл€ перехода к следующей точке
            counter = (counter + 1) % this.path.Count;

            // ¬ычисл€ем новое направление движени€
            Vector3 newDirection = (this.path[counter] - currentPosition).normalized;
            return newDirection;
        }
        return currentDirection;
    }
}
