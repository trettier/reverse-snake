using System.Collections;
using UnityEngine;

public class key : MonoBehaviour, ICollectible
{
    private Vector3 motion = new Vector3(0, 1, 0);  // Направление движения
    private float movedDistance = 0f;  // Пройденное расстояние
    private float distance = 1f;  // Общая дистанция, которую должен пройти объект
    private float speed = 7.5f;  // Скорость движения объекта

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

            // Если превышаем цель, скорректируем позицию и уничтожаем объект
            if (movedDistance >= distance)
            {
                Destroy(gameObject);
                yield break;  // Завершаем корутину
            }

            yield return null;  // Ожидаем один кадр
        }
    }
}