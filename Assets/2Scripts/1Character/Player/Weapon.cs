using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region WeaponSingleton
    public static Weapon instance;
    private void Awake()
    {
        if (instance == null) { instance = this; }
    }
    #endregion

    public int attackdamage;
    public float attackRate;
    public float skillRate;
    // public int CriValue;
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;
    public GameObject player;
    public GameObject skillEffect;
    public bool isAttack;
    public bool isSkill;


    public void UseAttack()
    {
        StopCoroutine(Swing());
        StartCoroutine(Swing());
    }

    public void UseSkill()
    {
        StopCoroutine(Skill());
        StartCoroutine(Skill());
    }

    // 일반 함수    : Use() 메인루틴 -> Swing() 서브루틴 -> Use() 메인루틴
    // 코루틴 함수 : Use() 메인루틴 + Swing() 코루틴

    IEnumerator Swing()
    {
        isAttack = true;
        yield return new WaitForSeconds(0.1f);
        trailEffect.enabled = true;

        yield return new WaitForSeconds(0.4f);
        SoundManager.Instance.PlaySound("Attack1");
        meleeArea.enabled = true;


        yield return new WaitForSeconds(0.2f);
        trailEffect.enabled = false;
        meleeArea.enabled = false;
        isAttack = false;
    }

    IEnumerator Skill()
    {
        isSkill = true;
        yield return new WaitForSeconds(0.5f);
        trailEffect.enabled = true;

        yield return new WaitForSeconds(1f);
        trailEffect.enabled = false;


        yield return new WaitForSeconds(0.5f);
        trailEffect.enabled = true;

        yield return new WaitForSeconds(0.2f);
        CreateSkillEffect();

        yield return new WaitForSeconds(0.6f);
        trailEffect.enabled = false;

        isSkill = false;
    }

    public void CreateSkillEffect()
    {
        GameObject prefab = Instantiate(skillEffect, player.transform.position, player.transform.rotation.normalized);
        Destroy(prefab, 1.8f);
    }
}
