using UnityEngine;
using UnityEngine.SceneManagement; // Для переключения сцен

public class ButtonHandler : MonoBehaviour
{
    
    public void OnPlayButtonClicked()
    {
        //Debug.Log("Play button clicked!");

        // Загрузка сцены игры (замените "GameScene" на название вашей сцены)
        SceneManager.LoadScene("LevelMenu");
    }
}