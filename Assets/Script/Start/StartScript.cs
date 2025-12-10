using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScript : MonoBehaviour
{

    void Start()
    {
        
    }

    void Update()
    {

    }

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Presist");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
