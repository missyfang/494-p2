using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScenes : MonoBehaviour
{
    //[SerializeField]
    //GameObject Panel;
    //[SerializeField]
    //Sprite Play;
    //[SerializeField]
    //Sprite Pause;

    //bool playPause = false;

   public void SwitchToLevelScene()
    {
        SceneManager.LoadScene("LevelScene");
    }

    public void SwitchToStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }

    //public void PlayPause()
    //{
    //    // Play
    //    if (!playPause)
    //    {
    //        PlayerInfo.Instance.disableMovement = true;
    //        EventBus.Publish<StartCameraMovementEvent>(new StartCameraMovementEvent(0.0f));
    //        Panel.SetActive(true);
    //    }

    //    // Pause
    //    else {
    //        EventBus.Publish<StartCameraMovementEvent>(new StartCameraMovementEvent(PlayerInfo.LastCheckPointCameraSpeed));
    //        PlayerInfo.Instance.disableMovement = false;
    //        Panel.SetActive(false);
    //    }

    //    playPause = !playPause;
    //}

      
}
