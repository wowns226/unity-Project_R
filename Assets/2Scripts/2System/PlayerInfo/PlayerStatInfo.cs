using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStatInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI PlayerHpText;
    [SerializeField] private TextMeshProUGUI PlayerStrText;
    [SerializeField] private TextMeshProUGUI PlayerDefText;

    private void Update()
    {
        UpdatePlayerInfo();
    }

    public void UpdatePlayerInfo()
    {
        PlayerHpText.text = Player.instance.maxhealth.ToString();
        PlayerStrText.text = Weapon.instance.attackdamage.ToString();
        PlayerDefText.text = Player.instance.Defense.ToString();
    }
}
