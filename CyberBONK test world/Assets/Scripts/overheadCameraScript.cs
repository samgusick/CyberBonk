using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class overheadCameraScript : MonoBehaviour
{
    public GameObject player;
    
    private void Update() {
        transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
    }
}
