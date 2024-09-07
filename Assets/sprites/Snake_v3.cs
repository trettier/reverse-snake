using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake_v3 : MonoBehaviour
{
    public GameObject SnakePrefab; // Префаб, который будет инстанцироваться
    public Vector3 SpawnPoint;     // Точка спауна
    public Vector3 Direction;      // Направление движения
    public float MoveSpeed;        // Скорость движения

    private List<GameObject> SnakeParts;  // Список частей змейки
    private List<Vector3> StartPositions; // Начальные позиции частей змейки
    private List<Vector3> Directions;     // Направления частей змейки
    private float Distance = 1f;          // Дистанция между частями
    private float MovedDistance;          // Пройденное расстояние

    void Start()
    {
        SnakeParts = new List<GameObject>();
        StartPositions = new List<Vector3>();
        Directions = new List<Vector3>();

        // Создаем и добавляем первую часть змейки (голову)
        GameObject SnakeHead = Instantiate(SnakePrefab, SpawnPoint, Quaternion.identity);
        SnakeParts.Add(SnakeHead);
        StartPositions.Add(SpawnPoint);
        Directions.Add(Direction);
    }

    void Update()
    {
        // Управление направлением движения змейки
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Direction = Vector3.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Direction = Vector3.right;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Direction = Vector3.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Direction = Vector3.down;
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        // Перебор всех частей змейки в обратном порядке
        for (int i = SnakeParts.Count - 1; i >= 0; i--)
        {
            float step = MoveSpeed * Time.deltaTime;
            SnakeParts[i].transform.Translate(Directions[i] * step);
            MovedDistance += step;

            // Проверка, прошла ли часть змейки одну клетку
            if (MovedDistance >= Distance)
            {
                SnakeParts[i].transform.position = StartPositions[i] + Directions[i] * Distance;
                StartPositions[i] = SnakeParts[i].transform.position;
                MovedDistance = 0;

                if (i == 0)
                {
                    Directions[i] = Direction;  // Голова змейки меняет направление
                }
                else
                {
                    Directions[i] = Directions[i - 1];  // Остальные части следуют за предыдущей
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("1 Triggered with: " + other.gameObject.name);
        // Проверка на столкновение с объектом, имеющим тег "Food"
        if (SnakeParts[0].GetComponent<Collider2D>().IsTouching(other))
        {
            if (other.CompareTag("Food"))
            {
                Destroy(other.gameObject);
                FindObjectOfType<FoodSpawner>().spawnFood();
                AddSnakePart();
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collided with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Food"))
        {
            Destroy(collision.gameObject);
            FindObjectOfType<FoodSpawner>().spawnFood();
            AddSnakePart();
        }
    }


    void AddSnakePart()
    {
        // Получаем последнюю часть змейки
        GameObject lastPart = SnakeParts[SnakeParts.Count - 1];
        Vector3 newPartPosition = lastPart.transform.position - Directions[Directions.Count - 1] * Distance;

        // Создаем новую часть змейки и добавляем ее в список
        GameObject newPart = Instantiate(SnakePrefab, newPartPosition, Quaternion.identity);
        SnakeParts.Add(newPart);
        StartPositions.Add(newPartPosition);
        Directions.Add(Directions[Directions.Count - 1]);  // Новая часть следует за последней
    }
}
