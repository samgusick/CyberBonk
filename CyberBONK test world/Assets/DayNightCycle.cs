using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    private void Update() {
        transform.RotateAround(Vector3.zero, Vector3.right, Time.deltaTime);
        transform.LookAt(Vector3.zero);
    }
}
