using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TogglePlayPause : MonoBehaviour
{
    [SerializeField]
    GameObject Panel;
    [SerializeField]
    Sprite Play;
    [SerializeField]
    Sprite Pause;
    [SerializeField]
    Button button;

    [SerializeField]
    AudioSource source;
    [SerializeField]
    AudioClip clip;

    private bool playPause = false;

    public void PlayPause()
    {
        source.PlayOneShot(clip,0.5f);
        // Play
        if (!playPause)
        {
            //button.GetComponent<SpriteRenderer>().sprite = Play;
            button.image.sprite = Play;
            PlayerInfo.Instance.disableMovement = true;
            EventBus.Publish<StartCameraMovementEvent>(new StartCameraMovementEvent(0.0f));
            Panel.SetActive(true);
        }

        // Pause
        else
        {
            //button.GetComponent<SpriteRenderer>().sprite = Pause;
            button.image.sprite = Pause;
            EventBus.Publish<StartCameraMovementEvent>(new StartCameraMovementEvent(PlayerInfo.LastCheckPointCameraSpeed));
            PlayerInfo.Instance.disableMovement = false;
            Panel.SetActive(false);
        }

        playPause = !playPause;
    }

}
