using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpOnTrigger : MonoBehaviour
{
    bool hasBeenTouched = false;
    private void OnTriggerEnter(Collider other)
    {
        if (hasBeenTouched)
            return;

        hasBeenTouched = true;
        // Increase level.
        PlayerInfo.Instance.ModifyLevel(1);
        // Publish level up event.
      

        EventBus.Publish<LevelUpEvent>(new LevelUpEvent("V" + PlayerInfo.Instance.Level.ToString()));
       

    }
}

public class LevelUpEvent
{
    public string level = "V0";
    public LevelUpEvent(string _level) { level = _level; }
}