using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SceneTransition : MonoBehaviour
{
    //wah
    public GameObject weaponMan;
    public GameObject gameMan;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        weaponMan = GameObject.FindGameObjectsWithTag("weaponMan")[0];
        gameMan = GameObject.FindGameObjectsWithTag("gameMan")[0];
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void pressed(String primary)
    {
        weaponMan.GetComponent<WeaponManager>().setPrimary(primary);
        gameMan.GetComponent<GameManager>().gameBegin();


        SceneManager.LoadScene("Demo");
    }

    public void restart()
    {
        Debug.Log("poop");
        GameManager.level = 0;
        SceneManager.LoadScene("Menu");
    }

    public void leaveGame()
    {
        Application.Quit();
    }
}
