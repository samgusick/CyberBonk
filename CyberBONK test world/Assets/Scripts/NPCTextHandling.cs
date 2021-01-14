using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class NPCTextHandling : MonoBehaviour
{
    private Canvas canvas;
    private TextMeshProUGUI text;
    private void Awake() {
        canvas = GetComponent<Canvas>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update() {
        canvas.transform.LookAt(Camera.main.transform, Vector3.up);
        
    }

    public void setTextNull()
    {
        text.text = "";
    }

    public void setText(string setText)
    {
        text.text = setText;
    }

}
