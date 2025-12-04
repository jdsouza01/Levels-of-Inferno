using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject loadoutMenu;
    public GameObject regularMenu;
    public GameObject weaponSelect;
    public GameObject weaponMan;
    public GameObject shotGunLocked;
    public GameObject shotGunUnlocked;
    public GameObject[] something;
    public TextMeshProUGUI highscore;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        something = GameObject.FindGameObjectsWithTag("weaponMan");
        weaponMan = something[0];
        loadoutMenu.SetActive(false);
        regularMenu.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void loadoutMenuload()
    {
        something = GameObject.FindGameObjectsWithTag("gameMan");
        loadoutMenu.SetActive(true);
        regularMenu.SetActive(false);
        highscore.SetText("Highscore: " + something[0].GetComponent<GameManager>().highscore() + " kills");

        if (weaponMan.GetComponent<WeaponManager>().shotGunUnlocked)
        {
            shotGunUnlocked.SetActive(true);
            shotGunLocked.SetActive(false);
        }
        else
        {
            shotGunLocked.SetActive(true);
            shotGunUnlocked.SetActive(false);
        }
    }

    public void regularMenuLoad()
    {
        regularMenu.SetActive(true);
        loadoutMenu.SetActive(false);
        weaponSelect.SetActive(false);
    }

    public void weaponSelectLoad()
    {
        weaponSelect.SetActive(true);
        regularMenu.SetActive(false);
    }
}
