using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject HeadHorizontal1;
    public GameObject HeadHorizontal2;
    public GameObject BodyHorizontal;
    public GameObject TailHorizontal1;
    public GameObject TailHorizontal2;

    public GameObject HeadVertical1;
    public GameObject HeadVertical2;
    public GameObject BodyVertical;
    public GameObject TailVertical1;
    public GameObject TailVertical2;

    public Vector3 SpawnPoint;
    public int BodyCount = 2;
    public float MoveSpeed = 0.1f;

    private Vector3 direction = Vector3.left;
    private float size;
    private List<SnakeSegment> segments = new List<SnakeSegment>();

    private List<Vector3> patrolPoints = new List<Vector3>(); // Список патрульных точек
    private IMovementBehavior2 movementBehavior; // Интерфейс движения

    private class SnakeSegment
    {
        public GameObject GameObject;
        public Vector3 StartPosition;
        public Vector3 Direction;
        public Vector3 OldPosition;
        public Vector3 OldDirection;
        public float MovedDistance;
    }

    void Start()
    {
        Renderer renderer = HeadHorizontal1.GetComponent<Renderer>();
        size = renderer.bounds.size.x;
        //Debug.Log(size.ToString());

        // Инициализация патрульных точек
        InitializePatrolPoints();

        // Инициализация патрульного поведения
        movementBehavior = new PatrolMovement2();
        movementBehavior.Initialize(transform, patrolPoints);

        segments.Add(new SnakeSegment
        {
            GameObject = Instantiate(HeadHorizontal1, SpawnPoint, Quaternion.identity),
            StartPosition = SnapToGrid(SpawnPoint),
            Direction = direction,
            MovedDistance = 0f,
            OldDirection = direction,
            OldPosition = transform.position
        });

        SpawnPoint.x += size;
        segments.Add(new SnakeSegment
        {
            GameObject = Instantiate(HeadHorizontal2, SpawnPoint, Quaternion.identity),
            StartPosition = SnapToGrid(SpawnPoint),
            Direction = direction,
            MovedDistance = 0f,
            OldDirection = direction,
            OldPosition = transform.position
        });

        for (int i = 0; i < BodyCount; i++)
        {
            SpawnPoint.x += size;
            segments.Add(new SnakeSegment
            {
                GameObject = Instantiate(BodyHorizontal, SpawnPoint, Quaternion.identity),
                StartPosition = SnapToGrid(SpawnPoint),
                Direction = direction,
                MovedDistance = 0f,
                OldDirection = direction,
                OldPosition = transform.position
            });
        }

        SpawnPoint.x += size;
        segments.Add(new SnakeSegment
        {
            GameObject = Instantiate(TailHorizontal1, SpawnPoint, Quaternion.identity),
            StartPosition = SnapToGrid(SpawnPoint),
            Direction = direction,
            MovedDistance = 0f,
            OldDirection = direction,
            OldPosition = transform.position
        });

        SpawnPoint.x += size;

        segments.Add(new SnakeSegment
        {
            GameObject = Instantiate(TailHorizontal2, SpawnPoint, Quaternion.identity),
            StartPosition = SnapToGrid(SpawnPoint),
            Direction = direction,
            MovedDistance = 0f,
            OldDirection = direction,
            OldPosition = transform.position
        });
    }

    private void InitializePatrolPoints()
    {
        // Добавьте свои координаты для патруля здесь
        patrolPoints.Add(new Vector3(-3, -1.95f, -1));
        patrolPoints.Add(new Vector3(-3, 8.05f, -1));
        patrolPoints.Add(new Vector3(7, 8.05f, -1));
        patrolPoints.Add(new Vector3(7, -1.95f, -1));
        // Можете добавить больше точек, чтобы определить маршрут
    }

    private Vector3 SnapToGrid(Vector3 position)
    {
        return position;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            direction = Vector3.left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            direction = Vector3.right;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            direction = Vector3.up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            direction = Vector3.down;
        }
    }

    void FixedUpdate()
    {
        for (int i = 0; i < segments.Count; i++)
        {
            SnakeSegment segment = segments[i];
            float step = MoveSpeed;

            segment.GameObject.transform.Translate(step * segment.Direction);
            segment.MovedDistance += step;
            if (segment.MovedDistance > 1f)
            {
                segment.GameObject.transform.position = segment.StartPosition + segment.Direction;
                segment.OldPosition = segment.StartPosition;
                segment.StartPosition = segment.GameObject.transform.position;
                segment.MovedDistance = 0;

                segment.OldDirection = segment.Direction;
                if (i == 0)
                {
                    // Используем патрульное поведение для головы змейки
                    Debug.Log(segment.GameObject.transform.position);
                    segment.Direction = movementBehavior.GetNextDirection(segment.GameObject.transform.position, segment.Direction);
                    direction = segment.Direction; // Обновляем текущее направление для тела змейки
                }
                else
                {
                    segment.Direction = segments[i - 1].OldDirection;
                }

                if (segment.OldDirection != segment.Direction)
                {
                    UpdateSegmentSprite(segment, i);
                }
            }
        }
    }

    private void UpdateSegmentSprite(SnakeSegment segment, int index)
    {
        SpriteRenderer spriteRenderer = segment.GameObject.GetComponent<SpriteRenderer>();

        // Вспомогательная функция для установки спрайта и отражения
        void SetSpriteAndFlip(GameObject spritePrefab, bool flipX, bool flipY)
        {
            ChangeSprite(segment.GameObject, spritePrefab);
            spriteRenderer.flipX = flipX;
            spriteRenderer.flipY = flipY;
        }

        // Определение спрайта и отражений для направлений
        if (segment.Direction == Vector3.left)
        {
            if (index == 0) // Голова 1 
            {
                SetSpriteAndFlip(HeadHorizontal1, false, false);
            }
            else if (index == 1) // Голова 2
            {
                SetSpriteAndFlip(HeadHorizontal2, false, false);
            }
            else if (index == segments.Count - 2) // Хвост 1
            {
                SetSpriteAndFlip(TailHorizontal1, false, false);
            }
            else if (index == segments.Count - 1) // Хвост 2
            {
                SetSpriteAndFlip(TailHorizontal2, false, false);
            }
            else // Тело
            {
                SetSpriteAndFlip(BodyHorizontal, false, false);
            }
        }
        else if (segment.Direction == Vector3.right)
        {
            if (index == 0) // Голова 1 
            {
                SetSpriteAndFlip(HeadHorizontal1, true, false);
            }
            else if (index == 1) // Голова 2
            {
                SetSpriteAndFlip(HeadHorizontal2, true, false);
            }
            else if (index == segments.Count - 2) // Хвост 1
            {
                SetSpriteAndFlip(TailHorizontal1, true, false);
            }
            else if (index == segments.Count - 1) // Хвост 2
            {
                SetSpriteAndFlip(TailHorizontal2, true, false);
            }
            else // Тело
            {
                SetSpriteAndFlip(BodyHorizontal, true, false);
            }
        }
        else if (segment.Direction == Vector3.up)
        {
            if (index == 0) // Голова 1
            {
                SetSpriteAndFlip(HeadVertical1, false, false);
            }
            else if (index == 1) // Голова 2
            {
                SetSpriteAndFlip(HeadVertical2, false, false);
            }
            else if (index == segments.Count - 2) // Хвост 1
            {
                SetSpriteAndFlip(TailVertical1, false, false);
            }
            else if (index == segments.Count - 1) // Хвост 2
            {
                SetSpriteAndFlip(TailVertical2, false, false);
            }
            else // Тело
            {
                SetSpriteAndFlip(BodyVertical, false, false);
            }
        }
        else if (segment.Direction == Vector3.down)
        {
            if (index == 0) // Голова 1
            {
                SetSpriteAndFlip(HeadVertical1, false, true);
            }
            else if (index == 1) // Голова 2
            {
                SetSpriteAndFlip(HeadVertical2, false, true);
            }
            else if (index == segments.Count - 2) // Хвост 1
            {
                SetSpriteAndFlip(TailVertical1, false, true);
            }
            else if (index == segments.Count - 1) // Хвост 2
            {
                SetSpriteAndFlip(TailVertical2, false, true);
            }
            else // Тело
            {
                SetSpriteAndFlip(BodyVertical, false, true);
            }
        }
    }

    private void ChangeSprite(GameObject obj, GameObject newPrefab)
    {
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
        SpriteRenderer newSpriteRenderer = newPrefab.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null && newSpriteRenderer != null)
        {
            spriteRenderer.sprite = newSpriteRenderer.sprite;
        }
    }
}




