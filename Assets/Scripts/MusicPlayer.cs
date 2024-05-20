using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private static MusicPlayer instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            GameObject.DontDestroyOnLoad(gameObject); // Make this object persistent
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Ensures only one instance of the Music Player exists
        }
    }
}