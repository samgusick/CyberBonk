using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handgunscript : MonoBehaviour
{
    
    public void stopFireAnimation()
    {
        GetComponentInParent<Animator>().SetBool("isFiring", false);
    }
}
