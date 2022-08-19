using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
	
    public static bool dialogueActivated = false;

    #region Dialogue SingleTon
    public static Dialogue Instance;
    private void Awake()
    {
        if( Instance == null)
        {
            Instance = this;
        }
    }
    #endregion
    [Header("UI Object")]
    public GameObject go_dialoguePanel;
    public GameObject go_NextButton;
    public GameObject go_AcceptButton;
    public TextMeshProUGUI dialogueText;

    [Space]
    public Queue<string> sentences;
    public string currentSentence;
    [Range(0f, 1f)]
    public float typingSpeed = 0.1f;

    [Space]
    public bool isTalking;
    public bool isTyping;
    public bool isNext;

    private void Start()
    {
        sentences = new Queue<string>();
        isTalking = false;
        isNext = true;
    }

    private void Update()
    {
        if (!isTyping)
        {
            NextSentence();
        }

        if (dialogueText.text.Equals(currentSentence))
        {
            isTyping = false;
        }

        if (sentences.Count == 0)
        {
            go_NextButton.SetActive(false);
            go_AcceptButton.SetActive(true);
        }
        else
        {
            go_NextButton.SetActive(true);
            go_AcceptButton.SetActive(false);
        }
    }

    public void OnDialogue(string[] lines)
    {
        sentences.Clear();
        foreach(string line in lines)
        {
            sentences.Enqueue(line);
        }
        go_dialoguePanel.SetActive(true);

        dialogueActivated = true;
    }

    public void NextSentence()
    {
        if (sentences.Count != 0) 
        {
            if (isNext)
            {
                currentSentence = sentences.Dequeue();
                isTyping = true;
                StartCoroutine(Typing(currentSentence));
                isNext = false;
            }
        }
        else
        {
            isTyping = false;
            isTalking = false;
        }
    }

    IEnumerator Typing(string line)
    {
        dialogueText.text = "";
        foreach(char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void DialogueReset()
    {
        sentences.Clear();
        currentSentence = null;
        isTyping = false;
        isTalking = false;
        isNext = true;
        go_dialoguePanel.SetActive(false);
        go_NextButton.SetActive(true);
        go_AcceptButton.SetActive(false);

        dialogueActivated = false;
    }

    public void OnClickedNextButton()
    {
        isNext = true;
    }

    public void OnClickedQuitButton()
    {
        DialogueReset();
    }

    public void OnClickedAcceptButton()
    {
        DialogueReset();
    }
}