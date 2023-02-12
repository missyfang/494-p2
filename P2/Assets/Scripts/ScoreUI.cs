using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    Subscription<ScoreEvent> score_event_subscription;
    Subscription<LevelUpEvent> level_up_event_subscription;
    [SerializeField]
    TextMeshProUGUI scoreText;
    [SerializeField]
    GameObject NotifcationText;
    [SerializeField]
    float timeToMove;
    [SerializeField]
    Material[] LevelColorMaterialsList;

    void Start()
    {
        score_event_subscription = EventBus.Subscribe<ScoreEvent>(_OnScoreUpdated);
        level_up_event_subscription = EventBus.Subscribe<LevelUpEvent>(_OnLevelUp);
        StartCoroutine(Move());
    }

    void _OnScoreUpdated(ScoreEvent e)
    {
        scoreText.text = "Rocks : " + e.new_score;
    }

    void _OnLevelUp(LevelUpEvent e)
    {
        NotifcationText.GetComponentInChildren<TextMeshProUGUI>().text ="Level Up! " + e.level;
        Debug.Log(e.level);
        NotifcationText.GetComponent<Image>().color = PlayerInfo.Instance.LevelToColor[e.level];
        StartCoroutine(Move());
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe(score_event_subscription);
    }

    IEnumerator Move()
    {
       
        float elapsedTime = 0;
        Vector3 orgPos = NotifcationText.transform.position;
        Debug.Log(orgPos);
        Vector3 targPos = new Vector3(orgPos.x, orgPos.y - 100, orgPos.z);

        // Move Down
        while (elapsedTime < timeToMove)
        {
            NotifcationText.transform.position = Vector3.Lerp(orgPos, targPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        NotifcationText.transform.position = targPos;
        yield return new WaitForSeconds(3.0f);

        // Move Up
        elapsedTime = 0;
        while (elapsedTime < timeToMove)
        {
            NotifcationText.transform.position = Vector3.Lerp(targPos, orgPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        NotifcationText.transform.position = orgPos;



    }
}
