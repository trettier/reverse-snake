using UnityEngine;
using UnityEngine.SceneManagement; // ��� ������������ ����

public class ButtonHandler : MonoBehaviour
{
    
    public void OnPlayButtonClicked()
    {
        //Debug.Log("Play button clicked!");

        // �������� ����� ���� (�������� "GameScene" �� �������� ����� �����)
        SceneManager.LoadScene("LevelMenu");
    }
}