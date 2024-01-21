using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Config config;
    public string nextScene;

    protected void Start()
    {
        var a = FindObjectOfType<AudioSource>();
        a.clip = config.Menu;
        a.Play();
    }

    protected void Update()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                QuitGame();
            }
            else
            {
                SceneManager.LoadScene(nextScene);
            }
        }
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
    EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }

}



