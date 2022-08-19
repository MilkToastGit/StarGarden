using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class GachaAnimation : MonoBehaviour
{
    public GameObject baseObject;
    public VideoPlayer player;
    public Animator glowAnim;

    private bool animActivated = false;

    private void OnEnable()
    {
        player.gameObject.SetActive(true);
        animActivated = false;
    }

    private void Update()
    {
        if (!animActivated && player.isPlaying && player.frame > 87)
        {
            animActivated = true;
            glowAnim.SetTrigger("Expand");
        }
    }

    public void OnGlowCoveringScreen()
    {
        player.gameObject.SetActive(false);
    }

    public void OnGlowFaded()
    {
        //baseObject.gameObject.SetActive(false);
    }
}
