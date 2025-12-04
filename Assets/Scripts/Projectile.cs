using UnityEngine;

public class Projectile : MonoBehaviour
{
    private void Start()
    {
        // Destroy projectile after 5 seconds if it doesn't hit anything
        Destroy(gameObject, 2f);
    }

}