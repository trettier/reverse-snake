using System.Collections;
using UnityEngine;
using UnityEngine.UI; // ��� ������ � UI ������������
using TMPro; // ��� ������������� TextMeshPro �����

public class MainCharacter : MonoBehaviour
{
    public float moveSpeed = 5f; // �������� �����������
    private Vector2 direction; // ����������� ��������

    private bool hasKey = false; // �������� ������� �����

    public GameObject gameOverPanel; // ������ ����� ����
    public TextMeshProUGUI gameOverText; // ��������� ��������� ����� ���� (���� ������������ TextMeshPro)

    private Rigidbody2D rb; // ������ �� Rigidbody2D ���������

    void Start()
    {
        // ������������� ���������� Rigidbody2D
        rb = GetComponent<Rigidbody2D>();

        // ��������� ������ ����� ���� ��� ������
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    void Update()
    {
        // ���������� ������������ �������� ��������� ��� ��������� ������
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
            direction = Vector2.zero; // ��������� ��� ���������� ���� ������
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        // ��������� ������� ����������� �� ����
        if (IsPathClear(direction))
        {
            // ������������� ������� Rigidbody2D �� ������ ����������� � �������� ��������
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        }
    }

    // ����� ��� �������� ������� ����������� �� ����
    bool IsPathClear(Vector2 moveDirection)
    {
        // ������� ������ ��� �������� �������� (��������, �������� ���� �����)
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(Physics2D.AllLayers);
        contactFilter.useTriggers = false;

        // ������ ��� �������� ����������� ��������
        RaycastHit2D[] hitResults = new RaycastHit2D[1];

        // ��������� ������� �� ����������, ������ ����� ����������� �� ���� ����
        float distance = moveDirection.magnitude * moveSpeed * Time.fixedDeltaTime;
        int hitCount = rb.Cast(moveDirection, contactFilter, hitResults, distance);

        // ���������� true, ���� ��� �����������
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
        key.GetComponent<ICollectible>().Collect(); // ����������� �����
        yield return new WaitForSeconds(0.5f); // �������� � 0.5 ������
        hasKey = true; // �������� ��������� ����
    }

    private void EndGame(string endText)
    {
        Debug.Log(endText);
        // �������� ������ ����� ����
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            // ������������� ��������� ���������
            if (gameOverText != null)
            {
                gameOverText.text = endText;
            }
        }

        // ������������� �������� ���������
        rb.velocity = Vector2.zero; // ��������� ����� ������ ��������
    }
}
