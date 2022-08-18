using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    public Quest[] quests;

    [SerializeField]
    private bool IsGiveQuest;

    private void OnTriggerStay( Collider other )
    {
        if ( Input.GetKeyDown(KeyCode.F) && other.gameObject.CompareTag("Player") && !IsGiveQuest)
        {
            foreach ( Quest quest in quests )
            {
                QuestManager.Instance.AddQuest(quest);
            }

            IsGiveQuest = true;
        }
    }
}
