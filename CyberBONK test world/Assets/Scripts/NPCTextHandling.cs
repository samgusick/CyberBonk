using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class NPCTextHandling : MonoBehaviour
{
    public static GameObject mainCamera;
    private Canvas canvas;
    private TextMeshProUGUI text;
    private void Awake() {
        canvas = GetComponent<Canvas>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start() {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }
    private void Update() {
    
        if (text.text != "")
        {
            canvas.transform.LookAt(mainCamera.transform, Vector3.up);
        }

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
