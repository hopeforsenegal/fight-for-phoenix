using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    public Image image;             // Array of images for the cutscene
    public Text text;               // Array of standard UI texts for the cutscene, using UnityEngine.UI.Text
    public float timePerImage = 1f; // Time in seconds for each image
    public string gameSceneName = "Game"; // Name of the game scene to load
    public Config config;

    private int currentIndex = 0;
    private float timer;

    void Start()
    {
        image.sprite = config.intro_sprites[currentIndex];
        text.text = config.intro_strings[currentIndex];
        timer = timePerImage;
    }

    void Update()
    {
        if (Input.anyKeyDown || (timer -= Time.deltaTime) <= 0) {
            if (currentIndex < config.intro_sprites.Length - 1) {
                text.text = config.intro_strings[currentIndex + 1];
                image.sprite = config.intro_sprites[currentIndex + 1];
            }

            if (currentIndex == config.intro_sprites.Length - 1) {
                SceneManager.LoadScene(gameSceneName);
            }
            timer = timePerImage;
            currentIndex += 1;
        }
    }
}
