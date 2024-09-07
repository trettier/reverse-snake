using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHead : MonoBehaviour
{
    public GameObject headPrefab;
    
    public float moveSpeed;
    public float x, y = 0f;
    public float speedMultiplier = 1f;
    public Vector3 direction = Vector3.left;

    private GameObject headSprite;
    private Vector3 lastCage;
    private Vector3 newDirection = Vector3.left;
    private Vector3 lastDirection;
    private float nextUpdate;

    [SerializeField]
    private KeyCode _leftKeyCode;

    [SerializeField]
    private KeyCode _rightKeyCode;

    [SerializeField]
    private KeyCode _downKeyCode;

    [SerializeField]
    private KeyCode _forwardKeyCode;

    void Start()
    {
        // Инициализируем голову змейки в начальной позиции
        Vector3 headSpawn = new Vector3(x, y, -1f);
        lastCage = headSpawn;
        headSprite = Instantiate(headPrefab, headSpawn, Quaternion.identity);
    }
    void Update()
    {
        HandleInput(); // Обработка ввода пользователя
        
    }

    void FixedUpdate()
    {
        if (Time.time < nextUpdate)     // Wait until the next update before proceeding
        {
            return;
        }
        MoveHead();    // Перемещение головы змейки
        nextUpdate = Time.time + (1f / (moveSpeed * speedMultiplier));
    }

    private void HandleInput()
    {
        // Обработка направления на основе нажатий клавиш
        if (Input.GetKeyDown(_forwardKeyCode) && lastDirection != Vector3.down)
        {
            newDirection = Vector3.up;
        }
        else if (Input.GetKeyDown(_leftKeyCode) && lastDirection != Vector3.right)
        {
            newDirection = Vector3.left;
        }
        else if (Input.GetKeyDown(_downKeyCode) && lastDirection != Vector3.up)
        {
            newDirection = Vector3.down;
        }
        else if (Input.GetKeyDown(_rightKeyCode) && lastDirection != Vector3.left)
        {
            newDirection = Vector3.right;
        }
    }

    private void MoveHead()
    {
        // Если змейка прошла одну клетку (расстояние >= 1), меняем направление
        if (Vector3.Distance(lastCage, headSprite.transform.position) >= 1f)
        {
            lastDirection = direction;
            direction = newDirection;
            lastCage = headSprite.transform.position;
        }

        // Перемещаем голову змейки в направлении с учетом скорости
        headSprite.transform.Translate(direction * Time.deltaTime);
    }
}
