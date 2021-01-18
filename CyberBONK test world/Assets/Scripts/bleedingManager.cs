using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bleedingManager : MonoBehaviour
{
    public void resetBleeding()
    {
        GetComponent<Animator>().SetBool("TakenDamage", false);
    }
}
