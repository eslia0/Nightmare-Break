using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BackGroundSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] backSound;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "LostTeddyBear_Stage1")
        {
            audioSource.Stop();
            audioSource.clip = backSound[2];
            audioSource.Play();
            ;
        } //else if (scene.name == "LostTeddyBear_Boss")
       // {
         //   audioSource.Stop();
         //   audioSource.clip = backSound[1];
          //  audioSource.Play();
      //  }

    }
}