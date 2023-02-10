using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackGround : MonoBehaviour
{
    [SerializeField]
    private Transform centerBackground; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y >= centerBackground.position.y + 14.0f)
        {
            centerBackground.position = new Vector3(centerBackground.position.x, transform.position.y + 14f, 0);
        }
    }
}
