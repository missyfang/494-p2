using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScenes : MonoBehaviour
{
    [SerializeField]
    AudioSource source;
    [SerializeField]
    AudioClip clip;

    public void SwitchToLevelScene()
    {
        source.PlayOneShot(clip, 0.5f);
        SceneManager.LoadScene("LevelScene");
    }

    public void SwitchToStartScene()
    {
        source.PlayOneShot(clip, 0.5f);
        SceneManager.LoadScene("StartScene");
    }
      
}
