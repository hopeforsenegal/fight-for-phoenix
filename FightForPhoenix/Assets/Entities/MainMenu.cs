using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public Config config;

    protected void Start()
    {
        var a = FindObjectOfType<AudioSource>();
        a.clip = config.Menu;
        a.Play();
    }
}
