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
    public int Level = 0;
    public bool disableMovement = false; 
   

    private void Awake()
    {
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
