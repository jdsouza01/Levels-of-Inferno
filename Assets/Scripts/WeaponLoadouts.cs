using UnityEngine;

public class WeaponLoadouts : MonoBehaviour
{

    public GameObject info;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        info.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void activetSmth()
    {
        info.SetActive(!info.activeSelf);
    }

    void OnMouseOver()
    {
        Debug.Log("Hello");
        info.SetActive(true);
    }

    void OnMouseExit()
    {
        info.SetActive(false);
    }
}
