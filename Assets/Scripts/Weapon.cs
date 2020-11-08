using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Weapon : NetworkBehaviour
{
    public Gun[] loadout;
    public Transform weaponParent;
    public GameObject bulletholePrefab;
    public LayerMask canBeShot;
    private int currentIndex;
    private float currentCooldown;
    private GameObject currentWeapon;
    void Start()
    {
        
    }

    void Update()
    {
        if(isLocalPlayer){
            if(Input.GetKeyDown(KeyCode.Alpha1)) Equip(0);

            if (currentWeapon != null)
            {
                Aim(Input.GetMouseButton(1));

                if(Input.GetMouseButtonDown(0) && currentCooldown <= 0)
                {
                    Shoot();
                }

                //Weapon Position Elasticity
                currentWeapon.transform.localPosition = Vector3.Lerp(currentWeapon.transform.localPosition, Vector3.zero, Time.deltaTime * 4f);

                //Cooldown
               if(currentCooldown > 0) currentCooldown -= Time.deltaTime;
            }
        }
    }

    void Equip(int p_ind)
    {
        if (currentWeapon != null) Destroy(currentWeapon);

        currentIndex = p_ind;

        GameObject t_newWeapon = Instantiate (loadout[p_ind].prefab, weaponParent.position, weaponParent.rotation, weaponParent) as GameObject;
        t_newWeapon.transform.localPosition = Vector3.zero;
        t_newWeapon.transform.localEulerAngles = Vector3.zero;
        currentWeapon = t_newWeapon;
    }

    void Aim(bool p_isAiming)
    {
        Transform t_anchor = currentWeapon.transform.Find("Anchor");
        Transform t_state_ADS = currentWeapon.transform.Find("States/ADS");
        Transform t_state_Hip = currentWeapon.transform.Find("States/Hip");

        if(p_isAiming)
        {
            t_anchor.position = Vector3.Lerp(t_anchor.position, t_state_ADS.position, Time.deltaTime * loadout[currentIndex].aimSpeed);
        }
        else
        {
            t_anchor.position = Vector3.Lerp(t_anchor.position, t_state_Hip.position, Time.deltaTime * loadout[currentIndex].aimSpeed);
        }
    }
    void Shoot()
    {
        Transform t_spawn = transform.Find("Cameras/Main Camera");
        //Bloom
        Vector3 t_bloom = t_spawn.position + t_spawn.forward * 1000f;
        t_bloom += Random.Range(-loadout[currentIndex].bloom, loadout[currentIndex].bloom) * t_spawn.up;
        t_bloom += Random.Range(-loadout[currentIndex].bloom, loadout[currentIndex].bloom) * t_spawn.right;
        t_bloom -= t_spawn.position;
        t_bloom.Normalize();

        //Raycast
        RaycastHit t_hit = new RaycastHit();
        if(Physics.Raycast(t_spawn.position, t_bloom, out t_hit, 1000f, canBeShot))
        {
            GameObject t_newHole = Instantiate(bulletholePrefab, t_hit.point + t_hit.normal * 0.001f, Quaternion.identity) as GameObject;
            t_newHole.transform.LookAt(t_hit.point + t_hit.normal);
            Destroy(t_newHole, 5f);
        }

        //Gun FX
        currentWeapon.transform.Rotate(-loadout[currentIndex].recoil, 0, 0);
        currentWeapon.transform.position -= currentWeapon.transform.forward * loadout[currentIndex].kickback;

        //Cooldown
        currentCooldown = loadout[currentIndex].firerate;
    }
}
