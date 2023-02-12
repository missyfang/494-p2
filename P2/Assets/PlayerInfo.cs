using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{

    public Dictionary<string, Color> LevelToColor = new Dictionary<string, Color>()
    {
        {"V0", Color.blue},  {"V1", Color.green},  {"V2", Color.magenta}
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
