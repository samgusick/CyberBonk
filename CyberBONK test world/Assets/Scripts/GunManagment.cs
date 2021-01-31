using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManagment : MonoBehaviour
{
    Weapon pistol;
    Weapon rifle;
    public PlayerManager playerManager;
    public GameObject handgunEnd;

    public Camera mainCamera;

    public GameObject assaultRifleEnd;
    public GameObject bloodSplatterObject;
    public List<Weapon> weaponStatsArray;
    public List<GameObject> weaponTypesArray;
    public int weaponEquippedID = 0;

    void Start()
    {
        gunActivation();
        weaponStatsArray = new List<Weapon>();
        //weaponTypesArray = new List<GameObject>();
        pistol = new Weapon("Pistol", 0, 15, false, handgunEnd);
        rifle = new Weapon("Rifle", .1f, 5, true, assaultRifleEnd);
        weaponStatsArray.Add(pistol);
        weaponStatsArray.Add(rifle);
        canFireAgain = true;

    }

    void gunActivation()
    {
        foreach (var item in weaponTypesArray)
        {
            if (item != weaponTypesArray[weaponEquippedID])
            {
                item.SetActive(false);
            }
        }


        weaponTypesArray[weaponEquippedID].SetActive(true);
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            weaponEquippedID = 0;
            gunActivation();
        }

        else if (Input.GetKey(KeyCode.Alpha2))
        {
            weaponEquippedID = 1;
            gunActivation();
        }
    }

    void fireGunParticles()
    {
        weaponTypesArray[weaponEquippedID].GetComponentInChildren<ParticleSystem>().Stop();
        weaponTypesArray[weaponEquippedID].GetComponentInChildren<ParticleSystem>().Play();
    }

    void playGunAnimation()
    {
        weaponTypesArray[weaponEquippedID].GetComponentInChildren<Animator>().SetBool("isFiring", true);
        if (playerManager.animator.GetBool("isShooting") && !playerManager.animator.GetBool("isScoped"))
        {

            playerManager.animator.Play("isShootingHip", 1, 0f);
        }
        else if (playerManager.animator.GetBool("isShooting") && playerManager.animator.GetBool("isScoped"))
        {
            playerManager.animator.Play("IsShootingScoped", 1, 0f);
        }

        else
        {
            playerManager.animator.SetBool("isShooting", true);
        }
    }

    void playFireSound()
    {
        weaponTypesArray[weaponEquippedID].GetComponentInChildren<AudioSource>().Play();
    }

    void splatterBlood(Ray ray, RaycastHit raycastHit)
    {
        Instantiate(bloodSplatterObject, raycastHit.point, Quaternion.LookRotation(ray.origin));
    }


    public bool canFireAgain;
    public void fireWeapon(float timeBetweenShots)
    {
        if (canFireAgain)
        {
            Vector3 rayPos = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
            RaycastHit raycastHit;
            Ray ray = mainCamera.ScreenPointToRay(rayPos);

            fireGunParticles();
            playGunAnimation();
            playFireSound();
            if (Physics.Raycast(ray, out raycastHit))
            {
                //Debug.DrawLine(weaponStatsArray[weaponEquippedID].weaponEnd.transform.position, raycastHit.point, Color.red, 5f);
                if (raycastHit.transform.gameObject.tag == "NPC")
                {
                    splatterBlood(ray, raycastHit);
                    raycastHit.transform.gameObject.GetComponentInParent<NPCBehaviour>().takeDamage(weaponStatsArray[weaponEquippedID].damagePerbullet);
                }
            }
            StartCoroutine(waitUntilNextShot(timeBetweenShots));
        }
    }

    IEnumerator waitUntilNextShot(float timeBetweenShots)
    {
        canFireAgain = false;
        yield return new WaitForSeconds(timeBetweenShots);
        canFireAgain = true;
        yield break;
    }


}

public class Weapon
{
    public string name;
    public float fireRate;
    public float damagePerbullet;
    public bool automatic;
    public GameObject weaponEnd;

    public Weapon(string name, float fireRate, float damagePerbullet, bool automatic, GameObject weaponEnd)
    {
        this.weaponEnd = weaponEnd;
        this.name = name;
        this.fireRate = fireRate;
        this.damagePerbullet = damagePerbullet;
        this.automatic = automatic;
    }
}
