using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmsAnimationEvents : MonoBehaviour
{
    public Animator animator;

    public void PistolFiringDone()
    {
        animator.SetBool("isShooting", false);
    }
}
