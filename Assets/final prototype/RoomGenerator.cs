using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMazeGenerator : MonoBehaviour
{
    public GameObject wallPrefab; // Префаб стены
    public float wallThickness = 1f; // Толщина стен
    private void Start()
    {
        GenerateRoomWalls();
    }

    void GenerateRoomWalls()
    {
        // Размеры камеры
        Camera mainCamera = Camera.main;
        float screenHeight = 2f * mainCamera.orthographicSize;
        float screenWidth = screenHeight * mainCamera.aspect;

        // Верхняя стена
        CreateWall(new Vector2(0, screenHeight / 2), new Vector2(screenWidth, wallThickness));
        // Нижняя стена
        CreateWall(new Vector2(0, -screenHeight / 2), new Vector2(screenWidth, wallThickness));
        // Левая стена
        CreateWall(new Vector2(-screenWidth / 2, 0), new Vector2(wallThickness, screenHeight));
        // Правая стена
        CreateWall(new Vector2(screenWidth / 2, 0), new Vector2(wallThickness, screenHeight));
    }

    void CreateWall(Vector2 position, Vector2 size)
    {
        // Создаем объект стены
        GameObject wall = Instantiate(wallPrefab, position, Quaternion.identity);
        // Устанавливаем размер стены
        wall.transform.localScale = new Vector3(size.x, size.y, 1);
    }
}
