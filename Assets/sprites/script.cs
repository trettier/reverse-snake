using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Скорость движения змейки
    private Vector3 direction = Vector3.right;
    private Vector3 lastDirection;
    public GameObject tailprefab;
    private List<Transform> tailSegments = new List<Transform>();
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && lastDirection != Vector3.down)
        {
            direction = Vector3.up; 
        }
        else if (Input.GetKeyDown(KeyCode.A) && lastDirection != Vector3.right)
        {
            direction = Vector3.left;
        }
        else if (Input.GetKeyDown(KeyCode.S) && lastDirection != Vector3.up) 
        {
            direction = Vector3.down;
        }
        else if (Input.GetKeyDown(KeyCode.D) && lastDirection != Vector3.left)
        {
            direction = Vector3.right;
        }

        lastDirection = direction;
        // Двигаем голову змейки
        moveTail();
        transform.position += direction * moveSpeed * Time.deltaTime;
        
    }
    void moveTail()
    {
        Vector3 previousPosition = transform.position;

        foreach (Transform segment in tailSegments)
        { 
            Vector3 tempPosition = segment.position;
            segment.position = previousPosition;
            previousPosition = tempPosition;
        }
    }
    public void addSegment()
    {
        GameObject newSegment = Instantiate(tailprefab);
        tailSegments.Add(newSegment.transform);
    }
    //void OnCollisionEnter2D(Collider2D collision)
    //{
    //    Debug.Log("коллизия");
    //    if (collision.gameObject.CompareTag("Food"))
    //    {
    //        Destroy(collision.gameObject);
    //        addSegment();
    //        FindObjectOfType<FoodSpawner>().spawnFood();
    //    }
    //}
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("коллизия");
        // Проверяем, если коллайдер триггерный
        if (other.CompareTag("Food"))
        {
            // Уничтожаем круг
            Destroy(other.gameObject);
            addSegment();
            FindObjectOfType<FoodSpawner>().spawnFood();
        }
    }
}
