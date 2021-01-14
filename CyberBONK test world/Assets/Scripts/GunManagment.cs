using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManagment : MonoBehaviour
{
    Weapon pistol;
    public PlayerManager playerManager;
    public GameObject handgunEnd;
    public GameObject bloodSplatterObject;
    public List<Weapon> weaponStatsArray;
    public List<GameObject> weaponTypesArray;
    int weaponEquippedID = 0;

    void Start()
    {
        weaponStatsArray = new List<Weapon>();
        //weaponTypesArray = new List<GameObject>();
        pistol = new Weapon("Pistol", 0, 15, false, handgunEnd);
        weaponStatsArray.Add(pistol);

    }

    private void Update() {
        
    }

    void fireGunParticles()
    {
        weaponTypesArray[weaponEquippedID].GetComponentInChildren<ParticleSystem>().Play();
    }

    void playGunAnimation()
    {
        weaponTypesArray[weaponEquippedID].GetComponentInChildren<Animator>().SetBool("isFiring", true);
        if (playerManager.animator.GetBool("isShooting") && !playerManager.animator.GetBool("isScoped"))
        {

            playerManager.animator.Play("isShootingHip", 1, 0f);
        }
        else if(playerManager.animator.GetBool("isShooting") && playerManager.animator.GetBool("isScoped"))
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

    public void fireWeapon()
    {
        Vector3 rayPos = new Vector3(Screen.width/2f, Screen.height/2f, 0);
        RaycastHit raycastHit;
        Ray ray = Camera.main.ScreenPointToRay(rayPos);

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

    }

    IEnumerator waitUntilNextShot()
    {
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
