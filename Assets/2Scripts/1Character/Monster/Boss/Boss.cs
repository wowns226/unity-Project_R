using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class Boss : Monster
{
    
    [SerializeField]
    private Transform target; // 쫓아갈 타겟
    [SerializeField]
    private BoxCollider meleeArea; // 공격 범위

    [SerializeField]
    private bool isChase; // 추적
    [SerializeField]
    private bool isAttack; // 공격

    [SerializeField]
    private Transform damagePos;
    [SerializeField]
    private GameObject damageTextPrefab;
    [SerializeField]
    private GameObject canvas;
    [SerializeField]
    private GameObject UIPanel;

    [SerializeField]
    private Image hpBar;
    [SerializeField]
    private Image hpBarDelay;

    [SerializeField]
    private TextMeshProUGUI nameText;

    [SerializeField]
    private Material mat;
    [SerializeField]
    private Animator ani;
    [SerializeField]
    private BoxCollider attackArea;

    [SerializeField]
    private UnityEvent ondie;

    [SerializeField]
    private GameObject meteorPrefab;

    private const string Idle = "Idle";
    private const string FlyIdle = "FlyIdle";
    private const string FlyMove = "FlyMove";
    private const string doHeadAttack = "doHeadAttack";
    private const string doWingAttack = "doWingAttack";
    private const string doFlameAttack = "doFlameAttack";

    [SerializeField]
    private float targetRadius = 1.5f;
    [SerializeField]
    private float targetRange = 10f;

    private void Awake()
    {
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        ani = GetComponent<Animator>();
    }

    private void Start()
    {
        nameText.text = this.Name;
    }

    private void OnEnable()
    {
        SoundManager.Instance.PlaySound("Boss_Growl");
        target = GameObject.FindGameObjectWithTag("Player").transform;
        UIPanel.SetActive(true);
        DisplayHp();
        ani.SetBool(Idle, true);
    }

    private void Update()
    {
        DisplayHp();

        Targeting();
    }

    private void LateUpdate()
    {
        if( isChase && !isAttack)
        {
            transform.LookAt(target);
        }
    }

    public void Targeting()
    {
        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));

        if ( rayHits.Length > 0 && !isAttack )
        {
            isChase = false;
            if ( hpBar.fillAmount < 0.4f )
            {
                StartCoroutine(Meteor());
            }
            else
            {
                StartCoroutine(Attack());
            }
        }
        else if ( rayHits.Length == 0 )
        {
            isChase = true;
        }
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, targetRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, targetRange);
    }*/

    public override void DisplayHp()
    {
        hpBar.fillAmount = (float)CurHp / (float)MaxHp;

        if ( hpBarDelay.fillAmount > hpBar.fillAmount )
        {
            hpBarDelay.fillAmount = Mathf.Lerp(hpBarDelay.fillAmount, hpBar.fillAmount, Time.deltaTime);
        }
    }

    public override IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.1f);
        isAttack = true;
        int attackNum = Random.Range(0, 4);
        switch ( attackNum )
        {
            case 0:
            case 1:
                // 머리 박치기
                StartCoroutine(HeadAttack());
                break;
            case 2:
            case 3:
                // 날개 찍기
                StartCoroutine(WingAttack());
                break;
        }
    }

    public IEnumerator HeadAttack()
    {
        ani.SetTrigger(doHeadAttack);

        yield return new WaitForSeconds(0.5f);

        attackArea.enabled = true;

        yield return new WaitForSeconds(1f);

        attackArea.enabled = false;
        isAttack = false;

        yield return null;
    }

    public IEnumerator WingAttack()
    {
        ani.SetTrigger(doWingAttack);
        yield return new WaitForSeconds(0.5f);

        attackArea.enabled = true;

        yield return new WaitForSeconds(1f);

        attackArea.enabled = false;
        isAttack = false;

        yield return null;
    }

    public IEnumerator Meteor()
    {
        ani.SetTrigger(doFlameAttack);
        isAttack = true;
        yield return new WaitForSeconds(2f);

        while ( true )
        {
            if ( hpBar.fillAmount <= 0 )
            {
                isAttack = false;
                yield break;
            }

            GameObject meteor = Instantiate<GameObject>(meteorPrefab, target.position, Quaternion.identity);
            SphereCollider collider = meteor.GetComponentInChildren<SphereCollider>();
            collider.enabled = false;

            yield return new WaitForSeconds(0.6f);

            collider.enabled = true;

            yield return new WaitForSeconds(1.3f);
            Destroy(meteor);

            yield return null;
        }
    }

    public override IEnumerator OnDamage()
    {
        SoundManager.Instance.PlaySound("Hit");
        mat.color = Color.yellow;

        yield return new WaitForSeconds(0.1f);

        if ( CurHp > 0 )
        {
            mat.color = Color.white;
        }
        else
        {
            StopCoroutine("OnDamage");
            StartCoroutine("Die");
        }
    }

    public override IEnumerator Die()
    {
        isChase = false;
        SoundManager.Instance.PlaySound("MonsterDie");
        isAttack = true;
        OnDie();
        mat.color = Color.gray;
        gameObject.layer = 13;
        canvas.gameObject.SetActive(false);

        ani.SetTrigger("doDie");

        yield return new WaitForSeconds(1.5f);

        while ( mat.color.a > 0 )
        {
            var color = mat.color;
            color.a -= 0.7f * Time.deltaTime;

            mat.color = color;
            yield return null;
        }

        UIPanel.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

    public override void OnDie()
    {
        ondie.Invoke();
    }

    private void OnTriggerEnter( Collider other )
    {
        if ( other.tag == "Melee" )
        {
            int damageAmount = other.GetComponent<Weapon>().attackdamage;

            CurHp -= damageAmount;

            GameObject damageText = Instantiate(damageTextPrefab, transform.position, Quaternion.identity, damagePos);
            damageText.GetComponent<DamagePopupText>().target = this.gameObject;
            damageText.GetComponent<DamagePopupText>().SetText(damageAmount);

            StartCoroutine(OnDamage());
        }

        else if ( other.tag == "Skill" )
        {
            int damageAmount = other.GetComponent<Skill>().damage;

            CurHp -= damageAmount;

            GameObject damageText = Instantiate(damageTextPrefab, transform.position, Quaternion.identity, damagePos);
            damageText.GetComponent<DamagePopupText>().target = this.gameObject;
            damageText.GetComponent<DamagePopupText>().SetText(damageAmount);

            StartCoroutine(OnDamage());
        }
    }

    public override void DropItem()
    {
        throw new System.NotImplementedException();
    }

    public override void DropCoin()
    {
        throw new System.NotImplementedException();
    }
}
