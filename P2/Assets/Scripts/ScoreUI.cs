using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    Subscription<ScoreEvent> score_event_subscription;
    [SerializeField]
    TextMeshProUGUI checkPointText;


    void Start()
    {
        score_event_subscription = EventBus.Subscribe<ScoreEvent>(_OnScoreUpdated);
    }

    void _OnScoreUpdated(ScoreEvent e)
    {
        checkPointText.text = "Checkpoints : " + e.new_score;
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe(score_event_subscription);
    }
}
