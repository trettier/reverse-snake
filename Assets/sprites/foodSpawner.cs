using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class FoodSpawner : MonoBehaviour
{
    public GameObject foodPrefab;  // Префаб еды
    public float minX, maxX, minY, maxY;  // Ограничения по координатам

    void Start()
    {
        Debug.Log("Функция Start не была вызвана только из-за маленькой буквы!!!!!!!!!!");
        spawnFood();
    }

    public void spawnFood()
    {
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);
        Vector3 spawnPosition = new Vector3(x, y, -1f);
        Instantiate(foodPrefab, spawnPosition, Quaternion.identity);
    }

}