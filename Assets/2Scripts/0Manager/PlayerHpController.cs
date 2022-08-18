using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHpController : MonoBehaviour
{
    [Header("플레이어 체력 관련 컴포넌트")]
    public Image playerCurHp;
    public Image playerDelayHp;
    // public Image playerCurMp;
    // public Image playerCurExp;

    void Update()
    {
        PlayerHpUpdate();
    }
    public void PlayerHpUpdate()
    {
        if (Player.instance.curhealth == Player.instance.maxhealth)
        {
            playerCurHp.fillAmount = 1f;
        }
        else
        {
            playerCurHp.fillAmount = (float)Player.instance.curhealth / (float)Player.instance.maxhealth;
        }

        if (playerDelayHp.fillAmount > playerCurHp.fillAmount)
        {
            playerDelayHp.fillAmount = Mathf.Lerp(playerDelayHp.fillAmount, playerCurHp.fillAmount, Time.deltaTime);
        }
        else
        {
            playerDelayHp.fillAmount = playerCurHp.fillAmount;
        }

    }
}
