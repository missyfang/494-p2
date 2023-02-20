using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FallingRockManager : MonoBehaviour
{
    

    [SerializeField]
    float speed = 1.0f;
    [SerializeField]
    float xAmount = 0.07f;
    [SerializeField]
    float yAmount = 0.02f;
    [SerializeField]
    float timeToShake = 3.0f;
    Vector3 orgPos;
    Rigidbody rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        // Fall
        rb.useGravity = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        // Do damage to player
        if (other.CompareTag("Player"))
        {
            EventBus.Publish<FallDamageEvent>(new FallDamageEvent());
            Debug.Log("hit player");
        }

        // Start falling effect
        if (other.CompareTag("FallThreshHold"))
        {
            Debug.Log("hit fall trigger");
            orgPos = transform.position;
            StartCoroutine(ShakeAndFall());
        }
    }


    IEnumerator ShakeAndFall()
    {     
        float elapsedTime = 0;
        // Shake effect
        while (elapsedTime < timeToShake)
        {
            transform.position = new Vector3(orgPos.x + Mathf.PingPong(Time.time, xAmount), orgPos.y, orgPos.z);
            //transform.position = new Vector3(orgPos.x, orgPos.y + Mathf.PingPong(Time.time * 2, yAmount), orgPos.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Fall
        rb.useGravity = true;
    }
}

public class FallDamageEvent
{
    public FallDamageEvent() {}
}