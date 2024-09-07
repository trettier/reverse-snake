using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    public float moveSpeed = 5f; // �������� �������� ���������

    private Vector2 movement; // ������ ��� �������� ����������� ��������
    private SpriteRenderer spriteRenderer; // ������ �� ��������� SpriteRenderer
    private Dictionary<string, List<Sprite>> animations; // ������� ��� �������� ��������
    private string currentDirection = "down"; // ������� �����������
    private int currentFrame = 0; // ������� ���� ��������
    private float frameDelay = 0.1f; // �������� ����� �������
    private float lastFrameTime; // ����� ���������� ���������� �����

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // ������������� ��������
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
        // ��������� ����� �� ������������
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // ����� �������� ����� � ������� ��� �������
        Debug.Log($"Movement Input: {movement}");

        if (movement != Vector2.zero)
        {
            // ����������� ����������� ��������
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
            currentDirection = "static"; // ���� ��� ��������, ���������� ����������� ��������
        }

        // ���������� ��������
        UpdateAnimation();
    }

    void FixedUpdate()
    {
        // �������� ���������
        transform.Translate(movement * moveSpeed * Time.fixedDeltaTime);

        // ����� ����� ������� ��������� � ������� ��� �������
        Debug.Log($"New position: {transform.position}");
    }

    void UpdateAnimation()
    {
        // �������� ������� ���������� �����
        if (Time.time - lastFrameTime >= frameDelay)
        {
            // ���������� ����� ��������
            currentFrame++;
            if (currentFrame >= animations[currentDirection].Count)
            {
                currentFrame = 0; // ����� �����, ���� ��������� ����� ��������
            }

            spriteRenderer.sprite = animations[currentDirection][currentFrame]; // ��������� �������� �������
            lastFrameTime = Time.time; // ���������� ������� ���������� �����

            // ����� ���������� �� �������� � �������
            Debug.Log($"Current Animation: {currentDirection}, Frame: {currentFrame}");
        }
    }

    List<Sprite> LoadAnimation(string folder)
    {
        List<Sprite> frames = new List<Sprite>();

        // �������� ���� ������ �������� �� ��������� �����
        frames.AddRange(Resources.LoadAll<Sprite>(folder));

        // ����� ���������� ����������� ������ � �������
        Debug.Log($"Loaded {frames.Count} frames from folder: {folder}");

        return frames;
    }
}
