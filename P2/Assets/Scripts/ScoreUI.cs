using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    Subscription<ScoreEvent> score_event_subscription;
    Subscription<LevelUpEvent> level_up_event_subscription;
    Subscription<PlayerHitBottomEvent> player_hit_bottom_event_sub;
    [SerializeField]
    TextMeshProUGUI scoreText;
    [SerializeField]
    GameObject NotifcationGo;
    [SerializeField]
    GameObject DeathGo;
    [SerializeField]
    float timeToMove;
    [SerializeField]
    Material[] LevelColorMaterialsList;

    void Start()
    {
        DeathGo.SetActive(false);
        score_event_subscription = EventBus.Subscribe<ScoreEvent>(_OnScoreUpdated);
        level_up_event_subscription = EventBus.Subscribe<LevelUpEvent>(_OnLevelUp);
        player_hit_bottom_event_sub = EventBus.Subscribe<PlayerHitBottomEvent>(_OnPlayerHitBottomEvent);
        StartCoroutine(MoveLevelNotification());
    }

    // Count good rocks grabs
    void _OnScoreUpdated(ScoreEvent e)
    {
        scoreText.text = "Rocks : " + e.new_score;
    }

    // Notify player they moved up a level
    void _OnLevelUp(LevelUpEvent e)
    {
        NotifcationGo.GetComponentInChildren<TextMeshProUGUI>().text ="Level Up! " + e.level;
        Debug.Log(e.level);
        NotifcationGo.GetComponent<Image>().color = PlayerInfo.Instance.LevelToColor[e.level];
        StartCoroutine(MoveLevelNotification());
    }

    // Display resetart button
    void _OnPlayerHitBottomEvent(PlayerHitBottomEvent e)
    {
        DeathGo.SetActive(true);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe(score_event_subscription);
    }


    // Move notification in and out of screen view
    IEnumerator MoveLevelNotification()
    {
       
        float elapsedTime = 0;
        Vector3 orgPos = NotifcationGo.transform.position;
        Debug.Log(orgPos);
        Vector3 targPos = new Vector3(orgPos.x, orgPos.y - 115, orgPos.z);

        // Move Down
        while (elapsedTime < timeToMove)
        {
            NotifcationGo.transform.position = Vector3.Lerp(orgPos, targPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        NotifcationGo.transform.position = targPos;
        yield return new WaitForSeconds(0.5f);

        // Move Up
        elapsedTime = 0;
        while (elapsedTime < timeToMove)
        {
            NotifcationGo.transform.position = Vector3.Lerp(targPos, orgPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        NotifcationGo.transform.position = orgPos;



    }
}
