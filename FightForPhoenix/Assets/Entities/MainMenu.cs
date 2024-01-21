using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Config config;
    public Button play;
    public Button quit;
    public string nextScene;

    protected void Start()
    {
        var a = FindObjectOfType<AudioSource>();
        a.clip = config.Menu;
        a.Play();

        play.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(nextScene);
        });
        quit.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        });
    }
}
