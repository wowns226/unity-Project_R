using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class NotifyText : MonoBehaviour
{
    #region NotifyText SingleTon
    public static NotifyText Instance;
    private void Awake()
    {
        if ( Instance == null )
            Instance = this;
    }
    #endregion

    [SerializeField]
    private TextMeshProUGUI notifyText;
    [SerializeField]
    private GameObject notifyParent;
    [SerializeField]
    private GameObject notifyPrefab;
    [Range(0f,1f)]
    [SerializeField]
    private float fadeDelay;

    public void SetText( string text )
    {
        GameObject notify = Instantiate(notifyPrefab, notifyParent.transform);

        TextMeshProUGUI notifyText = notify.GetComponent<TextMeshProUGUI>();

        notifyText.text = text;

        notify.GetComponent<RectTransform>().DOAnchorPosY(340, 1.2f);

        StopCoroutine(FadeAway(notifyText));
        StartCoroutine(FadeAway(notifyText));
    }

    private IEnumerator FadeAway(TextMeshProUGUI text)
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
