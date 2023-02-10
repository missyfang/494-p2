using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    Subscription<PlayerHitBottomEvent> player_hit_bottom_event_sub;
    Subscription<StartCameraMovement> start_camera_movement_event_sub;

    float upSpeed = 0.5f;
    bool shouldMoveCamera;

    // Start is called before the first frame update
    void Start()
    {
        shouldMoveCamera = true;

        player_hit_bottom_event_sub = EventBus.Subscribe<PlayerHitBottomEvent>(_OnPlayerHitBottom);
       // start_camera_movement_event_sub = EventBus.Subscribe<StartCameraMovement>(_OnStartCameraMovement);

    }

    // Update is called once per frame
    void Update()
    {
       

        if (!shouldMoveCamera)
            return;

        Vector3 pos = transform.position;
        pos.y += 1.0f * Time.deltaTime * upSpeed;
        transform.position = pos;
        
    }

    void _OnPlayerHitBottom(PlayerHitBottomEvent e)
    {
        shouldMoveCamera = false;

    }

    void _OnStartCameraMovement(StartCameraMovement e)
    {
        Debug.Log("move camera bitch");
        shouldMoveCamera = true;
        upSpeed = e.cameraSpeed;

    }
    private void OnDestroy()
    {
        EventBus.Unsubscribe(player_hit_bottom_event_sub);
        EventBus.Unsubscribe(start_camera_movement_event_sub);
    }
}
