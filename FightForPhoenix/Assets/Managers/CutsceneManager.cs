using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    public Image[] images;          // Array of images for the cutscene
    public Text text;               // Array of standard UI texts for the cutscene, using UnityEngine.UI.Text
    public float timePerImage = 1f; // Time in seconds for each image
    public string gameSceneName = "Game"; // Name of the game scene to load
    public Config config;

    private int currentIndex = 0;
    private float timer;

    void Start()
    {
        if (images.Length > 0) {
            images[currentIndex].sprite = config.intro_sprites[currentIndex];
            text.text = config.intro_strings[currentIndex];
            DisplayImage(images[currentIndex]);
        } else {
            Debug.Log("Mismatch in number of images and texts or no images/texts provided");
        }
        timer = timePerImage;
    }

    void Update()
    {
        if (Input.anyKeyDown || (timer -= Time.deltaTime) <= 0) {
            if (currentIndex < images.Length) {
                if (currentIndex < images.Length - 1) {
                    var nextImage = images[currentIndex + 1];

                    text.text = config.intro_strings[currentIndex + 1];
                    // Change image and text
                    DisplayImage(nextImage);
                }
            }

            if (currentIndex == images.Length - 1) {
                // All images and texts shown, start the game
                SceneManager.LoadScene(gameSceneName);
            }
            timer = timePerImage;
            currentIndex += 1;
        }
    }

    void DisplayImage(Image image)
    {
        foreach (var img in images) {
            img.gameObject.SetActive(img == image);
        }
    }
}
