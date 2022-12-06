using UnityEngine.SceneManagement;
using UnityEngine;

public class Events : MonoBehaviour
{
    public void PlayGame()
    {
        
    }

    public void FreePlay() {
        SceneManager.LoadSceneAsync("MiniGame");
    }
    public void ReplayGame() {
        SceneManager.LoadScene("MiniGame");
    }

    public void QuitToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame() {
        Application.Quit();
    }
}
