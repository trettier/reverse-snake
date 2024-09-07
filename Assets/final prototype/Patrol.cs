using System.Collections.Generic;
using UnityEngine;

public class PatrolMovement : IMovementBehavior
{
    private Transform snakeTransform;
    private float moveSpeed;
    private float size;
    private Rigidbody2D rb; // ������ �� Rigidbody2D

    private static readonly Vector3[] possibleDirections = new Vector3[]
    {
        Vector3.up,
        Vector3.down,
        Vector3.left,
        Vector3.right
    };

    public void Initialize(Transform snakeTransform, float moveSpeed, float size)
    {
        this.snakeTransform = snakeTransform;
        this.moveSpeed = moveSpeed;
        this.size = size;
        this.rb = snakeTransform.GetComponent<Rigidbody2D>();
    }

    public Vector3 GetNextDirection(Vector3 currentPosition, Vector3 currentDirection, List<Transform> segments)
    {
        Vector3 newDirection = GetRandomDirection(currentDirection);

        return newDirection;
    }

    private Vector3 GetRandomDirection(Vector3 currentDirection)
    {
        List<Vector3> validDirections = new List<Vector3>();

        // ��������� ������ ��������� �����������
        foreach (Vector3 direction in possibleDirections)
        {
            if (direction == -currentDirection) // ���������� ��������������� �����������
                continue;
            // ���������� Rigidbody2D.Cast ��� �������� ������� �����������
            if (!IsObstacleInDirection(direction))
            {
                validDirections.Add(direction);
            }
        }

        // ���� ��� ��������� �����������, ���������� ������� �����������
        if (validDirections.Count == 0)
            return Vector3.zero;

        // ���������� ��������� ���������� �����������
        return validDirections[Random.Range(0, validDirections.Count)];
    }

    private bool IsObstacleInDirection(Vector3 direction)
    {
        // ����������� ����������� � Vector2 ��� ������������� � Rigidbody2D.Cast
        Vector2 castDirection = new Vector2(direction.x, direction.y);

        // ������� ������ ��� �������� �������� (��������, �������� ���� �����)
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(Physics2D.AllLayers); // �������� ���� �����
        contactFilter.useTriggers = false; // ������������ ��������

        // ������ ��� �������� ����������� ��������
        RaycastHit2D[] hitResults = new RaycastHit2D[1];

        // ��������� ���������� ��� ��������
        float distance = size; // � ������ ������ ���������� ����� ������� �������

        // ��������� �������
        int hitCount = rb.Cast(castDirection, contactFilter, hitResults, distance);
        Debug.Log(hitCount);

        // ���������� true, ���� ���������� ����������� (hitCount > 0)
        return hitCount > 0;
    }
}
