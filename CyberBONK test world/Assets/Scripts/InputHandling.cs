using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandling : MonoBehaviour
{
    new Camera camera;

    public Animator armsAnimator;
    Vector3 rayPos = new Vector3(Screen.width/2f, Screen.height/2f, 0);
    private void Awake() {
        camera = GameObject.FindObjectOfType<Camera>();
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.E))
        {
            E_Pressed();
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            LeftClick_Pressed();
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            rightClick_Hold();
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            rightClick_released();
        }
    }

    public void rightClick_Hold()
    {
        armsAnimator.SetBool("isScoped", true);
    }

    public void rightClick_released()
    {
        armsAnimator.SetBool("isScoped", false);
    }
    public void E_Pressed(){
        RaycastHit raycastHit;
        Ray ray = camera.ScreenPointToRay(rayPos);
        if (Physics.Raycast(ray, out raycastHit))
        {
            if (raycastHit.transform.gameObject.tag == "NPC")
            {
                raycastHit.transform.gameObject.GetComponentInParent<NPCAudioManager>().playClip();
            }
        }
    } 

    public void LeftClick_Pressed(){
        transform.parent.GetComponentInChildren<GunManagment>().fireWeapon();
    }
}
