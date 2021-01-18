using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public Slider slider;

    private void Awake() {
        slider = GetComponentInChildren<Slider>();
    }

    private void Update() {
        if (PlayerManager.health > 0)
        {
            slider.value = 1 - PlayerManager.health / PlayerManager.startHealth;
        }
        else{
            slider.value = 1;
        }

    }
}
