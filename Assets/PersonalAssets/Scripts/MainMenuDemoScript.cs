using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuDemoScript : MonoBehaviour {

    public void NewGame()
    {
        SceneManager.LoadScene("IceMap");
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("MainMenuDemo");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
