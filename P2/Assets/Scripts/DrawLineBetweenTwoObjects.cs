using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLineBetweenTwoObjects : MonoBehaviour
{
    [SerializeField]
    GameObject objectOne;
    [SerializeField]
    LineRenderer lineRenderer;  

    void Start()
    {
        // Set the position count of the linerenderer to two
        lineRenderer.positionCount = 2;

    }

    private void Update()
    {
        // Get the transform of the two objects
        Vector3 second = new Vector3(objectOne.transform.position.x, objectOne.transform.position.y, -0.01f);
        Vector3 first = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -0.01f);

        DrawLineBetweenObjects(first, second);
    }


    void DrawLineBetweenObjects(Vector3 firstPos, Vector3 secondPos)
    {
        // Set the positions of the LineRenderer
        lineRenderer.SetPosition(0, firstPos);
        lineRenderer.SetPosition(1, secondPos);
    }
}
