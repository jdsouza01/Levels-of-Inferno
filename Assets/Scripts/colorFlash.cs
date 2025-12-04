using UnityEngine;

public class colorFlash : MonoBehaviour
{
     // Mesh renders
    public SpriteRenderer meshy;
    public Color origColor;
    public float flashTime = 0.1f;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void flashStart()
    {
        meshy.color = Color.red;
        Invoke("flashEnd", flashTime);

    }

    private void flashEnd()
    {
        meshy.color = origColor;
    }
}
