using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class QuestSlot : MonoBehaviour,IPointerClickHandler
{
    public Quest quest;

    [SerializeField]
    private TextMeshProUGUI textQuestName;
    [SerializeField]
    private GameObject go_questDetailUI;

    [SerializeField]
    private bool selected;
    [SerializeField]
    private bool completed;

    public bool Selected
    {
        get { return selected; }
        set
        {
            if (value)
            {
                textQuestName.color = Color.red;
                Debug.Log("»¡°­");
            }

            else
            {
                textQuestName.color = Color.white;
                Debug.Log("Èò»ö");

                if ( quest.IsComplete )
                {
                    textQuestName.color = Color.green;
                    Debug.Log("ÃÊ·Ï»ö");
                }
            }
                
        }
    }

    public void Init()
    {
        quest.questProgress = QuestProgress.ACCEPTED; 

        foreach(Quest.Task task in quest.tasks )
        {
            task.quest = this.quest;
        }

        textQuestName.text = quest.questName;

        Selected = false;
    }

    public void OnPointerClick( PointerEventData eventData )
    {
        onSlotClicked();
    }


    public void onSlotClicked()
    {
        if (!Selected)
        {
            Selected = true;
            QuestManager.Instance.ShowQuestContent(this);
        }
    }

    public void RemoveQuestSlot()
    {
        quest = null;
        this.gameObject.SetActive(false);
    }

    public void IsComplete()
    {
        if ( quest.IsComplete && !completed )
        {
            quest.questProgress = QuestProgress.COMPLETABLE;
            textQuestName.color = Color.green;
            textQuestName.text += " (¿Ï·á)";
            completed = true;
        }
    }
}
