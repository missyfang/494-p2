using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCameraMovement : MonoBehaviour
{
   
    public float cameraSpeed;

    void OnTriggerEnter(Collider other)
    {
        
        EventBus.Publish<StartCameraMovementEvent>(new StartCameraMovementEvent(cameraSpeed));
        Debug.Log("publised camer move");
    }
}


public class StartCameraMovementEvent
{
    public float cameraSpeed = 0;
    public StartCameraMovementEvent(float _cameraSpeed) { cameraSpeed = _cameraSpeed; }
    
}
