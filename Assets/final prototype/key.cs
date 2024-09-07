using System.Collections;
using UnityEngine;

public class key : MonoBehaviour, ICollectible
{
    private Vector3 motion = new Vector3(0, 1, 0);  // ����������� ��������
    private float movedDistance = 0f;  // ���������� ����������
    private float distance = 1f;  // ����� ���������, ������� ������ ������ ������
    private float speed = 7.5f;  // �������� �������� �������

    public void Collect()
    {
        StartCoroutine(KeyAnimation());
    }

    private IEnumerator KeyAnimation()
    {
        while (movedDistance < distance)
        {
            float step = speed * Time.deltaTime;
            transform.Translate(motion * step);

            movedDistance += step;

            // ���� ��������� ����, ������������� ������� � ���������� ������
            if (movedDistance >= distance)
            {
                Destroy(gameObject);
                yield break;  // ��������� ��������
            }

            yield return null;  // ������� ���� ����
        }
    }
}