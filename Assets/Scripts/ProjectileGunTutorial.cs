using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using NUnit.Framework.Constraints;
using UnityEngine.UI;
using System;
using UnityEngine.Android;

public class ProjectileGunTutorial : MonoBehaviour
{
    // Bullet prefab to spawn
    public GameObject bullet;

    // Bullet force
    public float shootForce, upwardForce;

    // Sprites
    public Sprite[] automaticRifle;
    public Sprite[] shotGun;
    public Sprite[] regSprite;
    public GameObject uiGun;

    // Gun stats
    public float timeBetweenShooting = 0.1f;
    public float spread = 3f;
    public float reloadTime = 1.5f;
    public float timeBetweenShots = 0.1f;
    public int magazineSize = 100;
    public int bulletsPerTap = 1;
    public bool allowButtonHold = true;
    public bool autoFire = false;
    public bool vampire = false;
    public bool burstShotGun = false;
    public bool slowBullets = false;

    int bulletsLeft, bulletsShot;
    public float healthAdder;
    public float ogHealthAdder;
    public float vampAdder;

    // Recoil
    public Rigidbody playerRb;
    public float recoilForce;

    // State
    bool shooting, readyToShoot, reloading;

    // References
    public Camera fpsCam;
    public Transform attackPoint;
    public GameObject playerMan;

    // Graphics
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;
    public TextMeshProUGUI killCount;
    public GameObject bulletImpact;

    public bool split = false;

    // Bug fixing
    bool allowInvoke = true;

    // Player controls
    private InputAction fire;
    private InputAction reload;
    CameraLookControls playerInput;
    public float damage = 1f;
    public float muzzleTime = 0.1f;
    private String currGun;

    // Data tracker
    private int kills = 0;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
        playerInput = new CameraLookControls();
    }

    private void OnEnable()
    {
        fire = playerInput.Player.Fire;
        fire.Enable();

        reload = playerInput.Player.Reload;
        reload.Enable();
    }

    private void onDisable()
    {
        fire.Disable();
        reload.Disable();
    }

    private void Update()
    {
        MyInput();

        // Update ammo display
        if (ammunitionDisplay != null)
            ammunitionDisplay.SetText((bulletsLeft / bulletsPerTap) + " / " + (magazineSize / bulletsPerTap));

        killCount.SetText("Kills: " + kills);
    }

    private void MyInput()
    {
        if (autoFire)
        {
            shooting = true;
        }
        else
        {
            if (allowButtonHold)
            {
                if (fire.ReadValue<float>() > 0)
                    shooting = true;
                else
                    shooting = false;
            }
            else
            {
                shooting = Input.GetKeyDown(KeyCode.Mouse0);
            }
        }

        if (reload.ReadValue<float>() > 0 && bulletsLeft < magazineSize && !reloading)
            Reload();

        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0)
            Reload();

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;
            Shoot();
        }
    }

    private void Shoot()
    {
        if (!PlayerManager.paused)
        {
            readyToShoot = false;
            if (split)
            {
                float[] posXspread = { 0f, -0.05f, 0.05f };
                for (int i = 0; i < 3; i++)
                {
                    float x = posXspread[i];
                    float y = 0;
                    Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f + x, 0.5f + y, 0));
                    RaycastHit hit;
                    Vector3 targetPoint;
                    if (Physics.Raycast(ray, out hit))
                    {
                        targetPoint = hit.point;
                        Instantiate(bulletImpact, hit.point, transform.rotation);

                        if (hit.transform.tag == "Enemy")
                        {
                            if (slowBullets)
                                hit.transform.GetComponent<Enemies>().Slow(2f);
                            bool kill = false;
                            kill = hit.transform.GetComponent<Enemies>().hit(damage);
                            if (kill && !hit.transform.GetComponent<Enemies>().isDead)
                            {
                                hit.transform.GetComponent<Enemies>().dead();
                                kills += 1;
                                playerMan.GetComponent<PlayerManager>().addKills();
                            }
                        }
                        else if (hit.transform.tag == "Pickup")
                        {
                            playerMan.GetComponent<PlayerManager>().increaseHealth(healthAdder);
                            Destroy(hit.transform.gameObject);
                        }
                    }
                    else
                        targetPoint = ray.GetPoint(75);

                    Vector3 directionWithoutSpread = targetPoint - attackPoint.position;


                    Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);
                }

            }
            else if (burstShotGun)
            {
                for (int i = 0; i < 20; i++)
                {
                    float x = UnityEngine.Random.Range(-spread, spread);
                    float y = UnityEngine.Random.Range(-spread, spread);
                    Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f + x, 0.5f + y, 0));
                    RaycastHit hit;
                    Vector3 targetPoint;
                    if (Physics.Raycast(ray, out hit))
                    {
                        targetPoint = hit.point;
                        Instantiate(bulletImpact, hit.point, transform.rotation);

                        if (hit.transform.tag == "Enemy")
                        {
                            if (slowBullets)
                                hit.transform.GetComponent<Enemies>().Slow(2f);
                            bool kill = false;
                            kill = hit.transform.GetComponent<Enemies>().hit(damage);
                            if (kill && !hit.transform.GetComponent<Enemies>().isDead)
                            {
                                hit.transform.GetComponent<Enemies>().dead();
                                kills += 1;
                                playerMan.GetComponent<PlayerManager>().addKills();
                            }
                        }
                        else if (hit.transform.tag == "Pickup")
                        {
                            playerMan.GetComponent<PlayerManager>().increaseHealth(healthAdder);
                            Destroy(hit.transform.gameObject);
                        }
                    }
                    else
                        targetPoint = ray.GetPoint(75);

                    Vector3 directionWithoutSpread = targetPoint - attackPoint.position;


                    Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);
                }
            }
            else
            {
                float x = UnityEngine.Random.Range(-spread, spread);
                float y = UnityEngine.Random.Range(-spread, spread);
                Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f + x, 0.5f + y, 0));
                RaycastHit hit;
                Vector3 targetPoint;
                if (Physics.Raycast(ray, out hit))
                {
                    targetPoint = hit.point;
                    Instantiate(bulletImpact, hit.point, transform.rotation);

                    if (hit.transform.tag == "Enemy")
                    {
                        if (slowBullets)
                            hit.transform.GetComponent<Enemies>().Slow(2f);
                        bool kill = false;
                        kill = hit.transform.GetComponent<Enemies>().hit(damage);
                        if (kill && !hit.transform.GetComponent<Enemies>().isDead)
                        {
                            hit.transform.GetComponent<Enemies>().dead();
                            kills += 1;
                            if (vampire)
                                playerMan.GetComponent<PlayerManager>().increaseHealth(vampAdder);
                            playerMan.GetComponent<PlayerManager>().addKills();
                        }
                    }
                    else if (hit.transform.tag == "Pickup")
                    {
                        playerMan.GetComponent<PlayerManager>().increaseHealth(healthAdder);
                        Destroy(hit.transform.gameObject);
                    }
                }
                else
                    targetPoint = ray.GetPoint(75);

                Vector3 directionWithoutSpread = targetPoint - attackPoint.position;


                Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);
            }


            AudioManager.instance.shot();
            /*
            // Instantiate bullet
            GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
            currentBullet.transform.forward = directionWithSpread.normalized;

            // Add forces
            Rigidbody bulletRb = currentBullet.GetComponent<Rigidbody>();
            if (bulletRb != null)
            {
                bulletRb.AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
                bulletRb.AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);
            }

            // Muzzle flash
            if (muzzleFlash != null)
                Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
            */
            bulletsLeft--;
            bulletsShot++;

            uiGun.GetComponent<Image>().sprite = regSprite[1];
            //Debug.Log("SHOT");
            Invoke(nameof(flash), muzzleTime);

            if (allowInvoke)
            {
                Invoke(nameof(ResetShot), timeBetweenShooting);
                allowInvoke = false;
                /*
                if (playerRb != null)
                    playerRb.AddForce(-directionWithSpread.normalized * recoilForce, ForceMode.Impulse);
                */
            }

            if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
                Invoke(nameof(Shoot), timeBetweenShots);
        }
    }

    private void flash()
    {
        uiGun.GetComponent<Image>().sprite = regSprite[0];
    }
    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        AudioManager.instance.reloadSound();
        Invoke(nameof(ReloadFinished), reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
        AudioManager.instance.reloadStop();
    }

    public void burstUpgrade()
    {
        timeBetweenShooting = 0.6f;
        timeBetweenShots = 0.04f;
        muzzleTime = 0.02f;
        damage += 1.25f;
        bulletsPerTap = 3;
        magazineSize *= 3;
        bulletsLeft = magazineSize;
    }

    public void quickUpgrade()
    {
        timeBetweenShots = 0.025f;
        timeBetweenShooting = 0.085f;
        damage -= 0.9f;
        muzzleTime = 0.01f;
        magazineSize = 50;
        reloadTime = 2.5f;
        bulletsLeft = magazineSize;
    }

    public void combatShotUpgrade()
    {
        spread = 0.03f;
        damage = 0.85f;
        magazineSize += bulletsPerTap * 10;
        bulletsLeft = magazineSize;
    }

    public void tacticalShotUpgrade()
    {
        timeBetweenShooting = 0.5f;
        spread = 0.085f;
        magazineSize += bulletsPerTap * 5;
        bulletsLeft = magazineSize;
    }

    public void splitUpgrade()
    {
        split = true;
    }

    public void increaseDmg()
    {
        damage *= 2;
        bulletsLeft = magazineSize;
    }

    public void vampireShells()
    {
        vampire = true;
        bulletsLeft = magazineSize;
    }

    public void burstShotsUpgrade()
    {
        burstShotGun = true;
        timeBetweenShots = 0.1f;
        magazineSize = (magazineSize / 20) * 3;
        bulletsPerTap = 3;
        magazineSize *= 3;
    }

    public void gameStart()
    {
        bulletsLeft = magazineSize;
        healthAdder = ogHealthAdder;
        kills = 0;
        split = false;
        vampire = false;
        burstShotGun = false;
        slowBullets = false;
    }

    public void increaseAmmo()
    {
        magazineSize += bulletsPerTap * 5;
    }

    public void setGun(String weapon)
    {
        switch (weapon)
        {
            case "automaticRifle":
                shootForce = 300;
                upwardForce = 0;
                regSprite = automaticRifle;
                uiGun.GetComponent<Image>().sprite = regSprite[0];
                timeBetweenShooting = 0.25f;
                spread = 0;
                reloadTime = 0.75f;
                timeBetweenShots = 0.1f;
                magazineSize = 20;
                bulletsPerTap = 1;
                damage = 2;
                muzzleTime = 0.1f;
                break;

            case "shotGun":
                shootForce = 300;
                upwardForce = 0;
                regSprite = shotGun;
                uiGun.GetComponent<Image>().sprite = regSprite[0];
                timeBetweenShooting = 1f;
                spread = 0.075f;
                reloadTime = 1f;
                timeBetweenShots = 0f;
                magazineSize = 100;
                bulletsPerTap = 20;
                damage = 0.75f;
                muzzleTime = 0.1f;
                break;
        }
    }
}