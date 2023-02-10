using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHead : MonoBehaviour
{
    Subscription<SuccessfulGrab> successful_grab_subscription;
    // Start is called before the first frame update
    void Start()
    {
        successful_grab_subscription = EventBus.Subscribe<SuccessfulGrab>(_OnSuccessfulGrab);
    }

    void _OnSuccessfulGrab(SuccessfulGrab e)
    {
        Debug.Log(e.successfulHandGo.transform.position);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    // Why are we destroying? 
    private void OnDestroy()
    {
        EventBus.Unsubscribe(successful_grab_subscription);
    }
}
