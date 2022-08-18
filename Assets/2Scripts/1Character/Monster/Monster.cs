using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    public string Name; // 이름
    public int Damage; // 데미지
    public int MaxHp; // 최대 체력
    public int CurHp; // 현재 체력

    public abstract void DisplayHp(); // 몬스터의 체력 실시간 업데이트
    public abstract IEnumerator Attack(); // 몬스터의 공격 코루틴
    public abstract IEnumerator OnDamage(); // 몬스터의 피격 코루틴
    public abstract IEnumerator Die(); // 몬스터의 사망 코루틴
    public abstract void OnDie();
    public abstract void DropItem(); // 몬스터 사망시 아이템 드랍 함수
    public abstract void DropCoin(); // 몬스터 사망시 골드 드랍 함수
}
