using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class billboardScript : MonoBehaviour
{
    public Sprite[] billboardPics;
    public Image image;
    private void Awake() {
        int billboardID = Random.Range(0, billboardPics.Length);
        image.sprite = billboardPics[billboardID];
    }
}
