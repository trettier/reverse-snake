using System.Collections;
using UnityEngine;

public class snake_v2 : MonoBehaviour
{
    public float moveSpeed = 2f;  // �������� �������� � �������� � �������
    public float distance = 10f;  // ����������, �� ������� ����� ����������� ������
    public GameObject tailPrefab; // ������ ������

    private Vector3 _startPosition;
    private Vector3 _direction = Vector3.right;  // ��������� ����������� ��������
    private Vector3 _newDirection;  // ����� �����������, ���� ��� ����� ������
    private Vector3 _spawnPoint; // ����� ������ ������
    private bool _directionChanged = false;  // ���� ��� ��������, ����� �� ������ �����������


    void Start()
    {
        _startPosition = transform.position;
        _newDirection = _direction;  // ���������� ����� ����������� ����� ��, ��� �������
        StartCoroutine(MoveToDistance());
    }

    void Update()
    {
        // �������� ����� ������������ ��� ����� �����������
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _newDirection = Vector3.left;
            _directionChanged = true;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _newDirection = Vector3.right;
            _directionChanged = true;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _newDirection = Vector3.up;
            _directionChanged = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _newDirection = Vector3.down;
            _directionChanged = true;
        }
    }

    private IEnumerator MoveToDistance()
    {
        while (true)
        {
            float movedDistance = 0f;  // �������� ���������� ����������

            while (movedDistance < distance)
            {
                float step = moveSpeed * Time.deltaTime;
                transform.Translate(_direction * step);

                movedDistance += step;

                // ���� ��������� ����, ������������� �������
                if (movedDistance >= distance)
                {
                    transform.position = _startPosition + _direction * distance;
                    break;
                }

                yield return null;  // ������� ���� ����
            }

            // ������ ������ ����������� ����������
            _startPosition = transform.position;

            // ���� ���� ������ ��������� �����������, ��������� ���
            if (_directionChanged)
            {
                _direction = _newDirection;
                _directionChanged = false;  // ���������� ���� ��������� �����������
            }

            // ��������� ��������� ������� � ���������� �������� ��� ��������
            movedDistance = 0f;  // ���������� ����������
        }
    }

    //private IEnumerator MoveToDistanceTail()
    //{
    //    while (true)
    //    {
    //        float movedDistanceTail = 0f;

    //        while (movedDistanceTail < distance)
    //        {
    //            float step = moveSpeed * Time.deltaTime;
    //            tail.transform.Translate(_direction * step);

    //            movedDistanceTail += step;

    //            // ���� ��������� ����, ������������� �������
    //            //if (movedDistanceTail >= distance)
    //            //{
    //            //    tail.transform.position = _startPosition + _direction * distance;
    //            //    break;
    //            //}

    //            yield return null;  // ������� ���� ����
    //        }

    //    }
    //}

    //void addSegment()
    //{
    //    _spawnPoint = _startPosition;
    //    tail = Instantiate(tailPrefab, _spawnPoint);
    //}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            Destroy(other.gameObject);
            FindObjectOfType<FoodSpawner>().spawnFood();

        }
    }
}
