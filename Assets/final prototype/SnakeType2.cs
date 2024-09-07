using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeType2 : MonoBehaviour
{
    public float moveSpeed;
    public GameObject tailPrefab;

    private Vector3 direction = Vector3.right; // Начальное направление движения
    private Vector3 nextDirection;
    private float size;

    private IMovementBehavior movementBehavior; // Интерфейс движения
    private float MovedDistance;
    private Vector3 StartPosition;

    private Rigidbody2D rb; // Ссылка на Rigidbody2D

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Получаем размер объекта для расчета шага движения
        Renderer renderer = GetComponent<Renderer>();
        size = renderer.bounds.size.x;
        size = 1;

        // Инициализируем начальное поведение движения
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
        // Перемещение головы
        float step = moveSpeed * Time.deltaTime;
        MovedDistance += step;
        transform.Translate(step * direction);

        // Проверяем, прошла ли голова расстояние, достаточное для добавления нового сегмента
        if (MovedDistance >= size)
        {
            // Обновляем позицию головы
            transform.position = StartPosition + direction * size;
            StartPosition = transform.position;

            // Добавляем новый сегмент змеи
            Instantiate(tailPrefab, StartPosition, Quaternion.identity);
            MovedDistance = 0;
            // Получаем следующее направление от стратегии движения
            nextDirection = movementBehavior.GetNextDirection(transform.position, direction, null);
            direction = nextDirection;
        }
    }
}
