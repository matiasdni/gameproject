using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxManager : MonoBehaviour {
    
    public float damage;
    // healthManager script to call damage function
    public HealthManager healthManager;
    // enemy collider
    public Collider enemyCollider;
    // Flag to track whether the coroutine is currently running
    private bool _coroutineRunning = false;
    private void Start() {
        damage = GetComponentInParent<EnemyController>().damage;
        healthManager = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<HealthManager>();
        enemyCollider = gameObject.GetComponent<Collider>();
    }

    public void OnTriggerEnter(Collider other) {
        if (!other.gameObject.CompareTag("Player")) return;
        if (_coroutineRunning) return;
        
        // Start a coroutine that will keep doing damage to the player
        // while the enemy is in contact with the player
        StartCoroutine(KeepDoingDamage(other));
        _coroutineRunning = true;
    }

    private IEnumerator KeepDoingDamage(Collider player) {
        // Keep doing damage to the player every few seconds
        // while the enemy is in contact with the player
        while (enemyCollider.bounds.Intersects(player.bounds)) {
            healthManager.TakeDamage(damage);
            yield return new WaitForSeconds(2);
        }
        // Set the flag to false when the coroutine ends
        _coroutineRunning = false;
    }
}
