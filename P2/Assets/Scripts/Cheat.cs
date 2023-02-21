using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject[] checkPoints;

    int level;
    private void Start()
    {
        level = 0;
    }
    // Cheat to jump levels.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            player.transform.position = checkPoints[level].transform.position;
            Camera.main.transform.position = new Vector3( Camera.main.transform.position.x, checkPoints[level].transform.position.y, Camera.main.transform.position.z);
           
            level++; 
        }
    }
}
