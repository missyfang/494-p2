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
    private void Update()
    {
        if (PlayerInfo.Instance.disableMovement == true)
            return;

        // detect clicks on screen not over obj
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // If click on something code should not ex
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out raycastHit, 100f))
            {
                if (raycastHit.transform != null)
                    return;
            }

            // Alternate between hands
            if (alternateIndex > 0)
                hand = left;
            else
                hand = right;

            alternateIndex *= -1;

            // set rock to null
            rock = null;
            // calc pos to move towards
            targPos = new Vector3(mousePos.x, mousePos.y, 0);

            //check how far the reach is
            if (Vector3.Distance(head.transform.position, targPos) > maxReachDistance)
                StartCoroutine(FailMove());
            else
                StartCoroutine(Move());
        }
    }

    void _OnDetectClickOnObjEvent(DetectClickOnObjEvent e)
    {
        if (PlayerInfo.Instance.disableMovement == true)
            return;
       
        rock = e.go;

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

        // Check if did not reach for any rock
        if(rock == null)
        {
            StartCoroutine(HandFall());
        }
        // Check if is bad rock
        else if (rock.CompareTag("Bad"))
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
    }



    // Move hand as close to mouse click osition as allowed
    private IEnumerator FailMove()
    {
        isMoving = true;
        float elapsedTime = 0;
       


        //// Calculate the vector between the mouse position and hand
        //if (rock != null)
        //{
        //    Vector3 temp = new Vector3(rock.transform.position.x, rock.transform.position.y, 0);
        //    dir = (temp - hand.transform.position).normalized;
        //}
        //else
        //{

        //}
        Vector3  dir = (targPos - hand.transform.position).normalized;
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

        // Check if rock is at hand pos
        if (rock != null)
        {
            GameObject[] validRockGos = GameObject.FindGameObjectsWithTag(validRockTag);
            GameObject aValidRock = null;
            foreach (GameObject r in validRockGos)
            {
                if (Vector3.Distance(hand.transform.position, r.transform.position) < 0.25)
                {
                    aValidRock = r;
                    break;
                }

            }
            // check if that rock is at mouse click pos
            if (aValidRock != null && Vector3.Distance(rock.transform.position, aValidRock.transform.position) < 0.1)
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
        else
            StartCoroutine(HandFall());
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