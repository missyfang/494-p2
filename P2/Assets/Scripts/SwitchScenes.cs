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
    public void SwitchToLevelSceneReset()
    {
        // Reset to level 0
        PlayerInfo.LastCheckPointPosition = new Vector3(0, -31, -1);
        PlayerInfo.LastCheckPointCameraPosition = new Vector3(0, -27.7000008f, -2);
        PlayerInfo.LastCheckPointCameraSpeed = 0;
        PlayerInfo.Level = 0;
        // Load scene
        SceneManager.LoadScene("LevelScene");
    }

    public void SwitchToStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }
      
}
