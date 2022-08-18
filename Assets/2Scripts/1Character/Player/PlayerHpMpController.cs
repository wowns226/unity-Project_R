using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHpMpController : MonoBehaviour
{
    [Header("플레이어 체력 관련 컴포넌트")]
    public Image playerCurHp;
    public Image playerDelayHp;
    public Image playerCurMp;
    public Image playerDelayMp;

    [SerializeField]
    private TextMeshProUGUI hpText;
    [SerializeField]
    private TextMeshProUGUI mpText;

    private void Start()
    {
        InvokeRepeating("PlayerHeal", 1f, 2f);
    }

    void Update()
    {
        hpText.text = $"{Player.instance.curhealth} / {Player.instance.maxhealth}";
        mpText.text = $"{Player.instance.curMana} / {Player.instance.maxMana}";

        PlayerHpUpdate();
        PlayerMpUpdate();
    }

    public void PlayerHeal()
    {
        if (Player.instance.curhealth < Player.instance.maxhealth )
        {
            Player.instance.curhealth += 1;
        }

        if( Player.instance.curMana < Player.instance.maxMana )
        {
            Player.instance.curMana += 1;
        }
    }

    public void PlayerHpUpdate()
    {
        if ( Player.instance.curhealth == Player.instance.maxhealth )
        {
            playerCurHp.fillAmount = 1f;
        }
        else
        {
            playerCurHp.fillAmount = (float)Player.instance.curhealth / (float)Player.instance.maxhealth;
        }

        if ( playerDelayHp.fillAmount > playerCurHp.fillAmount )
        {
            playerDelayHp.fillAmount = Mathf.Lerp(playerDelayHp.fillAmount, playerCurHp.fillAmount, Time.deltaTime);
        }
        else
        {
            playerDelayHp.fillAmount = playerCurHp.fillAmount;
        }
    }

    public void PlayerMpUpdate()
    {
        if ( Player.instance.curMana == Player.instance.maxMana )
        {
            playerCurMp.fillAmount = 1f;
        }
        else
        {
            playerCurMp.fillAmount = (float)Player.instance.curMana / (float)Player.instance.maxMana;
        }

        if ( playerDelayMp.fillAmount > playerCurMp.fillAmount )
        {
            playerDelayMp.fillAmount = Mathf.Lerp(playerDelayMp.fillAmount, playerCurMp.fillAmount, Time.deltaTime);
        }
        else
        {
            playerDelayMp.fillAmount = playerCurMp.fillAmount;
        }
    }
}
