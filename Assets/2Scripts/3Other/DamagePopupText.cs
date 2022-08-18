using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopupText : MonoBehaviour
{
    [SerializeField]
    private float alphaSpeed;

    [SerializeField]
    private float destoryTime;

    [SerializeField]
    TextMeshProUGUI text;
    [SerializeField]
    Color _color;

    public int damage;
    public GameObject target;


    private void Start()
    {
        alphaSpeed = 2.0f;
        destoryTime = 5f;

        text = GetComponent<TextMeshProUGUI>();
        _color = text.color;

        Invoke("DestroryObject", destoryTime);

        transform.position = Camera.main.WorldToScreenPoint(target.transform.position + new Vector3(0, 8f, 0));
    }

    private void Update()
    {
        
        _color.a = Mathf.Lerp(_color.a, 0, alphaSpeed * Time.deltaTime);
        text.color = _color;
    }

    public void SetText( int num )
    {
        text.text = num.ToString();
    }

    private void DestroryObject()
    {
        Destroy(this.gameObject);
    }
}
