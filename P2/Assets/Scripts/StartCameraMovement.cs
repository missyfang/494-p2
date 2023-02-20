using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCameraMovement : MonoBehaviour
{
   
    public float cameraSpeed;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            EventBus.Publish<StartCameraMovementEvent>(new StartCameraMovementEvent(cameraSpeed));
    }
}


public class StartCameraMovementEvent
{
    public float cameraSpeed = 0;
    public StartCameraMovementEvent(float _cameraSpeed) { cameraSpeed = _cameraSpeed; }
    
}
