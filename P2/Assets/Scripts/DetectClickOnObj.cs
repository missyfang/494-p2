using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectClickOnObj : MonoBehaviour
{
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            EventBus.Publish<DetectClickOnObjEvent>(new DetectClickOnObjEvent(this.gameObject));
        }
    }
}

public class DetectClickOnObjEvent
{
    public GameObject go;
    public DetectClickOnObjEvent(GameObject _go) { go = _go; }

   
}
