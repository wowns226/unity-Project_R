using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestReport : MonoBehaviour
{
    [SerializeField]
    private Quest quest;
    [SerializeField]
    private int successCount;

    public void Report()
    {
        Quest q = QuestManager.Instance.GetActiveQuestList(quest);

        if(q != null )
        {
            foreach ( var task in q.tasks )
            {
                if ( task.IsTarget(this.gameObject) )
                {
                    if ( task.taskType == TaskType.TALK )
                    {
                        task.TalkToNPCTarget(this.gameObject);
                    }
                    else if ( task.taskType == TaskType.KILL )
                    {
                        task.KillToMobTarget(this.gameObject);
                    }
                }
            }

            QuestManager.Instance.UpdateSelected();
        }
        
    }
}
