using System.Collections.Generic;
using UnityEngine;

public class PatrolMovement : IMovementBehavior
{
    private Transform snakeTransform;
    private float moveSpeed;
    private float size;
    private Rigidbody2D rb; // Ссылка на Rigidbody2D

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

        // Проверяем каждое возможное направление
        foreach (Vector3 direction in possibleDirections)
        {
            if (direction == -currentDirection) // Пропускаем противоположное направление
                continue;
            // Используем Rigidbody2D.Cast для проверки наличия препятствия
            if (!IsObstacleInDirection(direction))
            {
                validDirections.Add(direction);
            }
        }

        // Если нет доступных направлений, возвращаем текущее направление
        if (validDirections.Count == 0)
            return Vector3.zero;

        // Возвращаем случайное допустимое направление
        return validDirections[Random.Range(0, validDirections.Count)];
    }

    private bool IsObstacleInDirection(Vector3 direction)
    {
        // Преобразуем направление в Vector2 для использования с Rigidbody2D.Cast
        Vector2 castDirection = new Vector2(direction.x, direction.y);

        // Создаем фильтр для проверки коллизий (например, проверка всех слоев)
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(Physics2D.AllLayers); // Проверка всех слоев
        contactFilter.useTriggers = false; // Игнорировать триггеры

        // Массив для хранения результатов кастинга
        RaycastHit2D[] hitResults = new RaycastHit2D[1];

        // Вычисляем расстояние для кастинга
        float distance = size; // В данном случае расстояние равно размеру объекта

        // Выполняем кастинг
        int hitCount = rb.Cast(castDirection, contactFilter, hitResults, distance);
        Debug.Log(hitCount);

        // Возвращаем true, если обнаружено препятствие (hitCount > 0)
        return hitCount > 0;
    }
}
