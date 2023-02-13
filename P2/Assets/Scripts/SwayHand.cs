using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class SwayHand : MonoBehaviour
{
    float elapsedTime = 0;
    [SerializeField]
    float timeToMove;
    [SerializeField]
    float delta;
    [SerializeField]
    GameObject hand;
    [SerializeField]
    GameObject head;
    Vector3 orgPos;
    Vector3 targPos;
    bool isSwaying = false;
    // Start is called before the first frame update
    void Start()
    {
        orgPos = hand.transform.position;
        targPos = new Vector3(0.1921779f, -26.3f, 0f);
        targPos.x += delta;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSwaying)
            StartCoroutine(Sway());
    }

    IEnumerator Sway()
    {
        isSwaying = true;
        float elapsedTime = 0;
        while (elapsedTime < timeToMove)
        {
            hand.transform.position = Vector3.Lerp(orgPos, targPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        hand.transform.position = targPos;

        elapsedTime = 0;
        while (elapsedTime < timeToMove)
        {
            hand.transform.position = Vector3.Lerp(targPos, orgPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        hand.transform.position = orgPos;
        isSwaying = false; 
    }
}
