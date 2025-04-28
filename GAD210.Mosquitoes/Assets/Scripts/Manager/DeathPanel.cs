using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathPanel : MonoBehaviour
{
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void Play()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("NewMainHouse");
    }
}
