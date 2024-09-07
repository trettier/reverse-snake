using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake_v3 : MonoBehaviour
{
    public GameObject SnakePrefab; // ������, ������� ����� ����������������
    public Vector3 SpawnPoint;     // ����� ������
    public Vector3 Direction;      // ����������� ��������
    public float MoveSpeed;        // �������� ��������

    private List<GameObject> SnakeParts;  // ������ ������ ������
    private List<Vector3> StartPositions; // ��������� ������� ������ ������
    private List<Vector3> Directions;     // ����������� ������ ������
    private float Distance = 1f;          // ��������� ����� �������
    private float MovedDistance;          // ���������� ����������

    void Start()
    {
        SnakeParts = new List<GameObject>();
        StartPositions = new List<Vector3>();
        Directions = new List<Vector3>();

        // ������� � ��������� ������ ����� ������ (������)
        GameObject SnakeHead = Instantiate(SnakePrefab, SpawnPoint, Quaternion.identity);
        SnakeParts.Add(SnakeHead);
        StartPositions.Add(SpawnPoint);
        Directions.Add(Direction);
    }

    void Update()
    {
        // ���������� ������������ �������� ������
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
        // ������� ���� ������ ������ � �������� �������
        for (int i = SnakeParts.Count - 1; i >= 0; i--)
        {
            float step = MoveSpeed * Time.deltaTime;
            SnakeParts[i].transform.Translate(Directions[i] * step);
            MovedDistance += step;

            // ��������, ������ �� ����� ������ ���� ������
            if (MovedDistance >= Distance)
            {
                SnakeParts[i].transform.position = StartPositions[i] + Directions[i] * Distance;
                StartPositions[i] = SnakeParts[i].transform.position;
                MovedDistance = 0;

                if (i == 0)
                {
                    Directions[i] = Direction;  // ������ ������ ������ �����������
                }
                else
                {
                    Directions[i] = Directions[i - 1];  // ��������� ����� ������� �� ����������
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("1 Triggered with: " + other.gameObject.name);
        // �������� �� ������������ � ��������, ������� ��� "Food"
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
        // �������� ��������� ����� ������
        GameObject lastPart = SnakeParts[SnakeParts.Count - 1];
        Vector3 newPartPosition = lastPart.transform.position - Directions[Directions.Count - 1] * Distance;

        // ������� ����� ����� ������ � ��������� �� � ������
        GameObject newPart = Instantiate(SnakePrefab, newPartPosition, Quaternion.identity);
        SnakeParts.Add(newPart);
        StartPositions.Add(newPartPosition);
        Directions.Add(Directions[Directions.Count - 1]);  // ����� ����� ������� �� ���������
    }
}
