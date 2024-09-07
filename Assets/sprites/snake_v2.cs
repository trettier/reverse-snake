using System.Collections;
using UnityEngine;

public class snake_v2 : MonoBehaviour
{
    public float moveSpeed = 2f;  // Скорость движения в единицах в секунду
    public float distance = 10f;  // Расстояние, на которое нужно переместить объект
    public GameObject tailPrefab; // Префаб хвоста

    private Vector3 _startPosition;
    private Vector3 _direction = Vector3.right;  // Начальное направление движения
    private Vector3 _newDirection;  // Новое направление, если оно будет задано
    private Vector3 _spawnPoint; // Точка спавна хвоста
    private bool _directionChanged = false;  // Флаг для проверки, нужно ли менять направление


    void Start()
    {
        _startPosition = transform.position;
        _newDirection = _direction;  // Изначально новое направление такое же, как текущее
        StartCoroutine(MoveToDistance());
    }

    void Update()
    {
        // Проверка ввода пользователя для смены направления
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
            float movedDistance = 0f;  // Обнуляем пройденное расстояние

            while (movedDistance < distance)
            {
                float step = moveSpeed * Time.deltaTime;
                transform.Translate(_direction * step);

                movedDistance += step;

                // Если превышаем цель, скорректируем позицию
                if (movedDistance >= distance)
                {
                    transform.position = _startPosition + _direction * distance;
                    break;
                }

                yield return null;  // Ожидаем один кадр
            }

            // Объект прошел необходимое расстояние
            _startPosition = transform.position;

            // Если было нажато изменение направления, обновляем его
            if (_directionChanged)
            {
                _direction = _newDirection;
                _directionChanged = false;  // Сбрасываем флаг изменения направления
            }

            // Обновляем стартовую позицию и продолжаем движение без задержек
            movedDistance = 0f;  // Сбрасываем расстояние
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

    //            // Если превышаем цель, скорректируем позицию
    //            //if (movedDistanceTail >= distance)
    //            //{
    //            //    tail.transform.position = _startPosition + _direction * distance;
    //            //    break;
    //            //}

    //            yield return null;  // Ожидаем один кадр
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
