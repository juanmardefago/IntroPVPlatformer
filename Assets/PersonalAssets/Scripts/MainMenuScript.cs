using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {


    public void NewGame()
    {
        SceneManager.LoadScene("DemoLevel"); 
    }

    public void Continue()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }
}
