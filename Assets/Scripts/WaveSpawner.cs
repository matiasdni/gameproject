using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveSpawner : MonoBehaviour {
    public static WaveSpawner Instance;
    
    private void Awake() {
        if (Instance != null && Instance != this) { 
            Destroy(this); 
        } 
        else { 
            Instance = this; 
        } 
    }
    
    public enum SpawningState
    {
        Spawning,
        Waiting,
        Done
    };
    
    public SpawningState state;
    
    public GameObject enemyPrefab;
    
    // number of enemies to spawn in current wave
    public int enemiesToSpawn;
    
    // The initial number of enemies to spawn in the first wave
    public int enemiesPerWave;
    
    public float timeBetweenEnemies;

    public Vector3[] spawnPositions;

    public int currentWaveCount;
    
    [SerializeField] private int enemiesSpawned;

    // flag to manage the spawning of enemies
    [SerializeField] private bool isPaused;

    public void Init() {
        state = SpawningState.Waiting;
        enemiesPerWave = WaveManager.instance.enemiesPerWave; 
        timeBetweenEnemies = WaveManager.instance.timeBetweenEnemies;
        currentWaveCount = WaveManager.instance.waveNumber;
    }

    public void Pause() {
            isPaused = true;
    }
    
    public void Resume() {
            isPaused = false;
    }

    public bool Paused() => isPaused;
}