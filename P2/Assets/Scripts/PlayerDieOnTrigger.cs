using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDieOnTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject playerHead;
    [SerializeField]
    Material Deadmaterial;
  
    private void OnTriggerEnter(Collider other)
    {
        EventBus.Publish<PlayerHitBottomEvent>(new PlayerHitBottomEvent());
        playerHead.GetComponent<Renderer>().material = Deadmaterial;
    }
}

public class PlayerHitBottomEvent
{
    public PlayerHitBottomEvent() { }
}
