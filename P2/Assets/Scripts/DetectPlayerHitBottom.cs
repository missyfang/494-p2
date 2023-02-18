using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayerHitBottom : MonoBehaviour
{
    [SerializeField]
    GameObject playerHead;
    [SerializeField]
    Material Deadmaterial;
    [SerializeField]
    float bottomOfScreenOffset;

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(playerHead.transform.position);

        if (screenPos.y < bottomOfScreenOffset)
        {
            Debug.Log("Died at" + screenPos);
            
            EventBus.Publish<PlayerHitBottomEvent>(new PlayerHitBottomEvent());
            playerHead.GetComponent<Renderer>().material = Deadmaterial; 
        }
    }

}

//public class PlayerHitBottomEvent
//{
//    public PlayerHitBottomEvent() { }
//}
