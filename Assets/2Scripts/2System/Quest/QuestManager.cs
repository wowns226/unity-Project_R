using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class QuestManager : MonoBehaviour
{
    public static bool questActivated = false;

    #region QuestManager SingleTon
    public static QuestManager Instance;

    private void Awake()
    {
        if ( Instance == null)
        {
            Instance = this;
        }
    }
    #endregion

    [Header("Quest List Data")]
    [SerializeField]
    private List<QuestSlot> questSlots;
    [SerializeField]
    private List<QuestQuickSlot> questQuickSlots;

    [SerializeField]
    private List<Quest> activeQuestLists; // 진행중인 퀘스트 리스트
    [SerializeField]
    private List<Quest> completeQuestLists; // 완료된 퀘스트 리스트

    [Header("GameObject Data")]
    [SerializeField]
    private GameObject go_questSlotPrefab;
    [SerializeField]
    private GameObject go_questQuickSlotPrefab;
    [SerializeField]
    private GameObject go_questPanel;
    [SerializeField]
    private GameObject go_quickQuestListPanel;

    [Header("GameObject Parent")]
    [SerializeField]
    private GameObject go_questSlotsParent;
    [SerializeField]
    private GameObject go_questQuickSlotsParent;

    [Header("Quest UI Data")]
    [SerializeField]
    private TextMeshProUGUI textQuestName;
    [SerializeField]
    private TextMeshProUGUI textQuestDescription;

    [Header("Task UI Data")]
    [SerializeField]
    public GameObject[] taskInfoArray;
    [SerializeField]
    private TextMeshProUGUI questTaskTitleText;

    [Header("Reward UI Data")]
    [SerializeField]
    public GameObject[] rewardInfoArray;
    [SerializeField]
    public TextMeshProUGUI rewardInfoText;
    [SerializeField]
    public TextMeshProUGUI rewardExpCoinInfo;

    [Space]
    [SerializeField]
    private QuestSlot selectedSlot;
    [SerializeField]
    private bool isQuestControlButtonClick;
    

    [Space]
    [SerializeField]
    private Image quickQuestControlButton;
    [Header("Quest Data")]
    [SerializeField]
    private Quest[] quests;

    public IReadOnlyList<Quest> ActiveQuestLists => activeQuestLists;
    public IReadOnlyList<Quest> CompleteQuestLists => completeQuestLists;

    private void Start()
    {
        Clear();

        foreach ( Quest quest in quests )
            AddQuest(quest);
    }

    private void Update()
    {
        TryOpenQuestUI();
    }

    private void TryOpenQuestUI()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            questActivated = !go_questPanel.activeSelf;
            if ( questActivated ) OpenQuest();
            else CloseQuest();
        }
    }

    public void OpenQuest()
    {
        questActivated = true;
        go_questPanel.SetActive(true);
    }

    public void CloseQuest() 
    {
        questActivated = false;
        go_questPanel.SetActive(false);
    }

    public QuestSlot GetQuestSlot( Quest _quest ) 
    { 
        foreach( QuestSlot slot in questSlots )
        {
            if ( slot.quest.Equals(_quest) )
                return slot;
        }

        return null;
    }

    public QuestQuickSlot GetQuestQuickSlot( Quest _quest ) 
    {
        foreach ( QuestQuickSlot slot in questQuickSlots )
        {
            if ( slot.quest.Equals(_quest) )
                return slot;
        }

        return null;
    }

    public void AddQuest(Quest _quest)
    {
        activeQuestLists.Add(_quest);

        GameObject obj_QuestSlot = Instantiate(go_questSlotPrefab, go_questSlotsParent.transform);
        QuestSlot nowQuestSlot = obj_QuestSlot.GetComponent<QuestSlot>();
        nowQuestSlot.quest = _quest;
        nowQuestSlot.Init();

        GameObject obj_QuestQuickSlot = Instantiate(go_questQuickSlotPrefab, go_questQuickSlotsParent.transform);
        QuestQuickSlot nowQuestQuickSlot = obj_QuestQuickSlot.GetComponent<QuestQuickSlot>();
        nowQuestQuickSlot.quest = _quest;
        nowQuestQuickSlot.Init();

        questSlots.Add(nowQuestSlot);
        questQuickSlots.Add(nowQuestQuickSlot);
    }

    public void RemoveQuest(Quest _quest )
    {
        activeQuestLists.Remove(_quest);
        completeQuestLists.Add(_quest);

        questSlots.Remove(GetQuestSlot(_quest));
        questQuickSlots.Remove(GetQuestQuickSlot(_quest));
    }

    public void UpdateSelected()
    {
        ShowQuestContent(selectedSlot);
    }

    public void ShowQuestContent(QuestSlot slot)
    {
        if ( slot == selectedSlot )
            return;

        if ( selectedSlot != null)
            selectedSlot.Selected = false;

        selectedSlot = slot;
        textQuestName.text = $"{slot.quest.questName}";
        textQuestDescription.text = $"{slot.quest.questDescription}";

        ShowQuestGoal();
        ShowRewardItem();
    }

    public void ShowQuestGoal()
    {
        for ( int i = 0 ; i < taskInfoArray.Length ; i++ )
            taskInfoArray[i].SetActive(false);

        if ( selectedSlot == null)
            return;

        Quest.Task[] tasks = selectedSlot.quest.tasks;

        questTaskTitleText.text = "퀘스트 현황";

        for ( int i = 0 ; i < tasks.Length ; i++ ) 
        {
            taskInfoArray[i].SetActive(true);

            TextMeshProUGUI taskText = taskInfoArray[i].GetComponent<TextMeshProUGUI>();

            if ( tasks[i].taskType == TaskType.TALK )
            {
                taskText.text = tasks[i].DisplayTargetName + "에게 말걸기" + "     " + tasks[i].currentAmount + " / " + tasks[i].requiredAmount;
            }
            else if ( tasks[i].taskType == TaskType.KILL )
            {
                taskText.text = tasks[i].DisplayTargetName + "     " + tasks[i].currentAmount + " / " + tasks[i].requiredAmount;
            }
            else
            {
                taskText.text = tasks[i].DisplayTargetName;
            }
        }
    }

    public void ShowRewardItem()
    {
        for (int i = 0; i < rewardInfoArray.Length; i++)
            rewardInfoArray[i].SetActive(false);

        if ( selectedSlot == null)
            return;
        
        Reward reward = selectedSlot.quest.rewards;
        
        rewardInfoText.text = "보상";
        for (int i = 0; i < selectedSlot.quest.rewards.items.Count; i++)
        {
            if (reward.items == null)
                continue;

            rewardInfoArray[i].SetActive(true);
            if (reward.items[i].itemType == ItemType.Equipment)
            {
                Image rewardItemImg = rewardInfoArray[i].transform.GetChild(0).GetComponent<Image>();
                rewardItemImg.sprite = reward.items[i].itemImage;
                Color alp = rewardItemImg.color;
                alp.a = 1f;
                rewardItemImg.color = alp;

                TextMeshProUGUI rewardItemText = rewardInfoArray[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                rewardItemText.text = reward.items[i].itemName;
            }
            else
            {
                Image rewardItemImg = rewardInfoArray[i].transform.GetChild(0).GetComponent<Image>();
                rewardItemImg.sprite = reward.items[i].itemImage;

                TextMeshProUGUI rewardItemText = rewardInfoArray[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                rewardItemText.text = reward.items[i].itemName + "\n5개";
            }
        }
        

        
        rewardExpCoinInfo.text = $"코인 : {reward.coin}";
    }

    public void Clear()
    {
        if ( selectedSlot != null )
        {
            selectedSlot.Selected = false;
            selectedSlot = null;
        }

        textQuestName.text = "";
        textQuestDescription.text = "";
        questTaskTitleText.text = "";
        foreach (var slot in taskInfoArray )
        {
            slot.SetActive(false);
        }
        rewardInfoText.text = "";
        foreach ( var slot in rewardInfoArray )
        {
            slot.SetActive(false);
        }
        rewardExpCoinInfo.text = "";
    }

    public void onRewardBtn()
    {
        Debug.Log("보상 버튼 클릭");

        if ( selectedSlot == null )
            return;

        if ( selectedSlot.quest.IsComplete && selectedSlot.quest.questProgress.Equals(QuestProgress.COMPLETABLE) && !selectedSlot.quest.IsGiveUp) 
        {
            NotifyText.Instance.SetText($"<color=green>{selectedSlot.quest.questName}</color>의 보상을 획득하셨습니다");

            selectedSlot.quest.rewards.Give();
            SoundManager.Instance.PlaySound("QuestClear");

            QuestQuickSlot quickSlot = GetQuestQuickSlot(selectedSlot.quest);

            RemoveQuest(selectedSlot.quest);
            Destroy(selectedSlot.gameObject);
            Destroy(quickSlot.gameObject);
            Clear();
        } 
    }

    public void GiveupBtn()
    {
        if ( selectedSlot == null )
            return;

        if ( !selectedSlot.quest.IsGiveUp )
            return;

        NotifyText.Instance.SetText($"<color=green>{selectedSlot.quest.questName}</color>을 포기하셨습니다");

        QuestQuickSlot quickSlot = GetQuestQuickSlot(selectedSlot.quest);

        RemoveQuest(selectedSlot.quest);
        Destroy(selectedSlot.gameObject);
        Destroy(quickSlot.gameObject);
        Clear();
    }

    public void CheckCompletion()
    {
        foreach(QuestSlot questSlot in questSlots )
        {
            questSlot.IsComplete();
        }
    }

    public Quest GetActiveQuestList( Quest _quest )
    {
        foreach ( Quest quest in activeQuestLists )
        {
            if ( quest == _quest )
                return _quest;
        }

        return null;
    }

    public void onClickedControlButton()
    {
        isQuestControlButtonClick = !isQuestControlButtonClick;
        go_quickQuestListPanel.SetActive(isQuestControlButtonClick);
        if (isQuestControlButtonClick)
        {
            quickQuestControlButton.sprite = Resources.Load<Sprite>("Icon/pointer1");
        }
        else
        {
            quickQuestControlButton.sprite = Resources.Load<Sprite>("Icon/pointer2");
        }
    }

    public void onClickedQuestButton()
    {
        questActivated = true;
        if ( questActivated ) OpenQuest();
        else CloseQuest();
    }
}
