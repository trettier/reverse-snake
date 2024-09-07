using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Для работы с UI компонентами
using TMPro; // Для использования TextMeshPro типов

public class MainCharacter : MonoBehaviour
{
    public float moveSpeed = 5f; // Скорость перемещения
    private Vector2 direction; // Направление движения

    private bool hasKey = false; // Проверка наличия ключа

    public GameObject gameOverPanel; // Панель конца игры
    public TextMeshProUGUI gameOverText; // Текстовое сообщение конца игры (если используется TextMeshPro)

    private Rigidbody2D rb; // Ссылка на Rigidbody2D персонажа

    void Start()
    {
        // Инициализация компонента Rigidbody2D
        rb = GetComponent<Rigidbody2D>();

        // Отключаем панель конца игры при старте
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    void Update()
    {
        // Управление направлением движения персонажа при удержании клавиш
        if (Input.GetKey(KeyCode.A))
        {
            direction = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            direction = Vector2.right;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            direction = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            direction = Vector2.down;
        }
        else
        {
            direction = Vector2.zero; // Остановка при отпускании всех клавиш
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        // Проверяем наличие препятствий на пути
        if (IsPathClear(direction))
        {
            // Устанавливаем позицию Rigidbody2D на основе направления и скорости движения
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        }
    }

    // Метод для проверки наличия препятствий на пути
    bool IsPathClear(Vector2 moveDirection)
    {
        // Создаем фильтр для проверки коллизий (например, проверка всех слоев)
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(Physics2D.AllLayers);
        contactFilter.useTriggers = false;

        // Массив для хранения результатов кастинга
        RaycastHit2D[] hitResults = new RaycastHit2D[1];

        // Выполняем кастинг на расстояние, равное длине перемещения за один кадр
        float distance = moveDirection.magnitude * moveSpeed * Time.fixedDeltaTime;
        int hitCount = rb.Cast(moveDirection, contactFilter, hitResults, distance);

        // Возвращаем true, если нет препятствий
        return hitCount == 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Key"))
        {
            StartCoroutine(CollectAndDestroy(other));
        }
        else if (other.CompareTag("Lock") && hasKey)
        {
            EndGame("Win");
        }
        else if (other.CompareTag("Snake"))
        {
            EndGame("Lose");
        }
    }

    private IEnumerator CollectAndDestroy(Collider2D key)
    {
        key.GetComponent<ICollectible>().Collect(); // Перемещение ключа
        yield return new WaitForSeconds(0.5f); // Задержка в 0.5 секунд
        hasKey = true; // Персонаж подбирает ключ
    }

    private void EndGame(string endText)
    {
        Debug.Log(endText);
        // Включаем панель конца игры
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            // Устанавливаем текстовое сообщение
            if (gameOverText != null)
            {
                gameOverText.text = endText;
            }
        }

        // Останавливаем движение персонажа
        rb.velocity = Vector2.zero; // Остановка путем сброса скорости
    }
}
