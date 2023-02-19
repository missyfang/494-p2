using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpOnTrigger : MonoBehaviour
{
    bool hasBeenTouched = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (hasBeenTouched)
                return;

            hasBeenTouched = true;

            // Update last checkpoint
            PlayerInfo.LastCheckPointPosition = transform.position;
            PlayerInfo.LastCheckPointCameraPosition = Camera.main.transform.position;
            PlayerInfo.LastCheckPointCameraSpeed = GetComponent<StartCameraMovement>().cameraSpeed;

            // Increase level.
            PlayerInfo.Instance.ModifyLevel(1);

            // Publish level up event.
            string level = "V" + PlayerInfo.Instance.Level.ToString();
            Color levelColor = PlayerInfo.Instance.LevelToColor[level];
            EventBus.Publish<PlayerNotificationEvent>(new PlayerNotificationEvent("Level Up! " + level, levelColor));
        }

    }
}

public class PlayerNotificationEvent
{
    public string message = "V0";
    public Color messageColor = Color.blue;
    public PlayerNotificationEvent(string _message, Color _messageColor) { message = _message; messageColor = _messageColor; }
}