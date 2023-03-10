using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    Subscription<ScoreEvent> score_event_subscription;
    Subscription<PlayerNotificationEvent> level_up_event_subscription;
    Subscription<PlayerHitBottomEvent> player_hit_bottom_event_sub;
    [SerializeField]
    TextMeshProUGUI scoreText;
    [SerializeField]
    GameObject NotifcationGo;
    [SerializeField]
    GameObject RestartGo;
    [SerializeField]
    GameObject HomeGo;
    [SerializeField]
    float timeToMove;
    [SerializeField]
    Material[] LevelColorMaterialsList;
    [SerializeField]
    AudioClip levelUpSfx;
    [SerializeField]
    AudioClip errorSfx;
    [SerializeField]
    AudioClip DeathSfx;
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    ParticleSystem blood;

    bool HasDied;
    Vector3 notificationOrgPos;

    void Start()
    {
        HasDied = false;
        RestartGo.SetActive(false);
        HomeGo.SetActive(false);
        notificationOrgPos = NotifcationGo.transform.position;

        score_event_subscription = EventBus.Subscribe<ScoreEvent>(_OnScoreUpdated);
        level_up_event_subscription = EventBus.Subscribe<PlayerNotificationEvent>(_OnLevelUp);
        player_hit_bottom_event_sub = EventBus.Subscribe<PlayerHitBottomEvent>(_OnPlayerHitBottomEvent);


        // Inital level notification
        // EventBus.Publish<PlayerNotificationEvent>(new PlayerNotificationEvent("V0", playerInfo);
        // Publish level up event.
        //string level = "V" + PlayerInfo.Level.ToString();
        //Color levelColor = PlayerInfo.Instance.LevelToColor[level];
        //EventBus.Publish<PlayerNotificationEvent>(new PlayerNotificationEvent(level, levelColor));

    }

    // Count good rocks grabs
    void _OnScoreUpdated(ScoreEvent e)
    {
        scoreText.text = "Rocks : " + e.new_score;
    }

    // Notify player they moved up a level
    void _OnLevelUp(PlayerNotificationEvent e)
    {
        
        NotifcationGo.GetComponentInChildren<TextMeshProUGUI>().text = e.message;
        
        Debug.Log(e.message);
        NotifcationGo.GetComponent<Image>().color = e.messageColor;
        StartCoroutine(MoveLevelNotification(e.message));
       
    }

    // Display resetart button
    void _OnPlayerHitBottomEvent(PlayerHitBottomEvent e)
    {
        if (HasDied)
            return;

        if (!blood.isPlaying)
            blood.Play();

        PlayerInfo.Instance.disableMovement = true;

        audioSource.PlayOneShot(DeathSfx);
        RestartGo.SetActive(true);
        HomeGo.SetActive(true);

        HasDied = true;
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe(score_event_subscription);
    }


    // Move notification in and out of screen view
    IEnumerator MoveLevelNotification(string message)
    {
       
        float elapsedTime = 0;
        Vector3 orgPos = NotifcationGo.transform.position;
        Debug.Log(orgPos);
        Vector3 targPos = new Vector3(notificationOrgPos.x, notificationOrgPos.y - 125, notificationOrgPos.z);

        // Move Down
        while (elapsedTime < timeToMove)
        {
            NotifcationGo.transform.position = Vector3.Lerp(notificationOrgPos, targPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        NotifcationGo.transform.position = targPos;
        // play audio
        if (message == "Too far!")
            audioSource.PlayOneShot(errorSfx);
        else
            audioSource.PlayOneShot(levelUpSfx);

        yield return new WaitForSeconds(1.0f);

        // Move Up
        elapsedTime = 0;
        while (elapsedTime < timeToMove)
        {
            NotifcationGo.transform.position = Vector3.Lerp(targPos, notificationOrgPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        NotifcationGo.transform.position = notificationOrgPos;



    }
}
