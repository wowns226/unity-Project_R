using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.Events;
using TMPro;

public class Grunt : Monster
{
    public Transform target; // 쫓아갈 타겟

    public BoxCollider meleeArea; // 공격 범위

    public bool isChase; // 추적
    public bool isAttack; // 공격

    public Transform damagePos;
    public GameObject damageTextPrefab;
    public GameObject canvas;
    public GameObject UIPanel;

    public Image hpBar;
    public Image hpBarDelay;

    public Item[] dropItems;

    public TextMeshProUGUI nametext;

    private Rigidbody rigid;
    private Material mat;
    private NavMeshAgent nav;
    private Animator ani;

    [SerializeField]
    private UnityEvent ondie;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();

        Invoke("ChaseStart", 1f);
    }

    void Start()
    {
        nametext.text = Name;
        UIPanel.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void ChaseStart()
    {
        isChase = true;
        ani.SetBool("isWalk", true);
    }

    void Update()
    {
        if (nav.enabled && isChase && !isAttack)
        {
            nav.SetDestination(target.position);
            nav.isStopped = !isChase;
        }
        
        Targeting();
        DisplayHp();
    }

    public override void DisplayHp()
    {
        if (!UIPanel.gameObject.activeSelf && hpBar.fillAmount > 0 && hpBar.fillAmount < 1f)
        {
            UIPanel.gameObject.SetActive(true);
        }

        Vector3 hpBarBgPos = Camera.main.WorldToScreenPoint(transform.position) + new Vector3(0, 20f, 0);

        UIPanel.transform.position = hpBarBgPos;

        hpBar.fillAmount = (float)CurHp / (float)MaxHp;

        if (hpBarDelay.fillAmount > hpBar.fillAmount)
        {
            hpBarDelay.fillAmount = Mathf.Lerp(hpBarDelay.fillAmount, hpBar.fillAmount, Time.deltaTime);
        }
    }
    void FreezeVelocity()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }

    void Targeting()
    {
        float targetRadius = 2f;
        float targetRange = 5f;

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));

        if (rayHits.Length > 0 && !isAttack)
        {
            ani.SetBool("isWalk", false);
            StartCoroutine(Attack());
        }
        else if (rayHits.Length == 0)
        {
            isChase = true;
            ani.SetBool("isWalk", true);
        }
    }

    void FixedUpdate()
    {
        FreezeVelocity();
    }

    public override IEnumerator Attack()
    {
        isChase = false;
        isAttack = true;
        ani.SetBool("isAttack", true);
        ani.SetBool("isWalk", false);

        yield return new WaitForSeconds(0.8f);
        meleeArea.enabled = true;

        yield return new WaitForSeconds(0.3f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.3f);
        ani.SetBool("isAttack", false);

        yield return new WaitForSeconds(1f);
        isAttack = false;
        isChase = true;
        ani.SetBool("isWalk", true);
    }

    public override IEnumerator OnDamage()
    {
        StopCoroutine(Attack());
        SoundManager.Instance.PlaySound("Hit");
        mat.color = Color.yellow;
        isChase = false;

        yield return new WaitForSeconds(0.1f);

        if (CurHp > 0)
        {
            ani.SetTrigger("doGethit");
            mat.color = Color.white;
        }
        else
        {
            StartCoroutine("Die");
        }

        isChase = true;
        yield return null;
    }

    public override IEnumerator Die()
    {
        StopCoroutine("OnDamage");
        OnDie();
        mat.color = Color.gray;
        gameObject.layer = 13;
        UIPanel.gameObject.SetActive(false);

        isChase = false;
        nav.enabled = false;
        ani.SetTrigger("doDie");

        yield return new WaitForSeconds(0.5f);

        while (mat.color.a > 0)
        {
            var color = mat.color;
            color.a -= 0.6f * Time.deltaTime;

            mat.color = color;
            yield return null;
        }

        Destroy(this.gameObject);
    }

    public override void OnDie()
    {
        ondie.Invoke();

        SoundManager.Instance.PlaySound("MonsterDie");
        MonsterSpawner.Instance.DieToRemoveList(this.gameObject);
        DropItem();
        DropCoin();
    }
    public override void DropItem()
    {
        int itemindex = Random.Range(0, 12);
        Vector3 dropPos = new Vector3(this.transform.position.x + Random.Range(-3, 3), this.transform.position.y, this.transform.position.z + Random.Range(-3, 3));
        Item itemObj = dropItems[itemindex];

        if ( itemObj != null )
            Instantiate(itemObj.itemPrefabs, dropPos, Quaternion.identity);
    }

    public override void DropCoin()
    {
        int dropCoin = Random.Range(30, 100);
        Inventory.instance.PlusCoin(dropCoin);
    }

    private void OnTriggerEnter(Collider other)
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
}
