using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public static bool paused = false;
    public GameObject evolMenu;
    public GameObject evolMenu2;
    public GameObject evolMenuShotgun;
    public GameObject evolMenuShotgun2;
    public GameObject gameMan;
    public float health;
    public float ogHealth = 20;
    public float corrHealth = 20;
    public bool iFrameOff = true;
    public float iFrameTime = 0.5f;
    public int evolLim = 15;
    public int evolLim2 = 10;
    public TextMeshProUGUI healthTracker;
    public int kills = 0;
    int thisLevelKills = 0;
    public float healthAdder = 2;
    public GameObject ui;
    public GameObject gun;
    public bool evolvedUnlocked = false;
    public GameObject weaponMan;

    private float lastTickTime = 0f;
    public Image healthScreen;
    public float tickInterval = 2f; // damage every second
    public float tickDamage = 1f;   // how much damage per tick

    private bool evolved = false;
    private bool evolved2 = false;
    public int shotGunUnlockAmt = 5;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        paused = true;
        health = ogHealth;
        healthTracker.SetText("Health: " + health);
        ui.SetActive(false);
        gun.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        healthTracker.SetText("Health: " + health);
        var tempColor = healthScreen.color;
        tempColor.a = 1 - health / ogHealth;
        healthScreen.color = tempColor;

        // Ticking damage for Level 5 (last level)
        if (GameManager.level == 5 && Time.time - lastTickTime >= tickInterval)
        {
            health -= tickDamage;
            lastTickTime = Time.time;
        }

        if (health <= 0)
        {
            health = ogHealth;
            gameMan.GetComponent<GameManager>().playerDeath(kills);
        }

        if (thisLevelKills >= shotGunUnlockAmt && !weaponMan.GetComponent<WeaponManager>().shotGunUnlocked)
        {
            weaponMan.GetComponent<WeaponManager>().shotGunUnlocked = true;
        }

        if (!evolved && kills >= evolLim && weaponMan.GetComponent<WeaponManager>().primary == "automaticRifle")
        {
            Time.timeScale = 0f;
            paused = true;
            evolved = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            evolMenu.SetActive(true);
        }
        else if (!evolved2 && evolved && kills >= evolLim2 && weaponMan.GetComponent<WeaponManager>().primary == "automaticRifle")
        {
            Time.timeScale = 0f;
            paused = true;
            evolved2 = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            evolMenu2.SetActive(true);
        }
        
        if (!evolved && kills >= evolLim && weaponMan.GetComponent<WeaponManager>().primary == "shotGun")
        {
            Time.timeScale = 0f;
            paused = true;
            evolved = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            evolMenuShotgun.SetActive(true);
        }
        else if (!evolved2 && evolved && kills >= evolLim2 && weaponMan.GetComponent<WeaponManager>().primary == "shotGun")
        {
            Time.timeScale = 0f;
            paused = true;
            evolved2 = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            evolMenuShotgun2.SetActive(true);
        }
    }

    public void newLevel()
    {
        thisLevelKills = 0;
    }

    public void gameStart()
    {
        //Debug.Log("AHHH");
        ogHealth = corrHealth;
        evolved = false;
        evolved2 = false;
        paused = false;
        kills = 0;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ui.SetActive(true);
        gun.SetActive(true);
        evolMenu.SetActive(false);
    }

    public void Death()
    {
        paused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        ui.SetActive(false);
        gun.SetActive(false);  
    }

    public void onHit()
    {
        if (iFrameOff)
        {
            health -= 2;
            iFrameOff = false;

            Invoke("iFrame", iFrameTime);
        }
    }

    public void addKills()
    {
        kills++;
        thisLevelKills++;
    }

    private void iFrame()
    {
        iFrameOff = true;
    }

    public void buttonPressed()
    {
        evolMenu.SetActive(false);
        evolMenu2.SetActive(false);
        evolMenuShotgun.SetActive(false);
        evolMenuShotgun2.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void increaseHealth(float amt)
    {
        health += amt;
        if (health > ogHealth)
        {
            health = ogHealth;
        }
    }

}
