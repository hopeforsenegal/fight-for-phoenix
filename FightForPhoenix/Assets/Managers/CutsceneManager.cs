using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    public Image[] images; // Array of images for the cutscene
    public float timePerImage = 1f; // Time in seconds for each image
    public string gameSceneName = "GameScene"; // Name of the game scene to load
    public Config config;

    private int currentIndex = 0;
    private float timer;
    private float t1;
    private float t2 = 1;

    void Start()
    {
        if(images.Length > 0)
        {
            DisplayImage(images[currentIndex]);
        }else{
            Debug.Log("No images");
        }
        timer = timePerImage;

        //for (int i1 = 0; i1 < images.Length; i1++)
        //{
        //    var image = images[i1];
        //    image.sprite = config.sprites[i1];
        //}
    }

    void Update()
    {
        if (currentIndex < images.Length)
        {
            var current = images[currentIndex];
            // Fade out current image
            t1 += Time.deltaTime / timePerImage;
            if (t1 > timePerImage)
                t1 = 1;
            {
                current.color = new Color(1f, 1f, 1f, 1f - t1);
            }
        }

        if (currentIndex < images.Length - 1)
        {
            var next = images[currentIndex + 1];

            // Fade in next image
            t2 -= Time.deltaTime / timePerImage;
            if (t2 < 0)
                t2 = 0;
            {
                next.color = new Color(1f, 1f, 1f, t2);
            }
        }

        if (Input.anyKeyDown || (timer -= Time.deltaTime) <= 0)
        {
            Debug.Log("Do Next");
            if (currentIndex < images.Length)
            {
                var c = currentIndex;
                Debug.Log($"Do currentIndex {c}");

                if (c < images.Length - 1)
                {
                    var next = images[c + 1];
                    // Change image
                    DisplayImage(next);

                    t1 = 0;
                    t2 = 1;
                }
            }

            if(currentIndex == images.Length - 1)
            {
                // All images shown, start the game
                Debug.Log("Starting the game.");
                SceneManager.LoadScene(gameSceneName);
            }
            timer = timePerImage;

            currentIndex += 1;
        }
    }

    void DisplayImage(Image image)
    {
        foreach(var img in images)
        {
            img.gameObject.SetActive(img == image);
        }
    }
}