using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopNPC : MonoBehaviour
{
    [Header("NPC Info")]
    public string npcName;
    public string npcJob;

    [Space]
    [Header("NPC Dialogue Info")]
    public bool isTalk;

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

    private void Start()
    {
        isTalk = false;
        npcNameText.text = npcName;
        npcJobText.text = "<color=green> <" + npcJob + "> </color>";
    }

    private void Update()
    {
        ShowNpcInfo();
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if (!isTalk)
            {
                Interact();

                if (Input.GetKeyDown(KeyCode.F))
                {
                    DisInteract();
                    ShopOpen();
                    isTalk = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            isTalk = false;
            DisInteract();
        }
    }

    public void ShowNpcInfo()
    {
        go_npcText.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 10f, 0));
    }

    public void Interact()
    {
        // go_playerInteract.SetActive(true);
        npcInteractImage.SetActive(true);
        npcInteractImage.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 15f, 0));
        npcInteractText.text = "<color=red>" + "[F]" + "</color>" + "를 눌러 상호작용";
    }

    public void DisInteract()
    {
        npcInteractText.text = "";
        Shop.instance.CloseShop();
        npcInteractImage.SetActive(false);
        Inventory.instance.Go_Inventory.SetActive(false);
    }

    public void ShopOpen()
    {
        Shop.instance.OpenShop();
        Inventory.instance.Go_Inventory.SetActive(true);
    }
}
