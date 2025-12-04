using UnityEngine;

public class EvolutionMenu : MonoBehaviour
{
    public GameObject gameMan;
    public float startTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void buttonPressed()
    {
        gameMan.GetComponent<GameManager>().unPause(startTime);
    }
}
