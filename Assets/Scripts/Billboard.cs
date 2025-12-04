using Unity.VisualScripting;
using UnityEngine;
using Unity.Mathematics;

public class Billboard : MonoBehaviour
{
    private SpriteRenderer sr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (math.abs(Vector3.Distance(transform.position, cameraLook.instance.transform.position)) > 10)
        {
            transform.LookAt(cameraLook.instance.transform.position, -Vector3.down);
        }
    }
}
