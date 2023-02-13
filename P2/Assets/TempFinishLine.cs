using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempFinishLine : MonoBehaviour
{
    [SerializeField]
    GameObject homeGo;
    [SerializeField]
    GameObject restartGo;
    [SerializeField]
    GameObject endText;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            homeGo.SetActive(true);
            endText.SetActive(true);
            restartGo.SetActive(true);
        }
        
    }

}
