using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class MoveHand : MonoBehaviour
{
    Subscription<DetectClickOnObjEvent> detect_click_on_obj_event_sub;

    private Vector3 orgPos;
    private Vector3 targPos;
    private Vector3 mousePos; 
    private bool isMoving = false;
    private Rigidbody rb;
    private GameObject hand;
    private float alternateIndex = 1;
   // private GameObject[] validRockGos;
    private string validRockTag;
    private bool NotifiedTooFar = false;
    private GameObject rock;

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

    private void Start()
    {
        detect_click_on_obj_event_sub = EventBus.Subscribe<DetectClickOnObjEvent>(_OnDetectClickOnObjEvent);
    }

    void _OnDetectClickOnObjEvent(DetectClickOnObjEvent e)
    {
        if (PlayerInfo.Instance.disableMovement == true)
            return;

        rock = e.go;

        //// Alternate between hands
        //if (alternateIndex > 0)
        //    hand = left;
        //else
        //    hand = right;

        //alternateIndex *= -1;

        if (!isMoving)
        {
            validRockTag = "V" + PlayerInfo.Instance.Level.ToString();

            // Alternate between hands
            if (alternateIndex > 0)
                hand = left;
            else
                hand = right;

            alternateIndex *= -1;
            // Zero the z of target pos. 
            targPos = new Vector3(rock.transform.position.x, rock.transform.position.y, 0);

            // Check if reach is too far
            if (Vector3.Distance(head.transform.position, targPos) > maxReachDistance)
                StartCoroutine(FailMove());
            else
                StartCoroutine(Move());
        }
    }


    void Update()
    {
        //validRockTag = "V" + PlayerInfo.Instance.Level.ToString();

        //    if (PlayerInfo.Instance.disableMovement == true)
        //        return;
        //    // On mouse click
        //    if (Input.GetMouseButtonDown(0) && !isMoving)
        //    {
        //        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //        // Alternate between hands
        //        if (alternateIndex > 0)
        //            hand = left;
        //        else
        //            hand = right;

        //        alternateIndex *= -1;

        //        if (!isMoving)
        //        {
        //            // Zero the z of target pos. 
        //            targPos = new Vector3(mousePos.x, mousePos.y, 0);

        //            // Check if reach is too far
        //            if (Vector3.Distance(head.transform.position, targPos) > maxReachDistance)
        //                StartCoroutine(FailMove());
        //            else
        //                StartCoroutine(Move());
        //        }

        //    }
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

        // Check if is bad rock
        if (rock.CompareTag("Bad"))
        {
            StartCoroutine(HandFall());
            EventBus.Publish<GrabDamageEvent>(new GrabDamageEvent());
        }

        // good grab publich event
        else
        {
            EventBus.Publish<SuccessfulGrab>(new SuccessfulGrab(hand));
            isMoving = false;
        }

        //// Publish event if grab valid rock 
        //if (IsValidRock())
        //{
        //    EventBus.Publish<SuccessfulGrab>(new SuccessfulGrab(hand));
        //    isMoving = false;
        //}
        //// Did not grab valid rock
        //else
        //{
        //    StartCoroutine(HandFall());
        //}
    }



    // Move hand as close to mouse click osition as allowed
    private IEnumerator FailMove()
    {
        isMoving = true;
        float elapsedTime = 0;

        // Calculate the vector between the mouse position and hand
        Vector3 dir = (rock.transform.position - hand.transform.position).normalized;
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

       //StartCoroutine(HandFall());

        // Check if rock is at hand pos
        GameObject[] validRockGos = GameObject.FindGameObjectsWithTag(validRockTag);
        GameObject aValidRock = null;
        foreach (GameObject rock in validRockGos)
        {
            if (Vector3.Distance(hand.transform.position, rock.transform.position) < 0.3)
            {
                aValidRock = rock;
                break;
            }

        }
        // check if that rok is at mouse click pos
        if (aValidRock != null && aValidRock == rock)
        {
            EventBus.Publish<SuccessfulGrab>(new SuccessfulGrab(hand));
            isMoving = false;
        }

        else
        {
            // Notify player that rock was too far on first attempt
            if (!NotifiedTooFar)
            {
                NotifiedTooFar = true;
                EventBus.Publish<PlayerNotificationEvent>(new PlayerNotificationEvent("Too far!", Color.black));
            }
            StartCoroutine(HandFall());
        }
    }


    // Move hand to postion below head. 
    private IEnumerator HandFall()
    {
        // Calculate the opposite vector between the mouse position and hand
        rb = hand.GetComponent<Rigidbody>();
        targPos = head.transform.position;
        targPos.z = 0;
        targPos.y = targPos.y - 1.0f;
        Vector3 dir = targPos - hand.transform.position;

        // Hand fall down
        while (Vector3.Distance(hand.transform.position, targPos) > 0.1f)
        {
            rb.AddForce(dir.normalized * pushStrength, ForceMode.Force);
            yield return null;
        }
        
        rb.velocity = Vector3.zero;
        hand.transform.position = targPos;

        // Change alternating to use failed hand on next grab.
        Debug.Log("hand index" + alternateIndex);
        alternateIndex *= -1;
        isMoving = false;
    }

    //// Check if hand is at the same position as a valid rock
    //private bool IsValidRock()
    //{
    //    // Check bad rocks
    //    validRockGos = GameObject.FindGameObjectsWithTag("Bad");
    //    foreach (GameObject rock in validRockGos)
    //    {
    //        if (Vector3.Distance(hand.transform.position, rock.transform.position) < 0.25f)
    //        {
    //            Debug.Log("bad grab");
    //            EventBus.Publish<GrabDamageEvent>(new GrabDamageEvent());
    //        }
    //    }
    //    // Valid level rocks
    //    validRockGos = GameObject.FindGameObjectsWithTag(validRockTag);
    //    foreach(GameObject rock in validRockGos){
    //        if (Vector3.Distance(hand.transform.position, rock.transform.position) < 0.35f)
    //        { 
    //            return true;
    //        }
    //    }
    //    // Check point rocks
    //    validRockGos = GameObject.FindGameObjectsWithTag("CheckPoint");
    //    foreach (GameObject rock in validRockGos)
    //    {
    //        if (Vector3.Distance(hand.transform.position, rock.transform.position) < 0.25f)
    //            return true;
    //    }
    //    return false;
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