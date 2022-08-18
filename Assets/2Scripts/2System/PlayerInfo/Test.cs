using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public Animator anim;
    public Image playerstatButtonImage;

    public bool buttonClick = false;

    public void OnClickPlayerStatButton()
    {
        buttonClick = anim.GetBool("isOpen");

        anim.SetBool("isOpen", !buttonClick);
    }
}
