using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using NUnit.Framework.Constraints;
using UnityEngine.UI;
using System;

public class FlashbangThrower : MonoBehaviour
{
    public GameObject Flashbang;
    public Transform throwPoint;
    public Camera fpsCam;

    public float throwForce = 15f;
    public float upwardForce = 2f;
    public float explosionRadius = 8f;
    public float explosionDelay = 0.1f;
    public float flashDuration = 3f;
    public float damage = 5f;
    public float flashbangCooldown = 5f;
    public float ogFlashbangCooldown = 5f;
    private float lastFlashbangTime = -Mathf.Infinity;
    public GameObject explosionEffect;

    public GameObject flashbangIndicator;

    void Update()
    {
        if (flashbangIndicator != null)
        {
            if (Time.time > lastFlashbangTime + flashbangCooldown)
            {
                flashbangIndicator.SetActive(true);
            }
            else
            {
                flashbangIndicator.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > lastFlashbangTime + flashbangCooldown)
        {
            ThrowFlashbang();
            lastFlashbangTime = Time.time;
        }
    }

    void ThrowFlashbang()
    {
        GameObject fb = Instantiate(Flashbang, throwPoint.position, Quaternion.identity);
        Rigidbody rb = fb.GetComponent<Rigidbody>();
        Vector3 throwDirection = fpsCam.transform.forward;

        rb.AddForce(throwDirection * throwForce + Vector3.up * upwardForce, ForceMode.Impulse);

        // Attach the explosion logic script and set parameters
        FlashbangExplosion explosion = fb.AddComponent<FlashbangExplosion>();
        explosion.explosionRadius = explosionRadius;
        explosion.flashDuration = flashDuration;
        explosion.damage = damage;
        explosion.explosionEffect = explosionEffect;
    }
    
    public void gameStart()
    {
        flashbangCooldown = ogFlashbangCooldown;
    }
}

// This script is attached at runtime to the thrown flashbang
public class FlashbangExplosion : MonoBehaviour
{
    public float explosionRadius = 8f;
    public float flashDuration = 3f;
    public float damage = 5f;
    public GameObject explosionEffect;
    private bool hasExploded = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasExploded)
        {
            hasExploded = true;
            Invoke(nameof(Explode), 0.1f);
        }
    }

    private void Explode()
    {
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearby in colliders)
        {
            if (nearby.CompareTag("Enemy"))
            {
                Enemies enemy = nearby.GetComponent<Enemies>();
                if (enemy != null)
                {
                    if (enemy.isGhost)
                    {
                        enemy.execute();
                    }
                    enemy.Stun(flashDuration);
                }
            }
        }
        Destroy(gameObject);
    }
}