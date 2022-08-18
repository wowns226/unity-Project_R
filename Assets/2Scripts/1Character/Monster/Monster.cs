using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    public string Name; // �̸�
    public int Damage; // ������
    public int MaxHp; // �ִ� ü��
    public int CurHp; // ���� ü��

    public abstract void DisplayHp(); // ������ ü�� �ǽð� ������Ʈ
    public abstract IEnumerator Attack(); // ������ ���� �ڷ�ƾ
    public abstract IEnumerator OnDamage(); // ������ �ǰ� �ڷ�ƾ
    public abstract IEnumerator Die(); // ������ ��� �ڷ�ƾ
    public abstract void OnDie();
    public abstract void DropItem(); // ���� ����� ������ ��� �Լ�
    public abstract void DropCoin(); // ���� ����� ��� ��� �Լ�
}
