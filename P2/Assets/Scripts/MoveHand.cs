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
    float timeToFall;

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

    [SerializeField]
    GameObject reachRadiuslight;

    [SerializeField]
    AudioClip grabSfx;
    [SerializeField]
    AudioClip badGrabSfx;
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    float volume;

    [SerializeField]
    GameObject pauseButton;

    private void Start()
    {
        detect_click_on_obj_event_sub = EventBus.Subscribe<DetectClickOnObjEvent>(_OnDetectClickOnObjEvent);
    }
    private void Update()
    {
        if (PlayerInfo.Instance.disableMovement == true)
            return;

        // Detect clicks on screen not over obj
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // If click on something code should not ex
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out raycastHit, 300f))
            { 
                if (raycastHit.transform != null && !raycastHit.transform.CompareTag("Untagged" ))
                    return;
            }

            // Explicit check for click on pause button for some reason
            // Debug.Log("Distance" + Vector3.Distance(mousePos, Camera.main.ScreenToWorldPoint(pauseButton.transform.position)));
            if (Vector3.Distance(mousePos, Camera.main.ScreenToWorldPoint(pauseButton.transform.position)) < 1.0f)
                return;

            // Alternate between hands
            if (alternateIndex > 0)
                hand = left;
            else
                hand = right;

            alternateIndex *= -1;

            // Set rock to null
            rock = null;
            // Calc pos to move towards
            targPos = new Vector3(mousePos.x, mousePos.y, -1);

            // Check if reach is too far
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
            // Set current valid rock tag
            validRockTag = "V" + PlayerInfo.Instance.Level.ToString();

            // Alternate between hands
            if (alternateIndex > 0)
                hand = left;
            else
                hand = right;

            alternateIndex *= -1;

            // Zero the z of target pos. 
            targPos = new Vector3(rock.transform.position.x, rock.transform.position.y, -1);

            // Check if reach is too far
            if (Vector3.Distance(head.transform.position, targPos) > maxReachDistance)
            {
                StartCoroutine(FailMove());
            }
            else
                StartCoroutine(Move());

        }
    }

    // Move Hand to mouse click position
    private IEnumerator Move()
    {
        // Turn off hands gravity
        rb = hand.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

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
            StartCoroutine(HandFall(timeToFall));
        }
        // Check if is bad rock
        else if (rock.CompareTag("Bad"))
        {
            audioSource.PlayOneShot(badGrabSfx, volume);
            StartCoroutine(HandFall(timeToFall));
            EventBus.Publish<GrabDamageEvent>(new GrabDamageEvent());
        }

        // good grab publich event
        else
        {
            audioSource.PlayOneShot(grabSfx, volume);
            EventBus.Publish<SuccessfulGrab>(new SuccessfulGrab(hand));
            isMoving = false;
        }
    }



    // Move hand as close to mouse click osition as allowed
    private IEnumerator FailMove()
    {
        // Turn off hands gravity
        rb = hand.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

        isMoving = true;
        float elapsedTime = 0;
      
        Vector3  dir = (targPos - hand.transform.position).normalized;
        orgPos = hand.transform.position;
        targPos = head.transform.position + (dir * maxReachDistance);
        targPos.z = -1;

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
                // extra time penalty for over reach
                StartCoroutine(HandFall(timeToFall + 1));
                // Turn on reach radius light
                reachRadiuslight.SetActive(true);

                // Notify player that rock was too far on first attempt
                if (!NotifiedTooFar)
                {
                    NotifiedTooFar = true;
                    EventBus.Publish<PlayerNotificationEvent>(new PlayerNotificationEvent("Too far!", Color.black));
                }
            }
        }
        else
            StartCoroutine(HandFall(timeToFall));
    }


    // Turn on gracity to allow hand to fall. 
    private IEnumerator HandFall(float time)
    {
        // Turn on hands gravity for fall  
        rb = hand.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;
        float elapsedTime = 0;

        // small penalty
        while (elapsedTime < time)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        reachRadiuslight.SetActive(false);
        // Change alternating to use failed hand on next grab.
        alternateIndex *= -1;
        yield return null;
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