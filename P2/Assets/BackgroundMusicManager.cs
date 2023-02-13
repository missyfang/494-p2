using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusicManager : MonoBehaviour
{
    [SerializeField]
    AudioClip startClip;
    [SerializeField]
    AudioClip loopClip;
    private AudioSource audiosource;
    void Start()
    {
        audiosource = GetComponent<AudioSource>();
        audiosource.clip = startClip;
        audiosource.Play();
        StartCoroutine(playSound());
    }

    IEnumerator playSound()
    {
        audiosource.clip = startClip;
        audiosource.Play();
        yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length);
        audiosource.clip = loopClip;
        audiosource.Play();
    }
}
