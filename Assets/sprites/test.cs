using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    public GameObject snakePrefab;

    void Start()
    {
        // Создаем новый объект с помощью Instantiate
        GameObject snakeClone = Instantiate(snakePrefab, new Vector3(0, 0, 0), Quaternion.identity);

        // Проверяем, что у клона есть необходимые компоненты
        Collider2D collider = snakeClone.GetComponent<Collider2D>();
        if (collider == null)
        {
            collider = snakeClone.AddComponent<BoxCollider2D>();
            collider.isTrigger = true; // Если нужен триггер
        }

        Rigidbody2D rb = snakeClone.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = snakeClone.AddComponent<Rigidbody2D>();
            rb.isKinematic = true; // Используем кинематическое тело для стабильной физики
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered with: " + other.gameObject.name);
    }
}