using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour {
    public static WaveManager instance { get; private set; }
    private static WaveSpawner WaveSpawner;
    
    // variables
    public int waveNumber = 0;
    public int enemiesPerWave = 10;
    public int enemiesAlive = 0;
    public int enemiesKilled = 0;
    public float timeBetweenWaves = 10f;
    public float timeBetweenEnemies = 1.2f;
    public int enemiesToSpawn = 0;
    [SerializeField] private bool coroutineRunning = false;
    [SerializeField] private bool waveStarted = false;
    private int enemiesSpawned = 0;
    [SerializeField]private bool spawningWave = false;
    public bool gameOver = false;
    [SerializeField] private float difficultyMultiplier;
    private void Awake() {
        if (instance != null && instance != this) { 
            Destroy(this); 
        } else { 
            instance = this; 
        } 
    }

    private void Start() {
        WaveSpawner = WaveSpawner.Instance;
        difficultyMultiplier = 1 + (waveNumber * 0.1f);
    }

    private void Update() {
        if(!coroutineRunning && !WaveSpawner.Paused()) {
            StartCoroutine(StartWaves());
        }
    }

    private IEnumerator StartWaves() {
        if(coroutineRunning) yield break;
        coroutineRunning = true;
        Debug.Log("WAITING FOR UNPAUSE");
        while (WaveSpawner.Paused()) yield return null;
        while (!TextTracker.instance.wavesStartTextDisplayed) yield return null;
        if (!waveStarted) {
            Debug.Log("NO PREVIOUS WAVE DETECTED RUNNING, STARTING ONE");
            waveStarted = true;
            Debug.Log("CALLING SPAWNWAVE()");
            StartCoroutine(SpawnWave());
            while(spawningWave) yield return null;
            Debug.Log("WAITING FOR THE PLAYER TO KILL ALL ENEMIES");
            while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0) {
                yield return null;
            }
        }

        Debug.Log("WAITING FOR THE PLAYER TO KILL ALL ENEMIES");
        while(GameObject.FindGameObjectsWithTag("Enemy").Length > 0) yield return null;
        Debug.Log("END OF STARTWAVES, INCREMENTING WAVENUMBER AND CALLING FUNCTION");
        waveNumber++;
        WaveSpawner.currentWaveCount = waveNumber;
        coroutineRunning = false;
        waveStarted = false;
    }
    
    private IEnumerator SpawnWave(int wave = 0) {
        difficultyMultiplier = 1 + (wave * 2);
        if (wave < 0) wave = 0;
        SetSpawnTimes(wave);
        spawningWave = true;
        timeBetweenEnemies = 1.2f / difficultyMultiplier;
        enemiesToSpawn = (int)(enemiesPerWave * difficultyMultiplier);
        enemiesAlive = enemiesToSpawn;
        
        for (var i = 0; i < (30+waveNumber*10); i++) {
            SpawnEnemy();
            yield return new WaitForSeconds(timeBetweenEnemies);
        }

        Debug.Log("breaking out of coroutine spawnwave()");
        spawningWave = false;
    }

    private void SpawnEnemy() {
        ++enemiesSpawned;
        Vector3 spawnPosition = WaveSpawner.spawnPositions[Random.Range(0, WaveSpawner.spawnPositions.Length)];
        Debug.Log("INSTANTIATING ENEMY");
        GameObject enemy = Instantiate(WaveSpawner.enemyPrefab, spawnPosition, Quaternion.identity);
        var enemyController = enemy.GetComponent<EnemyController>();
        var enemyHManager = enemy.GetComponent<HealthManager>();
        enemyHManager.SetHealth(enemyHManager.GetMaxHealth() * difficultyMultiplier);
        enemyController.damage *= difficultyMultiplier;
        if(enemiesSpawned % 2 == 0) enemyController.speed = difficultyMultiplier * 2 * enemyController.speed;
    }
    
    public void Resume() {
        WaveSpawner.Init();
        StartCoroutine(StartWaves());
        WaveSpawner.Resume();
    }
    
    private void SetSpawnTimes(int wave) {
        timeBetweenEnemies = 1.2f;
        if(wave == 0) {
            return;
        }
        if (!(timeBetweenEnemies > 0.3f)) return;
        timeBetweenEnemies -= 0.1f * wave;
    }
}
