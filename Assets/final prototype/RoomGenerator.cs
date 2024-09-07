using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMazeGenerator : MonoBehaviour
{
    public GameObject wallPrefab; // ������ �����
    public float wallThickness = 1f; // ������� ����
    private void Start()
    {
        GenerateRoomWalls();
    }

    void GenerateRoomWalls()
    {
        // ������� ������
        Camera mainCamera = Camera.main;
        float screenHeight = 2f * mainCamera.orthographicSize;
        float screenWidth = screenHeight * mainCamera.aspect;

        // ������� �����
        CreateWall(new Vector2(0, screenHeight / 2), new Vector2(screenWidth, wallThickness));
        // ������ �����
        CreateWall(new Vector2(0, -screenHeight / 2), new Vector2(screenWidth, wallThickness));
        // ����� �����
        CreateWall(new Vector2(-screenWidth / 2, 0), new Vector2(wallThickness, screenHeight));
        // ������ �����
        CreateWall(new Vector2(screenWidth / 2, 0), new Vector2(wallThickness, screenHeight));
    }

    void CreateWall(Vector2 position, Vector2 size)
    {
        // ������� ������ �����
        GameObject wall = Instantiate(wallPrefab, position, Quaternion.identity);
        // ������������� ������ �����
        wall.transform.localScale = new Vector3(size.x, size.y, 1);
    }
}
