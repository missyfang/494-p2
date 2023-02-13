using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScenes : MonoBehaviour
{
    
   public void SwitchToLevelScene()
    {
        SceneManager.LoadScene("LevelScene");
    }

    public void SwitchToStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }
}
