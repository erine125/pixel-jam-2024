using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextboxManager : MonoBehaviour
{
    public GameObject[] textboxes; // Assign all your textboxes here in order
    public AudioSource audiosource;
    private int currentTextboxIndex = 0;

    void Start()
    {
        HideAllTextboxes();
        ShowNextTextbox();
    }

    void HideAllTextboxes()
    {
        foreach (GameObject textbox in textboxes)
        {
            textbox.SetActive(false);
        }
    }

    public void ShowNextTextbox()
    {
        if (currentTextboxIndex < textboxes.Length)
        {
            textboxes[currentTextboxIndex].SetActive(true);
            Button btn = textboxes[currentTextboxIndex].GetComponent<Button>();
            btn.onClick.AddListener(OnTextboxClicked);
        }
      
    }

    void OnTextboxClicked()
    {
        // Hide the current textbox
        audiosource.Play();
        textboxes[currentTextboxIndex].SetActive(false);
        

        // Increment the index to point to the next textbox
        currentTextboxIndex++;

        // Show the next textbox
        ShowNextTextbox();
    }
}