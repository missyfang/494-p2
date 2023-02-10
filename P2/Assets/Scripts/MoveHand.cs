using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using Vector3 = UnityEngine.Vector3;

public class MoveHand : MonoBehaviour
{
    private Vector3 orgPos;
    private Vector3 targPos;
    private Vector3 mousePos; 
    private bool isMoving = false;
    private Rigidbody rb;
    private GameObject hand;
    private LineRenderer lineRenderer;

    [SerializeField]
    float timeToMove;
    [SerializeField]
    float maxReachDistance;
    [SerializeField]
    float pushStrength;
    [SerializeField]
    GameObject left;
    [SerializeField]
    GameObject right;
    [SerializeField]
    GameObject head;


    void Update()
    {
        // On mouse click
        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // determin closer the hand
            if (Vector3.Distance(left.transform.position, mousePos) < Vector3.Distance(right.transform.position, mousePos))
                hand = left;
            else
                hand = right;

            if (!isMoving)
            {
                targPos = new Vector3(mousePos.x, mousePos.y, 0);
                // Check if reach is too far
                if (Vector3.Distance(hand.transform.position, targPos) > maxReachDistance)
                {
                    StartCoroutine(FailMove());
                }
                else
                    StartCoroutine(Move());
            }
        }
    }


    // Move Hand to mouse click position
    private IEnumerator Move()
    {
        isMoving = true;
        float elapsedTime = 0;
        orgPos = hand.transform.position;

        while (elapsedTime < timeToMove)
        {
            hand.transform.position = Vector3.Lerp(orgPos, targPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        hand.transform.position = targPos;

        EventBus.Publish<SuccessfulGrab>(new SuccessfulGrab(hand));

        isMoving = false;
    }

    // Move hand as close to mouse click osition as allowed
    private IEnumerator FailMove()
    {
        isMoving = true;
        //Calculate the vector between the object and the player
        Vector3 dir = mousePos - hand.transform.position;
        orgPos = hand.transform.position;
        rb = hand.GetComponent<Rigidbody>();
        while (Vector3.Distance(hand.transform.position, orgPos) < maxReachDistance)
        {
            //Translate the object in the direction of the vector
            rb.AddForce(dir.normalized * pushStrength);
            yield return null; 
        }
        dir = orgPos - hand.transform.position;
        while (Vector3.Distance(hand.transform.position, head.transform.position) < maxReachDistance)
        {
            //Translate the object in the direction of the vector
            rb.AddForce(dir.normalized * pushStrength);
            yield return null;
        }
        rb.velocity = Vector3.zero;
        isMoving = false;
    }


}

public class SuccessfulGrab
{
    public GameObject successfulHandGo;
    public SuccessfulGrab(GameObject _successfulHandGo) { successfulHandGo = _successfulHandGo; }

}