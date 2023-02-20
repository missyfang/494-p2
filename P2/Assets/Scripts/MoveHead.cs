using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class MoveHead : MonoBehaviour
{
    [SerializeField]
    float MaxDistanceBetweenHeadandHand;
    [SerializeField]
    float timeToMove;
    [SerializeField]
    GameObject rightHand;
    [SerializeField]
    GameObject leftHand;

    GameObject handToMoveTowards;
   
    Vector3 orgPos;
    Vector3 targPos;
    Vector3 direction;

    Subscription<SuccessfulGrab> successful_grab_subscription;
   
    void Start()
    {
        successful_grab_subscription = EventBus.Subscribe<SuccessfulGrab>(_OnSuccessfulGrab);
    }

    void _OnSuccessfulGrab(SuccessfulGrab e)
    {
        handToMoveTowards = e.successfulHandGo;
        StartCoroutine(Move());
    }

    
    // Why are we destroying? 
    private void OnDestroy()
    {
        EventBus.Unsubscribe(successful_grab_subscription);
    }


    // Move body towards hand that just successfully grabbed. 
    IEnumerator Move()
    {
        float elapsedTime = 0;
        orgPos = transform.position;
        direction = (handToMoveTowards.transform.position - transform.position).normalized;
        targPos = orgPos + direction;

        while (Vector3.Distance(transform.position, handToMoveTowards.transform.position) > 0.75f && elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(orgPos, targPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
       
        Debug.Log(Vector3.Distance(transform.position, handToMoveTowards.transform.position));

    }
}
