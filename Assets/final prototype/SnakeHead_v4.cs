using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class SnakeHead_v4 : MonoBehaviour
{
    public float moveSpeed;
    public GameObject tailPrefab;
    public int initialSegments = 3;

    private Vector3 direction = Vector3.zero;
    private bool addTail = false;
    private float size;

    private List<SnakeSegment> segments = new List<SnakeSegment>();
    private IMovementBehavior movementBehavior; // Интерфейс движения

    private class SnakeSegment
    {
        public Transform Transform;
        public Vector3 Direction;
        public Vector3 OldDirection;
        public float MovedDistance;
        public Vector3 StartPosition;
        public Vector3 OldPosition;
    }

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        size = renderer.bounds.size[0];

        // Добавляем голову змейки в список
        segments.Add(new SnakeSegment
        {
            Transform = transform,
            Direction = direction,
            OldDirection = direction,
            MovedDistance = 0f,
            StartPosition = transform.position,
            OldPosition = transform.position
        });

        // Добавляем начальные сегменты
        for (int i = 0; i < initialSegments; i++)
        {
            AddTail();
        }

        // Устанавливаем начальную стратегию движения
        Transform keyTransform = GameObject.FindGameObjectWithTag("Key").transform;
        movementBehavior = new PatrolMovement();
        movementBehavior.Initialize(transform, moveSpeed, size);
    }

    void Update()
    {
        // Получаем следующее направление от стратегии движения
        direction = movementBehavior.GetNextDirection(transform.position, direction, GetSegmentTransforms());
        Debug.Log(direction);
        // Если надо добавить хвост, делаем это в начале движения
        if (addTail && segments[0].MovedDistance == 0)
        {
            AddTail();
            addTail = false;
        }
    }

    void FixedUpdate()
    {
        Move();
        MoveTail();
    }

    void Move()
    {
        float step = moveSpeed * Time.deltaTime;
        SnakeSegment headSegment = segments[0];

        headSegment.Transform.Translate(step * headSegment.Direction);
        headSegment.MovedDistance += step;

        if (headSegment.MovedDistance >= size)
        {
            headSegment.Transform.position = headSegment.StartPosition + headSegment.Direction * size;
            headSegment.OldPosition = headSegment.StartPosition;
            headSegment.StartPosition = headSegment.Transform.position;
            headSegment.MovedDistance = 0;

            headSegment.OldDirection = headSegment.Direction;
            headSegment.Direction = direction;
        }
    }

    void MoveTail()
    {
        for (int i = 1; i < segments.Count; i++)
        {
            float step = moveSpeed * Time.deltaTime;
            SnakeSegment segment = segments[i];

            segment.Transform.Translate(step * segment.Direction);
            segment.MovedDistance += step;

            if (segment.MovedDistance >= size)
            {
                segment.Transform.position = segment.StartPosition + segment.Direction * size;
                segment.OldPosition = segment.StartPosition;
                segment.StartPosition = segment.Transform.position;
                segment.MovedDistance = 0;

                segment.OldDirection = segment.Direction;
                segment.Direction = segments[i - 1].OldDirection;
            }
        }
    }

    void AddTail()
    {
        SnakeSegment lastSegment = segments[segments.Count - 1];
        Vector3 spawnPosition = lastSegment.OldPosition;

        GameObject tailSegment = Instantiate(tailPrefab, spawnPosition, Quaternion.identity);

        segments.Add(new SnakeSegment
        {
            Transform = tailSegment.transform,
            Direction = lastSegment.OldDirection,
            OldDirection = lastSegment.OldDirection,
            MovedDistance = 0f,
            StartPosition = spawnPosition,
            OldPosition = spawnPosition
        });
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered with: " + other.gameObject.name);
        if (other.CompareTag("Food"))
        {
            Destroy(other.gameObject);
            FindObjectOfType<FoodSpawner>().spawnFood();
            addTail = true;
        }
    }

    private List<Transform> GetSegmentTransforms()
    {
        List<Transform> segmentTransforms = new List<Transform>();
        foreach (var segment in segments)
        {
            segmentTransforms.Add(segment.Transform);
        }
        return segmentTransforms;
    }
}
