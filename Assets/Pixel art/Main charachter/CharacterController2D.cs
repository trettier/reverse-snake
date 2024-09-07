using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    public float moveSpeed = 5f; // Скорость движения персонажа

    private Vector2 movement; // Вектор для хранения направления движения
    private SpriteRenderer spriteRenderer; // Ссылка на компонент SpriteRenderer
    private Dictionary<string, List<Sprite>> animations; // Словарь для хранения анимаций
    private string currentDirection = "down"; // Текущее направление
    private int currentFrame = 0; // Текущий кадр анимации
    private float frameDelay = 0.1f; // Задержка между кадрами
    private float lastFrameTime; // Время последнего обновления кадра

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Инициализация анимаций
        animations = new Dictionary<string, List<Sprite>>
        {
            { "up", LoadAnimation("up") },
            { "down", LoadAnimation("down") },
            { "left", LoadAnimation("left") },
            { "right", LoadAnimation("right") },
            { "static", LoadAnimation("static") }
        };
    }

    void Update()
    {
        // Получение ввода от пользователя
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Вывод значений ввода в консоль для отладки
        Debug.Log($"Movement Input: {movement}");

        if (movement != Vector2.zero)
        {
            // Определение направления движения
            if (movement.x < 0)
                currentDirection = "left";
            else if (movement.x > 0)
                currentDirection = "right";
            else if (movement.y > 0)
                currentDirection = "up";
            else if (movement.y < 0)
                currentDirection = "down";
        }
        else
        {
            currentDirection = "static"; // Если нет движения, используем статическую анимацию
        }

        // Обновление анимации
        UpdateAnimation();
    }

    void FixedUpdate()
    {
        // Движение персонажа
        transform.Translate(movement * moveSpeed * Time.fixedDeltaTime);

        // Вывод новой позиции персонажа в консоль для отладки
        Debug.Log($"New position: {transform.position}");
    }

    void UpdateAnimation()
    {
        // Проверка времени обновления кадра
        if (Time.time - lastFrameTime >= frameDelay)
        {
            // Обновление кадра анимации
            currentFrame++;
            if (currentFrame >= animations[currentDirection].Count)
            {
                currentFrame = 0; // Сброс кадра, если достигнут конец анимации
            }

            spriteRenderer.sprite = animations[currentDirection][currentFrame]; // Установка текущего спрайта
            lastFrameTime = Time.time; // Обновление времени последнего кадра

            // Вывод информации об анимации в консоль
            Debug.Log($"Current Animation: {currentDirection}, Frame: {currentFrame}");
        }
    }

    List<Sprite> LoadAnimation(string folder)
    {
        List<Sprite> frames = new List<Sprite>();

        // Загрузка всех кадров анимации из указанной папки
        frames.AddRange(Resources.LoadAll<Sprite>(folder));

        // Вывод количества загруженных кадров в консоль
        Debug.Log($"Loaded {frames.Count} frames from folder: {folder}");

        return frames;
    }
}
