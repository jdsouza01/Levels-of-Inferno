using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponChoose : MonoBehaviour
{
    public GameObject AutomaticRifle;
    public GameObject shotGun;
    public GameObject weaponMan;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        weaponMan = GameObject.FindGameObjectsWithTag("weaponMan")[0];
        if (weaponMan.GetComponent<WeaponManager>().shotGunUnlocked)
        {
            shotGun.SetActive(true);
        }
        else
            shotGun.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
