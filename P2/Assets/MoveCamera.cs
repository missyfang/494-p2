using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField]
    float upSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        Vector3 pos = transform.position;
        pos.y += 1.0f * Time.deltaTime * upSpeed;
        transform.position = pos;
        
    }
}
