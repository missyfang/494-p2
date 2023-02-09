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


    void Update()
    {
        // On mouse click
        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // determin closer hand
            if (Vector3.Distance(left.transform.position, mousePos) < Vector3.Distance(right.transform.position, mousePos))
                hand = left;
            else
                hand = right;

            if (!isMoving)
            {
                targPos = new Vector3(mousePos.x, mousePos.y, 0);
                if (Vector3.Distance(hand.transform.position, targPos) > maxReachDistance)
                {
                    StartCoroutine(FailMove());
                }
                else
                    StartCoroutine(Move());
            }
        }
    }


    // Move Hand to position
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


        isMoving = false;
    }

    // Move hand as close to position as allowed
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
        rb.velocity = Vector3.zero;
        isMoving = false;
    }
}
