using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using static UnityEngine.GraphicsBuffer;
using Input = UnityEngine.Input;

public class RotateArm : MonoBehaviour
{
    [SerializeField]
    float rotationSpeed = 10;

    //values for internal use
    private Quaternion _lookRotation;
    private Vector3 _direction;


    Rigidbody rb;
    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }
    void Update ()
 {
        //if(Input.GetKey(KeyCode.UpArrow))
        //{
        //    transform.Rotate (0, 0, 1 * Time.deltaTime * rotationSpeed, Space.World);
        //}

        //if(Input.GetKey(KeyCode.DownArrow))
        //{
        //    transform.Rotate (0, 0, -1 * Time.deltaTime * rotationSpeed, Space.World);
        //}

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
           // Vector3 difference = Input.mousePosition - transform.position;
          
            var objectScreenPosition = Camera.main.WorldToScreenPoint(rb.position);
            var difference = Input.mousePosition - objectScreenPosition;
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, Quaternion.Euler(0, 0, rotationZ), rotationSpeed));
            //  transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        }
    }

    IEnumerator RotateToCursor()
    {
        Debug.Log("called");
        //Vector3 target = Input.mousePosition;
        //find the vector pointing from our position to the target
        _direction = (Input.mousePosition - transform.position).normalized;

        //create the rotation we need to be in to look at the target
        _lookRotation = Quaternion.LookRotation(_direction);

        while (Quaternion.Angle(transform.rotation, _lookRotation) > 0.01f)
        {
            Debug.Log("rotating");
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _lookRotation, Time.deltaTime);
            yield return null;
        }

        Debug.Log("DOne");
        //rotate us over time according to speed until we are in the required rotation
        //transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * rotationSpeed);
    }


}
