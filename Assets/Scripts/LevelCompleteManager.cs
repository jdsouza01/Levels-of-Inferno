using UnityEngine;

public class LevelCompleteManager : MonoBehaviour
{
    void Start()
    {
        PlayerManager.paused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        PassiveUpgradeManager passiveMan = Object.FindFirstObjectByType<PassiveUpgradeManager>();
        if (passiveMan != null)
        {
            passiveMan.SetAvailablePassive(GameManager.level);
        }
    }

    public void OnContinueButtonPressed()
    {
        PlayerManager.paused = false;
        
        GameManager.instance.LoadNextGameplayScene();
    }
}

