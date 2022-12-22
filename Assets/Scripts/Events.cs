using UnityEngine.SceneManagement;
using UnityEngine;

public class Events : MonoBehaviour
{
    public void PlayGame() {
        SceneManager.LoadScene("Shooter");
    }

    public void ReplayGame() {
        SceneManager.LoadScene("Shooter");
    }
}