using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestProgress
{
    ACCEPETABLE,
    ACCEPTED,
    COMPLETABLE,
    COMPLETE
}

public enum TaskType
{
    TALK,
    KILL,
    JustDoIt
}

[CreateAssetMenu(fileName = "Quest_", menuName = "Assets/Quest/Quest")]
public class Quest : ScriptableObject
{
    public string questName;
    [TextArea]
    public string questDescription;
    public QuestProgress questProgress;
    [Header("임무")]
    public Task[] tasks;
    [Header("보상")]
    public Reward rewards;
    public bool IsComplete
    {
        get
        {
            foreach ( Task task in tasks )
            {
                if ( !task.IsComplete )
                {
                    questProgress = QuestProgress.ACCEPTED;
                    return false;
                }
            }

            questProgress = QuestProgress.COMPLETABLE;
            return true;
        }
    }
    [Header("옵션")]
    [SerializeField]
    private bool isGiveUp;

    public bool IsGiveUp => isGiveUp;

    [System.Serializable]
    public class Task
    {
        public Quest quest;
        public GameObject taskTargetObj;
        public string taskTargetName;
        public string DisplayTargetName;
        public TaskType taskType;

        public int requiredAmount;
        public int currentAmount;

        public bool IsComplete
        {
            get { return currentAmount >= requiredAmount; }
        }

        public void TalkToNPCTarget( GameObject target, int count = 1 )
        {
            Debug.Log("TalkToNPCTarget");
            if ( target.name.Contains(this.taskTargetName) )
                currentAmount = Mathf.Clamp(currentAmount + count, 0, requiredAmount);

            QuestManager.Instance.CheckCompletion();
            QuestManager.Instance.UpdateSelected();
            QuestManager.Instance.GetQuestQuickSlot(this.quest).UpdateContent();
        }
        
        public void KillToMobTarget( GameObject target, int count = 1 )
        {
            Debug.Log("KillToMobTarget");
            if ( target.name.Contains(this.taskTargetName) )
                currentAmount = Mathf.Clamp(currentAmount + count, 0, requiredAmount);

            QuestManager.Instance.CheckCompletion();
            QuestManager.Instance.UpdateSelected();
            QuestManager.Instance.GetQuestQuickSlot(this.quest).UpdateContent();
        }

        public bool IsTarget( GameObject target )
        {
            if ( target.name.Contains(this.taskTargetName) || target.Equals(taskTargetObj) )
                return true;

            return false;
        }
    }
}

[System.Serializable]
public class Reward
{
    public List<Item> items;
    public int coin;

    public void Give()
    {
        foreach(var item in items )
        {
            if ( item.itemType == ItemType.Used )
            {
                Debug.Log($"퀘스트 보상 {item.itemName} 지급");
                Inventory.instance.AcquireItem(item, 5);
            }
            else
            {
                Debug.Log($"퀘스트 보상 {item.itemName} 지급");
                Inventory.instance.AcquireItem(item);
            }
        }

        Debug.Log($"퀘스트 보상 돈 : {coin} 지급");
        Inventory.instance.PlusCoin(coin);
    }
}