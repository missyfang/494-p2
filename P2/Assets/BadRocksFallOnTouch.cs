using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class BadRocksFallOnTouch : MonoBehaviour
{
   
    [SerializeField]
    float speed = 5.0f;
    [SerializeField]
    float xAmount = 0.5f;
    [SerializeField]
    float yAmount;
    [SerializeField]
    float timeToShake = 1.0f;
    Vector3 orgPos;

    private void OnTriggerEnter(Collider other)
    {
        // Detecting falling rock at threshold
        if (other.CompareTag("FallingRock")) 
            StartCoroutine(Shake(other.gameObject));

    }

    IEnumerator Shake(GameObject go)
    {
        Rigidbody rb = go.GetComponent<Rigidbody>();
        float elapsedTime = 0;
        orgPos = go.transform.position;

        // Shake effect
        while (elapsedTime < timeToShake)
        {
            go.transform.position =  new Vector3(orgPos.x + Mathf.PingPong(Time.time, xAmount), go.transform.position.y, go.transform.position.z);
            go.transform.position = new Vector3(go.transform.position.x, orgPos.y + Mathf.PingPong(Time.time*2, yAmount), go.transform.position.z);
            elapsedTime += Time.deltaTime;
            yield return null; 
        }

        // Fall
        rb.useGravity = true;
    }
}
