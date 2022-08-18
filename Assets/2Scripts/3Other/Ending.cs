using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Ending : MonoBehaviour
{
    public static Ending Instance;
    private void Awake()
    {
        if ( Instance == null )
            Instance = this;
    }

    [SerializeField]
    private GameObject EndingPanel;
    [SerializeField]
    private GameObject EndingTitle;
    [SerializeField]
    private TextMeshProUGUI respawnText;
    [SerializeField]
    private Transform startPos;
    [SerializeField]
    private float timer = 5f;

    public GameObject Go_EndingPanel => EndingPanel;

    public void PlayerDie()
    {
        Player.instance.nav.enabled = false;
        EndingPanel.SetActive(true);
        respawnText.gameObject.SetActive(true);


        StartCoroutine(PlayerDieCoroutine());
        
    }

    public IEnumerator PlayerDieCoroutine()
    {
        while ( timer > 0 )
        {
            //timer = Mathf.Clamp(timer -= Time.deltaTime, 0f, 10f);
            timer -= Time.deltaTime;

            respawnText.text = $"<color=green><color=red>{(int)timer}</color>초 후에 마을에서 부활합니다</color>";

            if ( timer <= 0 )
            {
                RespawnPlayer();
                yield break;
            }

            yield return null;
        }

        yield return null;
    }

    public void RespawnPlayer()
    {
        Player.instance.anim.SetBool("isDie", false);
        Player.instance.gameObject.transform.position = startPos.position;
        Player.instance.isDamage = false;
        Player.instance.mesh.material.color = Color.white;
        Player.instance.curhealth = Player.instance.maxhealth;
        Player.instance.curMana = Player.instance.maxMana;
        Player.instance.isDie = false;
        Player.instance.nav.enabled = true;
        timer = 5f;
        respawnText.gameObject.SetActive(false);
        EndingPanel.SetActive(false);
    }
}