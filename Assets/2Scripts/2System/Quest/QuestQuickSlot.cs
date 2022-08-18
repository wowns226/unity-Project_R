using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestQuickSlot : MonoBehaviour
{
    public Quest quest;

    [Header("Text")]
    [SerializeField]
    private TextMeshProUGUI questTitleText;
    [SerializeField]
    private TextMeshProUGUI questDescription;
    [SerializeField]
    private TextMeshProUGUI questState;

    [Header("Color")]
    [SerializeField]
    private Color activedColor;
    [SerializeField]
    private Color completedColor;

    public void Init()
    {
        questTitleText.text = quest.questName;
        foreach(Quest.Task task in quest.tasks )
        {
            if(task.taskType == TaskType.JustDoIt )
            {
                questDescription.text = $"● {task.DisplayTargetName}";
            }
            else if(task.taskType == TaskType.TALK )
            {
                questDescription.text = $"● {task.DisplayTargetName}";
                questDescription.text += "와 대화";
            }
            else
            {
                questDescription.text = $"● {task.DisplayTargetName}";
                questDescription.text += $"  {task.currentAmount} / {task.requiredAmount}";
            }
        }
    }

    public void UpdateContent()
    {
        foreach ( Quest.Task task in quest.tasks )
        {
            if ( task.taskType == TaskType.JustDoIt )
            {
                questDescription.text = $"● {task.DisplayTargetName}";
            }
            else if ( task.taskType == TaskType.TALK )
            {
                questDescription.text = $"● {task.DisplayTargetName}";
                questDescription.text += "와 대화";
            }
            else
            {
                questDescription.text = $"● {task.DisplayTargetName}";
                questDescription.text += $"  {task.currentAmount} / {task.requiredAmount}";
            }

            if ( task.IsComplete )
            {
                questState.text = "(완료)";
                questState.color = completedColor;
                questTitleText.color = completedColor;
                questDescription.color = completedColor;
            }
        }
    }
}
