using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class PassiveUpgradeManager : MonoBehaviour
{
    [Header("Passive Slots")]
    public TextMeshProUGUI[] passiveSlotTexts; // Drag in Passive1Text - Passive5Text

    [Header("Upgrade Option Buttons")]
    public Button[] upgradeButtons;            // Drag in Upgrade1 - Upgrade3 buttons
    public TextMeshProUGUI[] upgradeButtonTexts; // Text on Upgrade1-3 buttons

    [Header("All Possible Upgrades")]
    public string[] allUpgradeNames = new string[10]; // Fill in list of 10 upgrade names

    private string[] currentOptions = new string[3];
    private int currentUnlockedSlot = -1;

    public GameObject playerMan;
    public GameObject gunMan;
    public GameObject enemyMan;
    private bool gotSlowed = false;

    void Start()
    {
    }

    public void gameStart()
    {
        gotSlowed = false;
        for (int i = 0; i < 5; i++)
        {
            passiveSlotTexts[i].SetText("");
        }
    }

    public void SetAvailablePassive(int levelIndex)
    {
        currentUnlockedSlot = levelIndex;
        GenerateRandomUpgrades();

        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            upgradeButtons[i].gameObject.SetActive(true);
            upgradeButtons[i].interactable = true;
            upgradeButtonTexts[i].text = currentOptions[i];
        }
    }

    public void SelectUpgrade(int index)
    {
        Debug.Log($"[SelectUpgrade] Clicked Upgrade {index}");

        if (currentUnlockedSlot < 0 || currentUnlockedSlot >= passiveSlotTexts.Length)
        {
            Debug.LogWarning($"[SelectUpgrade] Invalid slot index: {currentUnlockedSlot}");
            return;
        }

        string selectedUpgrade = currentOptions[index];
        Debug.Log($"[SelectUpgrade] Selected: {selectedUpgrade}");

        if (passiveSlotTexts[currentUnlockedSlot] == null)
        {
            Debug.LogWarning($"[SelectUpgrade] passiveSlotTexts[{currentUnlockedSlot}] is null!");
            return;
        }

        passiveSlotTexts[currentUnlockedSlot].text = selectedUpgrade;
        Debug.Log($"[SelectUpgrade] Applied to slot {currentUnlockedSlot}");

        // Disable upgrade buttons
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            upgradeButtons[i].interactable = false;
        }

        ApplyUpgradeEffect(selectedUpgrade);
    }


    void GenerateRandomUpgrades()
    {
        List<int> usedIndices = new List<int>();
        for (int i = 0; i < 3; i++)
        {
            int randIndex;
            do
            {
                randIndex = UnityEngine.Random.Range(0, allUpgradeNames.Length);
            } while (usedIndices.Contains(randIndex));
            if (gotSlowed && allUpgradeNames[randIndex].Equals("Slow Bullets"))
            {
                i--;
                continue;
            }
            usedIndices.Add(randIndex);
            currentOptions[i] = allUpgradeNames[randIndex];
        }
    }

    void HideAllUpgradeOptions()
    {
        foreach (var btn in upgradeButtons)
            btn.interactable = false;
    }

    void ApplyUpgradeEffect(string upgrade)
    {
        switch (upgrade)
        {
            case "Health Boost":
                playerMan.GetComponent<PlayerManager>().ogHealth += 15;
                playerMan.GetComponent<PlayerManager>().health += 15;
                break;
            case "Speed Up":
                playerMan.GetComponent<cameraControl>().moveSpeed += 0.2f;
                break;
            case "Faster Reload":
                gunMan.GetComponent<ProjectileGunTutorial>().reloadTime *= 0.9f;
                break;
            case "Damage Increase":
                gunMan.GetComponent<ProjectileGunTutorial>().damage *= 1.1f;
                break;
            case "Icy Bullets":
                gunMan.GetComponent<ProjectileGunTutorial>().slowBullets = true;
                gotSlowed = true;
                break;
            case "Increase Ammo":
                gunMan.GetComponent<ProjectileGunTutorial>().increaseAmmo();
                break;
            case "Medkit Spawnrate":
                enemyMan.GetComponent<EnemyManager>().medkitDropRate += 5f;
                break;
            case "Increase Fire Rate":
                gunMan.GetComponent<ProjectileGunTutorial>().timeBetweenShooting *= 0.9f;
                break;
            case "Flashbang CD":
                playerMan.GetComponent<FlashbangThrower>().flashbangCooldown -= 1f;
                break;
            case "Flashbang Stun+":
                playerMan.GetComponent<FlashbangThrower>().flashDuration += 0.5f;
                break;
            case "Medkit Health Gain":
                gunMan.GetComponent<ProjectileGunTutorial>().healthAdder += 1;
                break;
        }
    }
}

