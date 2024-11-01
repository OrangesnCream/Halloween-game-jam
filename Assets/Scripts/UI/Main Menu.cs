using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame ()
    {
        // Loads the next scene in the queue
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadScene (string sceneName)
    {
        // Loads the scene with the given name
        Time.timeScale = 1f; 
        SceneManager.LoadScene(sceneName);
    }

    public void UnPause ()
    {
        // Unpauses the game
        Time.timeScale = 1f;
    }

    public void ReloadScene ()
    {
        // Reloads the current scene
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OpenLink (string link){
        Application.OpenURL(link);
    }
}
