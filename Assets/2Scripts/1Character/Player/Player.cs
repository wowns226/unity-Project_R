using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    #region Player Singleton
    static public Player instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion

    [SerializeField]
    private Image coolImg;
    [SerializeField]
    private TextMeshProUGUI coolText;

    public GameObject damageTextPrefab;
    public GameObject canvas;

    public Animator anim; // 플레이어의 애니메이션
    public SkinnedMeshRenderer mesh; // 플레이어의 메쉬렌더러
    public NavMeshAgent nav; // 플레이어의 NavMesh

    [Header("플레이어 스탯 정보")]
    public int maxhealth; // 최대 체력
    public int curhealth; // 현재 체력
    public int maxMana;
    public int curMana;
    public int Defense; // 방어력
    public int skillMana; // 스킬 사용 마나

    float inputX; // X축 이동
    float inputY; // Z축 이동

    public float walkSpeed; // 걷기 움직이는 속도
    public float runSpeed; // 뛰기 움직이는 속도
    float rotateSpeed; // 플레이어의 회전 속도 [ 0, 1 ]
    float fireDelay; // 공격 딜레이
    [SerializeField]
    float skillDelay; // 스킬 딜레이

    [Header("키 정보")]
    bool rDown; // 뛰기 키를 눌렀나 확인
    bool fDown; // 공격 키를 눌렀나 확인
    bool sDown; // 스킬 키를 눌렀나 확인
    public bool jDown; // 포탈 이동
    bool key1Down; // 숫자 1번 키를 눌렀나 확인

    public bool isMove;
    public bool isFireReady; // 공격을 할 수 있는지 확인
    public bool isSkillReady; // 스킬을 사용할 수 있는지 확인
    public bool isDamage; // 공격 받았는지 확인
    public bool isDie; // 죽음

    Vector3 moveVec; // InputX와 InputY를 합쳐서 만들 벡터
    Quaternion rotation; // 캐릭터 회전

    public int FieldIndex = 0;

    [SerializeField]
    private UnityEvent onDie;

    void Start()
    {
        walkSpeed = 10f;
        runSpeed = 20f;
        rotateSpeed = 0.3f;
        fireDelay = 0f;
        skillDelay = 0f;

        curhealth = maxhealth;
        curMana = maxMana;
    }

    void Update()
    {
        if ( !Equipment.equipmentActivated && !QuestManager.questActivated && !Dialogue.dialogueActivated && !Shop.shopActivated && !EscManager.escActivated && !isDie) 
        {
            GetInput();
            Move();
            Rotate();

            if ( !Inventory.inventoryActivated && !SceneManager.Instance.go_stageScene.activeSelf )
            {
                Attack();
                Skill();
                UseQuickSlotItem();
            }
                
        }
        else
        {
            anim.SetBool("isWalk", false);
            anim.SetBool("isRun", false);
        }
    }

    // 키다운 확인
    public void GetInput()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        rDown = Input.GetButton("Run");
        fDown = Input.GetButtonDown("Fire1");
        sDown = Input.GetButtonDown("Fire2");
        jDown = Input.GetButtonDown("Jump");
        key1Down = Input.GetButtonDown("Item");
    }

    // 이동 구현
    public void Move()
    {
        if (!Weapon.instance.isAttack && !Weapon.instance.isSkill)
        {
            moveVec = new Vector3(inputX, 0, inputY).normalized; // 이동하는 방향 값이 1로 보정된 벡터

            transform.position += moveVec * Time.deltaTime * (rDown ? runSpeed : walkSpeed);
        }


        anim.SetBool("isWalk", moveVec != Vector3.zero);
        anim.SetBool("isRun", rDown);
    }

    // 회전 구현
    public void Rotate()
    {
        if (moveVec != Vector3.zero)
        {
            rotation = Quaternion.LookRotation(moveVec);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed);
        }
    }

    // 공격 구현
    public void Attack()
    {
        if (Weapon.instance == null)
        {
            Debug.Log("equipWeapon null");
            return;
        }

        if ( Weapon.instance.isSkill )
            return;

        fireDelay += Time.deltaTime;

        isFireReady = Weapon.instance.attackRate <= fireDelay;

        if (fDown && isFireReady )
        {
            Weapon.instance.UseAttack();
            anim.SetTrigger("doAttack");
            fireDelay = 0;
        }
    }

    // 스킬 구현
    public void Skill()
    {
        if (Weapon.instance == null)
        {
            Debug.Log("equipWeapon null");
            return;
        }

        if ( Weapon.instance.isAttack )
            return;

        //skillDelay += Time.deltaTime;

        skillDelay = Mathf.Clamp(skillDelay + Time.deltaTime, 0, Weapon.instance.skillRate);

        isSkillReady = Weapon.instance.skillRate <= skillDelay;

        coolImg.fillAmount = 1 - ( skillDelay / Weapon.instance.skillRate );
        coolText.text = string.Format("{0:0.##}", ( 1 - ( skillDelay / Weapon.instance.skillRate ) ));

        if ( coolText.text.Equals("0") ) 
            coolText.gameObject.SetActive(false);
        else
            coolText.gameObject.SetActive(true);

        if ( curMana < skillMana && isSkillReady )
        {
            //NotifyText.Instance.SetText("<color=red>스킬을 사용하는데 마나가 부족합니다</color>");
            return;
        }

        if (sDown && isSkillReady)
        {
            Weapon.instance.UseSkill();
            curMana -= skillMana;
            anim.SetTrigger("doSkill");
            skillDelay = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 아이템과의 충돌
        if (other.gameObject.CompareTag("Item"))
        {
            SoundManager.Instance.PlaySound("GetItem");
            Item _item = other.GetComponent<ItemPickUp>().item;
            Inventory.instance.AcquireItem(_item);
            NotifyText.Instance.SetText($"<color=green>{_item.itemName}</color>을 획득하셨습니다.");
            Destroy(other.gameObject);
        }
        // 몬스터 공격과의 충돌
        else if (other.gameObject.CompareTag("EnemyBullet"))
        {
            if (!isDamage)
            {
                int damageAmount = other.GetComponent<Bullet>().damage;
                bool isMelee = other.GetComponent<Bullet>().isMelee;

                int damage = damageAmount - (int)( Defense * 0.5 );

                curhealth -= damage;

                GameObject damageText = Instantiate(damageTextPrefab, transform.position, Quaternion.identity, canvas.transform);
                damageText.GetComponent<DamagePopupText>().target = this.gameObject;
                damageText.GetComponent<DamagePopupText>().SetText(damageAmount);

                StartCoroutine(OnDamage());

                if ( !isMelee )
                    Destroy(other.gameObject);
            }
        }
    }

    // 피격 코루틴
    IEnumerator OnDamage()
    {
        if ( curhealth > 0 )
        {
            isDamage = true;
            mesh.material.color = Color.yellow;
            SoundManager.Instance.PlaySound("Hit");
            yield return new WaitForSeconds(2f);
            isDamage = false;
            mesh.material.color = Color.white;
        }
        else
        {
            StartCoroutine("Die");

        }
    }

    // 사망 코루틴
    IEnumerator Die()
    {
        isDie = true;
        isDamage = true;
        mesh.material.color = Color.gray;
        anim.SetBool("isDie", true);
        yield return new WaitForSeconds(2f);
        onDie.Invoke();
        yield return null;
    }

    public void UseQuickSlotItem()
    {
        if ( key1Down )
        {
            QuickSlot quick = QuickSlot.Instance;

            quick.quickSlot.Use((UsableItem)quick.quickSlot.item);
        }
    }
}
