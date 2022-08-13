using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDAnimation : MonoBehaviour
{
    private Animator anim;
    private bool showing = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void AnimateHUD()
    {
        if(!showing)
        {
            anim.SetTrigger("ShowHUD");
            showing = true;
        }
        else if (showing)
        {
            anim.SetTrigger("HideHUD");
            showing = false;
        }
    }
}