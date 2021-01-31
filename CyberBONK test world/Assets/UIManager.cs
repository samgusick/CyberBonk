using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public Slider slider;
    public Image crosshair;
    private void Awake() {
        slider = GetComponentInChildren<Slider>();
    }

    private void Update() {

        if (Input.GetKey(KeyCode.Mouse1))
        {
            crosshair.enabled = false;
        }
        else
        {
            crosshair.enabled = true;
        }

        if (PlayerManager.health > 0)
        {
            slider.value = 1 - PlayerManager.health / PlayerManager.startHealth;
        }
        else{
            slider.value = 1;
        }

    }
}
