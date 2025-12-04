using System;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public String primary;
    public GameObject gun;
    public bool shotGunUnlocked = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void gameStart()
    {
        gun.GetComponent<ProjectileGunTutorial>().setGun(primary);
    }

    public void weaponUnlock(String name)
    {
        switch (name)
        {
            case "shotGun":
                shotGunUnlocked = true;
                break;
        }
    }

    public void setPrimary(String name)
    {
        primary = name;
        AudioManager.instance.assignGunNoise(name);
    }
}
