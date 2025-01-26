using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private AudioSource audioSource;

    public void OnStartButton()
    {
        audioSource.Play();
        SceneManager.LoadScene(1);
    }

    public void OnQuitButton()
    {
        audioSource.Play();
        Application.Quit();
    }
    public void OnMainMenuButton()
    {
        SceneManager.LoadScene(0);
    }
}
