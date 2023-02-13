using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    Subscription<FallDamageEvent> fall_damage_event_subscription;
    Subscription<GrabDamageEvent> grab_damage_event_subscription;

    [SerializeField]
    float flashDuration;
    [SerializeField]
    Material flashMaterial;
    [SerializeField]
    Material originalMaterial;
    [SerializeField]
    GameObject head;

    bool takingDamage = false;
 
    void Start()
    {
        fall_damage_event_subscription = EventBus.Subscribe<FallDamageEvent>(_OnFallDamageEvent);
        grab_damage_event_subscription = EventBus.Subscribe<GrabDamageEvent>(_OnGrabDamageEvent);
    }

    // Damage when hit with falling rock
    void _OnFallDamageEvent(FallDamageEvent e)
    {
        if (!takingDamage)
            StartCoroutine(DamageEffect());
    }

    // Damage when grab a bad rock
    void _OnGrabDamageEvent(GrabDamageEvent e)
    {
        if (!takingDamage)
            StartCoroutine(DamageEffect());
    }


    // flash red and freeze when try to grab red rock. 
    IEnumerator DamageEffect()
    {
        Debug.Log("playe was damaged");
        takingDamage = true;

        // Freeze
        PlayerInfo.Instance.disableMovement = true;

        // Flash
        for (int i = 0; i < 3; i++)
        {
            head.GetComponent<Renderer>().material = flashMaterial;
            yield return new WaitForSeconds(flashDuration);

            head.GetComponent<Renderer>().material = originalMaterial;
            yield return new WaitForSeconds(flashDuration);
        }

        PlayerInfo.Instance.disableMovement = false;
        takingDamage = false;
    }
}
