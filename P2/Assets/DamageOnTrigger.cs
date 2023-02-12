using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    [SerializeField]
    Material flashMaterial;

    private void OnTriggerEnter(Collider other)
    {
        PlayerInfo.Instance.disableMovement = true;
    }
}
