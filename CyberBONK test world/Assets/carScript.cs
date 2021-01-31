using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carScript : MonoBehaviour
{
    public GameObject CarFront;
    public GameObject CarBack;
    Color carColor;

    public float carSpeed;
    private void FixedUpdate() {
        transform.position = transform.position + transform.forward * carSpeed;
        if (transform.localPosition.x > 40)
        {
            transform.localPosition = new Vector3(-40, transform.localPosition.y, transform.localPosition.z);
        } 
    }

    private void Awake() {

        //  carColor = Random.ColorHSV();

        //  CarFront.GetComponent<Renderer>().material.color = carColor;
        //  CarBack.GetComponent<Renderer>().material.color = carColor;
    }
}
