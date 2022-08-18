using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamagePopup : MonoBehaviour
{
    public static DamagePopup Instance;
    private void Awake()
    {
        if ( Instance == null )
            Instance = this;
    }

    [SerializeField]
    private GameObject DamagePopupPrefab;
    [SerializeField]
    Vector3 pos;
    [Range(0,1)]
    [SerializeField]
    private float fadeDelay;

    public GameObject CreateDamageText( int damage, GameObject parent )
    {
        GameObject damagePopup = Instantiate(DamagePopupPrefab, parent.transform);

        TextMeshProUGUI damagePopupText = damagePopup.GetComponent<TextMeshProUGUI>();

        damagePopupText.text = damage.ToString();

        return damagePopup;
    }

    public IEnumerator Fade( TextMeshProUGUI text )
    {
        while ( text.alpha > 0 )
        {
            text.alpha -= fadeDelay * Time.deltaTime;
            yield return null;
        }

        Destroy(text.gameObject);
        yield return null;
    }
}
