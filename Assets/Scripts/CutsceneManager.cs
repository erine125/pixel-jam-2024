using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ImageSequenceController : MonoBehaviour
{
    public CanvasGroup[] images; // 
    public AudioClip[] sfx; // list of SFX to play with each scene
    public AudioSource audiosource;
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
                if (sfx[currentIndex] != null)
                {
                    audiosource.PlayOneShot(sfx[currentIndex], 0.4f);
                }

                StartCoroutine(FadeImage(currentIndex, currentIndex + 1));
                currentIndex++;
            } else
            {
                GoToNextScene();
            }
        }
    }

    public void GoToNextScene()
    {
        // Get the current scene's build index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // Get the total number of scenes in the build settings
        int totalScenes = SceneManager.sceneCountInBuildSettings;

        // Calculate the next scene index
        // If the current scene is the last one, cycle back to the first scene (index 0)
        int nextSceneIndex = (currentSceneIndex + 1) % totalScenes;

        // Load the next scene
        SceneManager.LoadScene(nextSceneIndex);
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
