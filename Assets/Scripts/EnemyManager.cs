using Unity.Mathematics;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    private ArrayList enemy = new ArrayList();
    private ArrayList delEnemy = new ArrayList();
    public GameObject enemyTarget;
    public GameObject playerMan;
    public GameObject gameMan;
    public float zombTime = 2f;
    public float zombieTimer;
    public Vector3[] locations;
    public GameObject medkit;
    public float ogMedkitSpawnRate = 25;
    public float medkitDropRate;
    public bool active;

    // Enemy Prefabs
    public GameObject zombie;
    public GameObject babyZombie;
    public GameObject skeleton;
    public GameObject ghost;
    public GameObject demon;
    public GameObject golem;

    private int golemSpawnCount = 0;
    private const int maxGolemSpawns = 1;
    private GameObject golemInstance;
    private bool golemIsDead = false;



    void Start()
    {
        active = false;
        //RefreshReferences();
    }

    void OnLevelWasLoaded(int level)
    {
        //RefreshReferences();
    }

    void RefreshReferences()
    {
        playerMan = GameObject.FindWithTag("Player");
        if (Camera.main != null)
            enemyTarget = Camera.main.gameObject;

        foreach (GameObject e in enemy)
        {
            if (e != null)
            {
                Enemies enemyScript = e.GetComponent<Enemies>();
                enemyScript.target = enemyTarget;
                enemyScript.playerMan = playerMan;
            }
        }
    }
    void Update()
    {
        if (!active) return;

        checkEnemyDeath();

        // Check if the Golem has died
        if (!golemIsDead && golemInstance == null && golemSpawnCount >= 1)
        {
            golemIsDead = true;
        }

        alterZombTimeLevel1(gameMan.GetComponent<GameManager>().getTime());

        levelEnemySpawn();
    }


    private void levelEnemySpawn()
    {
        float timeSinceLastSpawn = Time.time - zombieTimer;

        if (timeSinceLastSpawn > zombTime)
        {
            zombieTimer = Time.time;
            GameObject enemyPrefab = spawnRates(GameManager.level);

            if (enemyPrefab != null)
            {
                spawnEnemy(enemyPrefab);
            }
        }
    }

    // Helper to select enemy prefab based on level and weights
    private GameObject spawnRates(int level)
    {
        // List of tuples: (prefab, weight)
        List<(GameObject, float)> spawnTable = new List<(GameObject, float)>();

        switch (level)
        {
            case 0:
                spawnTable.Add((zombie, 85f));
                spawnTable.Add((babyZombie, 15f));
                break;

            case 1:
                spawnTable.Add((zombie, 30f));
                spawnTable.Add((babyZombie, 20f));
                spawnTable.Add((skeleton, 50f));
                break;

            case 2:
                spawnTable.Add((zombie, 25f));
                spawnTable.Add((skeleton, 25f));
                spawnTable.Add((ghost, 50));
                break;

            case 3:
                spawnTable.Add((zombie, 20f));
                spawnTable.Add((babyZombie, 10f));
                spawnTable.Add((skeleton, 30f));
                spawnTable.Add((ghost, 30f));
                spawnTable.Add((demon, 10f));
                break;

            case 4:
                spawnTable.Add((ghost, 30f));
                spawnTable.Add((demon, 70f));
                break;

            case 5:
                if (!golemIsDead)
                {
                    if (golemSpawnCount < maxGolemSpawns)
                        spawnTable.Add((golem, 100f));
                }
                else
                {
                    // Golem is dead — now allow other enemies to spawn
                    spawnTable.Add((zombie, 20f));
                    spawnTable.Add((babyZombie, 10f));
                    spawnTable.Add((skeleton, 20f));
                    spawnTable.Add((ghost, 20f));
                    spawnTable.Add((demon, 30f));
                }
                break;

        }


        // Remove null prefabs (defensive)
        spawnTable.RemoveAll(entry => entry.Item1 == null);

        // Calculate total weight
        float totalWeight = 0f;
        foreach (var entry in spawnTable)
            totalWeight += entry.Item2;

        if (totalWeight <= 0f)
            return null;

        float roll = UnityEngine.Random.value * totalWeight;
        float accum = 0f;

        foreach (var entry in spawnTable)
        {
            accum += entry.Item2;
            if (roll <= accum)
                return entry.Item1;
        }

        // Fallback
        return spawnTable[0].Item1;
    }

    private void spawnEnemy(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogWarning("Enemy prefab not assigned.");
            return;
        }

        Vector3 spawnPos = locations[UnityEngine.Random.Range(0, locations.Length)];
        GameObject enemyObj = Instantiate(prefab, spawnPos, Quaternion.identity);
        if (prefab == golem)
        {
            golemSpawnCount++;
            golemInstance = enemyObj; // ✅ Store reference
        }

        enemy.Add(enemyObj);

        Enemies e = enemyObj.GetComponent<Enemies>();
        e.target = enemyTarget;
        e.playerManager = playerMan.GetComponent<PlayerManager>(); //Fix this lol
    }
    
    private void checkEnemyDeath()
    {
        foreach (GameObject e in enemy)
        {
            if (e != null && !e.GetComponent<Enemies>().isAlive())
            {
                if (UnityEngine.Random.Range(0, 100) < medkitDropRate)
                    Instantiate(medkit, new Vector3(e.transform.position.x, 0.09f, e.transform.position.z), Quaternion.identity);
                delEnemy.Add(e);
            }
        }

        foreach (GameObject e in delEnemy)
        {
            enemy.Remove(e);
            Destroy(e);
        }

        delEnemy.Clear();
    }

    private void alterZombTimeLevel1(float currTime)
    {
        if (currTime > 300)
            zombTime = 0.5f;
        else if (currTime > 240)
            zombTime = 0.6f;
        else if (currTime > 180)
            zombTime = 0.7f;
        else if (currTime > 120)
            zombTime = 0.8f;
        else if (currTime > 60)
            zombTime = 0.9f;
        else if (currTime > 30)
            zombTime = 1f;
    }

    public void Death()
    {
        enemy.Clear();
        active = false;
    }

    public void gameStart()
    {
        zombieTimer = Time.time;
        medkitDropRate = ogMedkitSpawnRate;

        // ✅ Dynamically adjust spawn time by level (faster at higher levels)
        zombTime = Mathf.Clamp(2.0f - (GameManager.level * 0.2f), 0.3f, 2.0f);

        golemSpawnCount = 0;
        golemIsDead = false;
        golemInstance = null;
        active = true;
    }

}