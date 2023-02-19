using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{ 
    public Dictionary<string, Color> LevelToColor = new Dictionary<string, Color>()
    {
        {"V0", Color.blue},  {"V1", new Color(0f, 0.745283f, 0.1683396f, 1.0f)},  {"V2", new Color(1f, 0.3333333f, 0.9792358f, 1.0f)},  {"V3",  new Color(1f, 0.5583236f, 0f, 1.0f)}
    };
  

    // Singelton 
    public static PlayerInfo Instance;
    // Player starting pos
    public static Vector3 LastCheckPointPosition = new Vector3(0, -31, -1);
    // Player starting pos
    public static Vector3 LastCheckPointCameraPosition = new Vector3(0, -27.7000008f, -2);
    public static float LastCheckPointCameraSpeed = 0; 
   public int Level = 0;
    public bool disableMovement = false; 
   

    private void Awake()
    {
        // Set up for last checkpoint 
        transform.position = LastCheckPointPosition;
        Camera.main.transform.position = LastCheckPointCameraPosition;
        EventBus.Publish<StartCameraMovementEvent>(new StartCameraMovementEvent(LastCheckPointCameraSpeed));

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
       
    }


    public void ModifyLevel(int levelDelta)
    {
        Level += levelDelta;
    }

}
