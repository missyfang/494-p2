using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMouseClickEffect : MonoBehaviour
{
    private bool isChangingColor = false;
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && !isChangingColor)
            StartCoroutine(RockHighlightOnCLick());
    }
    

    // Change color game object sightly on click
    private IEnumerator RockHighlightOnCLick()
    {
        isChangingColor = true;

        Color color = gameObject.GetComponent<Renderer>().material.color;
        gameObject.GetComponent<Renderer>().material.color = Color.Lerp(color, Color.white, 0.5f);

        yield return new WaitForSeconds(1.0f);

        gameObject.GetComponent<Renderer>().material.color = Color.Lerp(Color.white, color, 1f);

        isChangingColor = false;
    }
}
