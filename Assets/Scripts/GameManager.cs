using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public GameObject enemyTarget;
    public GameObject playerMan;
    public GameObject cam;
    public float startTime;
    public float currTime;
    public TextMeshProUGUI timeTracker;
    public GameObject levelCompletedScreen;

    public static int level = 0; // Start from 0 to match stages array
    public UnityEngine.Object[] stages; // List of gameplay scenes: Demo1, Demo2, ...
    public float[] winTimes;            // E.g. [30, 30, 30, 30, 30]
    public UnityEvent death;

    public bool gameOn;

    public static GameManager instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (level < stages.Length && scene.name == stages[level].name)
        {
            startTime = Time.time; // reset timer and start game when gameplay scene loads
            gameOn = true;
        }
    }

    void Start()
    {
        gameOn = false;
    }

    void Update()
    {
        if (gameOn)
        {
            currTime = Time.time - startTime;

            float timeLeft = winTimes[level] - currTime;
            timeLeft = Mathf.Max(0f, timeLeft); // Clamp so it doesn’t go negative

            if (timeTracker != null)
            {
                timeTracker.SetText("Time Left: " + Mathf.CeilToInt(timeLeft));
            }

            if (!PlayerManager.paused && checkWin(currTime))
            {
                Debug.Log("Level complete!");
                gameOn = false;
                StartCoroutine(HandleLevelComplete());
            }
        }
    }

    private IEnumerator HandleLevelComplete()
    {
        SceneManager.LoadScene("Levelcompleted");
        levelCompletedScreen.SetActive(true);

        yield return null;
        Time.timeScale = 0f; // pause after scene loads
    }

    public void LoadNextGameplayScene()
    {
        AudioManager.instance.stopWalk();
        AudioManager.instance.reloadStop();
        PlayerManager.paused = false;
        level++;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        levelCompletedScreen.SetActive(false);

        if (level < stages.Length)
        {
            SceneManager.LoadScene(stages[level].name);
            // gameBegin() will be called automatically by OnSceneLoaded
        }
        else
        {
            Debug.Log("All levels complete!");
            // Optionally load main menu or end screen
        }
    }

    public float getTime()
    {
        return currTime;
    }

    public void playerDeath(int kills)
    {
        string path = Application.dataPath + "/metric.txt";
        string pathTemp = Application.dataPath + "/temp.txt";

        if (!File.Exists(path))
        {
            File.WriteAllText(path, "");
        }
        if (!File.Exists(pathTemp))
        {
            File.WriteAllText(pathTemp, "0");
        }
        StreamWriter writer = new StreamWriter(path, true);
        StreamReader reader = new StreamReader(pathTemp);

        int round = Int32.Parse(reader.ReadLine());

        writer.WriteLine(round + 1);
        writer.WriteLine("Kills: " + kills);
        writer.WriteLine("Time: " + (Time.time - startTime));

        reader.Close();
        writer.Close();

        writer = new StreamWriter(pathTemp, false);
        writer.WriteLine(round + 1);
        writer.Close();

        path = Application.dataPath + "/highscore.txt";

        if (!File.Exists(path))
        {
            File.WriteAllText(path, "0");
        }
        
        reader = new StreamReader(path);

        if (kills >= Int32.Parse(reader.ReadLine()))
        {
            reader.Close();
            writer = new StreamWriter(path, false);
            writer.WriteLine(kills);
        }

        writer.Close();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        gameObject.BroadcastMessage("Death");
        SceneManager.LoadScene("Death");
    }

    public int highscore()
    {
        string path = Application.dataPath + "/highscore.txt";
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "0");
        }
        StreamReader reader = new StreamReader(path);
        int highest = Int32.Parse(reader.ReadLine());
        reader.Close();
        return highest;
    }

    public void unPause(float pTime)
    {
        //startTime += Time.time - pTime;
    }

    public bool checkWin(float time)
    {
        return level < winTimes.Length && time >= winTimes[level];
    }

    public void gameBegin()
    {
        level = 0;
        startTime = Time.time;
        gameOn = true;
        gameObject.BroadcastMessage("gameStart");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void RegisterEnemy(GameObject enemyObject)
    {
        Enemies enemy = enemyObject.GetComponent<Enemies>();
        if (enemy != null)
        {
            enemy.target = playerMan;
            enemy.playerMan = playerMan;
        }
    }
}
