using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ImageSequenceController : MonoBehaviour
{
    public CanvasGroup[] images; // Assign your images with CanvasGroup in the inspector
    public float fadeDuration = 1.0f; // Duration of the fade effect
    private int currentIndex = 0; // Track the current image index

    private void Start()
    {
        // Initialize by hiding all images except the first one
        foreach (CanvasGroup img in images)
        {
            img.alpha = 0; // Set all images to be fully transparent
            img.interactable = false;
            img.blocksRaycasts = false;
        }
        if (images.Length > 0)
        {
            images[0].alpha = 1; // Make only the first image visible
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Check for mouse click
        {
            if (currentIndex < images.Length - 1) // Check if there are more images to show
            {
                StartCoroutine(FadeImage(currentIndex, currentIndex + 1));
                currentIndex++;
            } else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

    private IEnumerator FadeImage(int fromIndex, int toIndex)
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = elapsedTime / fadeDuration;
            //images[fromIndex].alpha = 1 - alpha; // Fade out the current image
            images[toIndex].alpha = alpha; // Fade in the next image
            yield return null;
        }

        images[fromIndex].alpha = 0; // Ensure the previous image is completely invisible
    }
}
