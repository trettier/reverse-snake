using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeType2 : MonoBehaviour
{
    public float moveSpeed;
    public GameObject tailPrefab;

    private Vector3 direction = Vector3.right; // ��������� ����������� ��������
    private Vector3 nextDirection;
    private float size;

    private IMovementBehavior movementBehavior; // ��������� ��������
    private float MovedDistance;
    private Vector3 StartPosition;

    private Rigidbody2D rb; // ������ �� Rigidbody2D

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // �������� ������ ������� ��� ������� ���� ��������
        Renderer renderer = GetComponent<Renderer>();
        size = renderer.bounds.size.x;
        size = 1;

        // �������������� ��������� ��������� ��������
        movementBehavior = new PatrolMovement();
        movementBehavior.Initialize(transform, moveSpeed, size);

        MovedDistance = 0;
        StartPosition = transform.position;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        // ����������� ������
        float step = moveSpeed * Time.deltaTime;
        MovedDistance += step;
        transform.Translate(step * direction);

        // ���������, ������ �� ������ ����������, ����������� ��� ���������� ������ ��������
        if (MovedDistance >= size)
        {
            // ��������� ������� ������
            transform.position = StartPosition + direction * size;
            StartPosition = transform.position;

            // ��������� ����� ������� ����
            Instantiate(tailPrefab, StartPosition, Quaternion.identity);
            MovedDistance = 0;
            // �������� ��������� ����������� �� ��������� ��������
            nextDirection = movementBehavior.GetNextDirection(transform.position, direction, null);
            direction = nextDirection;
        }
    }
}
