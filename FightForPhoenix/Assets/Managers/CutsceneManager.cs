using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Using TextMeshPro for text elements

public class CutsceneManager : MonoBehaviour
{
    public Image[] images; // Array of images for the cutscene
    public TMP_Text[] texts; // Array of texts for the cutscene
    public float timePerImage = 1f; // Time in seconds for each image
    public string gameSceneName = "GameScene"; // Name of the game scene to load
    // public Config config;

    private int currentIndex = 0;
    private float timer;
    private float t1;
    private float t2 = 1;

    void Start()
    {
        if (images.Length > 0 && texts.Length == images.Length)
        {
            DisplayImage(images[currentIndex], texts[currentIndex]);
        }
        else
        {
            Debug.Log("Mismatch in number of images and texts or no images/texts provided");
        }
        timer = timePerImage;
    }

    void Update()
    {
        if (currentIndex < images.Length)
        {
            var currentImage = images[currentIndex];
            var currentText = texts[currentIndex];

            // Fade out current image and text
            t1 += Time.deltaTime / timePerImage;
            if (t1 > timePerImage)
                t1 = 1;

            currentImage.color = new Color(1f, 1f, 1f, 1f - t1);
            currentText.color = new Color(currentText.color.r, currentText.color.g, currentText.color.b, 1f - t1);
        }

        if (currentIndex < images.Length - 1)
        {
            var nextImage = images[currentIndex + 1];
            var nextText = texts[currentIndex + 1];

            // Fade in next image and text
            t2 -= Time.deltaTime / timePerImage;
            if (t2 < 0)
                t2 = 0;

            nextImage.color = new Color(1f, 1f, 1f, t2);
            nextText.color = new Color(nextText.color.r, nextText.color.g, nextText.color.b, t2);
        }

        if (Input.anyKeyDown || (timer -= Time.deltaTime) <= 0)
        {
            if (currentIndex < images.Length)
            {
                if (currentIndex < images.Length - 1)
                {
                    var nextImage = images[currentIndex + 1];
                    var nextText = texts[currentIndex + 1];

                    // Change image and text
                    DisplayImage(nextImage, nextText);

                    t1 = 0;
                    t2 = 1;
                }
            }

            if (currentIndex == images.Length - 1)
            {
                // All images and texts shown, start the game
                SceneManager.LoadScene(gameSceneName);
            }
            timer = timePerImage;
            currentIndex += 1;
        }
    }

    void DisplayImage(Image image, TMP_Text text)
    {
        foreach (var img in images)
        {
            img.gameObject.SetActive(img == image);
        }

        foreach (var txt in texts)
        {
            txt.gameObject.SetActive(txt == text);
        }
    }
}
