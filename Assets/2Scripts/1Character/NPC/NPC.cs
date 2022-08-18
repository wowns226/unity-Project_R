using UnityEngine;
using UnityEngine.Events;
using TMPro;

public enum NPCState
{
    None,
    InQuest,
    CompleteQuest
}

public class NPC : MonoBehaviour
{
    [Header("NPC Info")]
    public int npcID;
    public string npcName;
    public string npcJob;
    [SerializeField]
    private NPCState npcState = NPCState.None;
    [SerializeField]
    private UnityEvent onTalk;
    [SerializeField]
    private bool IsQuestStart;

    [Space]
    [Header("NPC Dialogue Info")]
    [SerializeField]
    private string[] noneStateSentence;
    [SerializeField]
    private string[] inQuestStateSentence;
    [SerializeField]
    private string[] completeQuestStateSentence;


    [Space]
    [Header("NPC GUI")]
    public GameObject go_npcText;
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI npcJobText;
    public GameObject npcInteractImage;

    [Space]
    [Header("Player GUI")]
    public GameObject go_playerInteract;
    public TextMeshProUGUI npcInteractText;

    [Space]
    [Header("Dialogue GUI")]
    public GameObject go_dialogue;
    public TextMeshProUGUI dialogueNpcName;
    public TextMeshProUGUI dialogueNpcSentence;

    [Space]
    [Header("Quest Info")]
    [SerializeField]
    private Quest quest;

    private void Start()
    {
        npcNameText.text = npcName;
        npcJobText.text = "<color=green> <" + npcJob + "> </color>";
        dialogueNpcSentence.text = "";
    }

    private void Update()
    {
        ShowNpcInfo();
    }

    private void OnTriggerStay( Collider other )
    {

        if ( other.transform.CompareTag("Player") )
        {
            if ( !Dialogue.Instance.isTalking )
            {
                Interact();

                if ( Input.GetKeyDown(KeyCode.F) )
                {
                    DisInteract();
                    InputF();

                    if ( !IsQuestStart ) 
                    {
                        Dialogue.Instance.OnDialogue(noneStateSentence);
                        Dialogue.Instance.isTalking = true;

                        IsQuestStart = true;
                    }
                    else
                    {
                        Quest q = QuestManager.Instance.GetActiveQuestList(quest);
                        if ( q != null )
                        {

                            switch ( q.questProgress )
                            {
                                case QuestProgress.ACCEPTED:
                                    npcState = NPCState.InQuest;
                                    break;
                                case QuestProgress.COMPLETABLE:
                                    npcState = NPCState.CompleteQuest;
                                    break;
                                default:
                                    break;
                            }
                        }
                        Dialogue.Instance.OnDialogue(GetSentense());
                        Dialogue.Instance.isTalking = true;
                    }
                }
            }
        }
    }

    private void OnTriggerExit( Collider other )
    {
        if ( other.transform.CompareTag("Player") )
        {
            Dialogue.Instance.isTalking = false;
            DisInteract();
            Dialogue.Instance.DialogueReset();
        }
    }

    public void ShowNpcInfo()
    {
        go_npcText.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 10f, 0));
    }

    private void Interact()
    {
        go_playerInteract.SetActive(true);
        npcInteractImage.SetActive(true);
        npcInteractImage.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 15f, 0));
        npcInteractText.text = "<color=red>" + "[F]" + "</color>" + "를 눌러 상호작용";
    }

    private void DisInteract()
    {
        npcInteractText.text = "";
        npcInteractImage.SetActive(false);
        dialogueNpcSentence.text = "";
        go_dialogue.SetActive(false);
    }

    private void InputF()
    {
        onTalk.Invoke();

        dialogueNpcName.text = npcName;
        go_dialogue.SetActive(true);
        npcState = NPCState.InQuest;
    }

    public string[] GetSentense()
    {
        
        switch ( npcState )
        {
            case NPCState.None:
                return noneStateSentence;
            case NPCState.InQuest:
                return inQuestStateSentence;
            case NPCState.CompleteQuest:
                return completeQuestStateSentence;
            default :
                return null;
        }
    }
}
