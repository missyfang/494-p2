using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
//using static UnityEditor.Experimental.GraphView.GraphView;
using Vector3 = UnityEngine.Vector3;

public class MoveHand : MonoBehaviour
{
    private Vector3 orgPos;
    private Vector3 targPos;
    private Vector3 mousePos; 
    private bool isMoving = false;
    private Rigidbody rb;
    private GameObject hand;
    private float alternateIndex = 1;
    private GameObject[] validRockGos;
    private string validRockTag;


    [SerializeField]
    float flashDuration;
    [SerializeField]
    Material flashMaterial;
    [SerializeField]
    Material originalMaterial;
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
        validRockTag = "V" + PlayerInfo.Instance.Level.ToString();

        if (PlayerInfo.Instance.disableMovement == true)
            return;
        // On mouse click
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Alternate between hands
            if (alternateIndex > 0)
                hand = left;
            else
                hand = right;

            alternateIndex *= -1;

            if (!isMoving)
            {
                // Zero the z of target pos. 
                targPos = new Vector3(mousePos.x, mousePos.y, 0);

                // Check if reach is too far
                if (Vector3.Distance(head.transform.position, targPos) > maxReachDistance)
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

        // Move hand to target
        while (elapsedTime < timeToMove)
        {
            hand.transform.position = Vector3.Lerp(orgPos, targPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        hand.transform.position = targPos;

        // Publish event if grab valid rock 
        if (IsValidRock())
        {
            EventBus.Publish<SuccessfulGrab>(new SuccessfulGrab(hand));
            isMoving = false;
        }
        // Did not grab valid rock
        else
            StartCoroutine(HandFall());

      
    }

    // Move hand as close to mouse click osition as allowed
    private IEnumerator FailMove()
    {
        isMoving = true;
        float elapsedTime = 0;

        // Calculate the vector between the mouse position and hand
        Vector3 dir = (mousePos - hand.transform.position).normalized;
        orgPos = hand.transform.position;
        targPos = head.transform.position + (dir * maxReachDistance);
        targPos.z = 0;

        // Hand move up
        while (elapsedTime < timeToMove)
        {
            hand.transform.position = Vector3.Lerp(orgPos, targPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        hand.transform.position = targPos;

        // Pause
        yield return new WaitForSeconds(0.1f);

        // Hand falls below head
        StartCoroutine(HandFall());
        
       
    }


    // Move hand to postion below head. 
    private IEnumerator HandFall()
    {
        // Calculate the opposite vector between the mouse position and hand
        rb = hand.GetComponent<Rigidbody>();
        targPos = head.transform.position;
        targPos.z = 0;
        targPos.y = targPos.y - 2.0f;
        Vector3 dir = targPos - hand.transform.position;
        Debug.Log("this is the target" + targPos);
        // Hand fall down
        while (Vector3.Distance(hand.transform.position, targPos) > 0.1f)
        {
            rb.AddForce(dir.normalized * pushStrength, ForceMode.Force);
            yield return null;
        }
        
        rb.velocity = Vector3.zero;
        hand.transform.position = targPos;

        // Change alternating to use failed hand on next grab.
        alternateIndex *= -1;
        isMoving = false;
    }

    // Check if hand is at the same position as a valid rock
    private bool IsValidRock()
    {
        // Valid level rocks
        validRockGos = GameObject.FindGameObjectsWithTag(validRockTag);
        foreach(GameObject rock in validRockGos){
            if (Vector3.Distance(hand.transform.position, rock.transform.position) < 0.25f)
                return true;
        }
        // Check point rocks
        validRockGos = GameObject.FindGameObjectsWithTag("CheckPoint");
        foreach (GameObject rock in validRockGos)
        {
            if (Vector3.Distance(hand.transform.position, rock.transform.position) < 0.25f)
                return true;
        }

        // Check bad racks
        validRockGos = GameObject.FindGameObjectsWithTag("Bad");
        foreach (GameObject rock in validRockGos)
        {
            if (Vector3.Distance(hand.transform.position, rock.transform.position) < 0.25f)
            {
                //StartCoroutine(DamageEffect());
                Debug.Log("bad grab");
                EventBus.Publish<GrabDamageEvent>(new GrabDamageEvent());
            }
        }

        return false;
    }

    //// flash red and freeze when try to grab red rock. 
    //IEnumerator DamageEffect()
    //{
    //    PlayerInfo.Instance.disableMovement = true;
    //    // Flash
    //    for (int i = 0; i < 5; i++)
    //    {
    //        head.GetComponent<Renderer>().material = flashMaterial;
    //        yield return new WaitForSeconds(flashDuration);

    //        head.GetComponent<Renderer>().material = originalMaterial;
    //        yield return new WaitForSeconds(flashDuration);
    //    }

    //    PlayerInfo.Instance.disableMovement = false;

    //}

}


// publish when player makes a successful grab.
public class SuccessfulGrab
{
    public GameObject successfulHandGo;
    public SuccessfulGrab(GameObject _successfulHandGo) { successfulHandGo = _successfulHandGo; }

}

// publish when player takes damage
public class GrabDamageEvent
{
    public GrabDamageEvent() {}
}