using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
	public GameObject victoryScreen;

    public SpawnList[] waves;

    public int currentWave = 0;
    public int currentElement = 0;
    public int currentCount = 0;
	public int maxTimer;
	int timer;

    public float timeBetweenWaves;
    public float firstWaveTimer;
    public float laterWaveTimer;
    bool waiting;
    static int enemyCount;
    static int thisWave;

    SpawnElement curSe;

    Text timerText;
    Text waveText;

	EnemyManager enemyManager;

    public GameObject[] spawnPos;

    public Toggle skipToggle;

    public Text countdownText;


    void Start()
    {
		timer = maxTimer;
		enemyManager = GetComponent<EnemyManager>();
		
        waiting = true;
        timerText = GameObject.Find("Timer").GetComponent<Text>();
        waveText = GameObject.Find("WaveText").GetComponent<Text>();
        enemyCount = 0;
        thisWave = 0;
        waveText.text = "Wave: " + (thisWave + 1).ToString();
        for (int i = 0; i < waves[thisWave].wave.Length; i++)
        {
            enemyCount += waves[thisWave].wave[i].count;
        }
        StartCoroutine("SpawnWait");
    }

    IEnumerator SpawnWait()
    {
        waiting = true;
        yield return new WaitForSeconds(firstWaveTimer);
        waiting = false;
        StartCoroutine("SpawnEnemy");
    }

    void Update()
    {
        if (waiting && Input.GetKeyDown(KeyCode.Return))
        {
            StopCoroutine("SpawnWait");
            StopCoroutine("WaitForWave");
            StartCoroutine("SpawnEnemy");
			waiting = false;
        }
    }

    IEnumerator SpawnEnemy()
    {
        curSe = waves[currentWave].wave[currentElement];

        if (curSe.enemy)
        {
            GameObject g = curSe.enemy as GameObject;
            if (curSe.health > 0)
            {
                g.GetComponent<Enemy>().Health = curSe.health;
            }
            for (int i = 0; i < spawnPos.Length; i++)
            {
                if (spawnPos[i] != null)
                {
                    enemyManager.AddEnemy(Instantiate(g, spawnPos[i].transform.position, Quaternion.identity) as GameObject);
                    g.transform.position = spawnPos[i].transform.position;
                    g.GetComponent<NavMeshAgent>().Warp(spawnPos[i].transform.position);
                }
            }
        }
        yield return new WaitForSeconds(curSe.delay);
        NextSpawn();
    }

    void NextSpawn()
    {
        if (currentCount + 1 < waves[currentWave].wave[currentElement].count)
        {
            currentCount++;
        }
        else
        {
            if (currentWave < waves.GetLength(0))
            {
                if (currentElement + 1 < waves[currentWave].wave.GetLength(0))
                {
                    currentElement++;
                    currentCount = 0;
                }
                else
                {
                    currentWave++;
                    currentElement = 0;
                    currentCount = 0;
                    return;
                }
            }
            else
            {
                if (currentElement + 1 < waves[currentWave].wave.GetLength(0))
                {
                    currentElement++;
                    currentCount = 0;
                }
                else
                {
                    return;
                }
            }
        }
        if (currentWave < waves.Length)
            StartCoroutine("SpawnEnemy");
    }

    public void EnemyDead()
    {
        enemyCount--;
        if (enemyCount <= 0)
        {          
            GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
            foreach (GameObject tower in towers)
            {
                if (tower.GetComponent<CombatTower>())
                {
                    tower.GetComponent<CombatTower>().AmmoRestock();
                }
            }

            if (thisWave >= waves.Length)
            {
                victoryScreen.SetActive(true);
                Time.timeScale = 0;
            }
            if (GameObject.FindObjectOfType<AnimationStop>() != null)
            {
                GameObject.FindObjectOfType<AnimationStop>().StartAni();
            }
            if (GameObject.FindObjectOfType<RotatingObj>() != null)
            {
                GameObject.FindObjectOfType<RotatingObj>().Rotation();
            }
            StartCoroutine("WaitForWave");
        }
    }

    public int GetWave()
    {
        return thisWave;
    }

    IEnumerator WaitForWave()
    {
		
        try
        {
            timer = maxTimer;
            thisWave++;
            for (int i = 0; i < waves[thisWave].wave.Length; i++)
            {
                enemyCount += waves[thisWave].wave[i].count;
                timer += (int)waves[thisWave].wave[i].delay * (int)waves[thisWave].wave[i].count;
            }
            if (spawnPos.Length != 0)
            {
                enemyCount *= spawnPos.Length;
            }
            waveText.text = "Wave: " + (thisWave + 1).ToString();
            GameObject.Find("WaveSound").GetComponent<AudioSource>().Play();
        }
        catch
        {

        }
        yield return new WaitForSeconds(timeBetweenWaves);              
        yield return new WaitForSeconds(1f);
        waiting = false;
        StartCoroutine("SpawnEnemy");
    }
}
