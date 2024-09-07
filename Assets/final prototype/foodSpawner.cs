using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class FoodSpawner : MonoBehaviour
{
    public GameObject foodPrefab;  // Префаб еды
    public int minX, maxX, minY, maxY;  // Ограничения по координатам

    void Start()
    {
        spawnFood();
    }

    public void spawnFood()
    {
        int x = Random.Range(minX, maxX);
        int y = Random.Range(minY, maxY);
        Vector3 spawnPosition = new Vector3(x, y, -1f);
        Instantiate(foodPrefab, spawnPosition, Quaternion.identity);
    }

}