using System;
using Unity.VisualScripting;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public float maxHealth = 5f;
    public float currentHealth;
    // The amount of time in seconds that the player's material color should change for
    public float colorChangeDuration = 1f;
    
    [SerializeField] private Healthbar healthbar;
    
    // Renderer component of the player
    private Renderer _playerRenderer;
    
    // The original player color
    private Color _playerColor;
    
    // to keep track of how much time has passed since the player's health changed
    private float _timer;

    private bool _isPlayer = false;
    
    
    
    private void Start() {
        currentHealth = maxHealth;
        _isPlayer = true;
        _playerRenderer = GetComponent<Renderer>();
        _playerColor = _playerRenderer.material.color;
        healthbar.UpdateHealthBar(currentHealth, maxHealth);
    }
    
    private void Update() {
        if(currentHealth <= 0) HandleDeath();
        
        // Increment the timer by the time since the last frame
        // If the timer is greater than or equal to the color change duration, reset the timer
        // and revert the player's material color back to its original color
        if (!_isPlayer) return;
        if (!(_timer > 0)) return;
        _timer -= Time.deltaTime;
        if (_timer <= 0) {
            _playerRenderer.material.color = _playerColor;
        }
        // Change the color of the player's material
        _playerRenderer.material.color =
            Color.Lerp(_playerColor, Color.red, _timer / colorChangeDuration);

    }
    
    public void TakeDamage(float damage) {
        currentHealth -= damage;
        _timer = colorChangeDuration;
        healthbar.UpdateHealthBar(currentHealth, maxHealth);
    }
    
    public void Heal(float healAmount) {
        currentHealth += healAmount;
        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
        healthbar.UpdateHealthBar(currentHealth, maxHealth);
    }

    private void HandleDeath() {
        if (gameObject.CompareTag("Player")) {
            gameObject.SetActive(false);
            healthbar.gameObject.SetActive(false);
            WaveSpawner.Instance.Pause();
            WaveManager.instance.gameOver = true;
            TextTracker.instance.gameOver = true;
        }
        else {
            TextTracker.instance.score += 10 + WaveManager.instance.waveNumber * 10;
            WaveManager.instance.enemiesKilled++;
            Destroy(gameObject);
        }
    }
    
    public float GetHealth() {
        return currentHealth;
    }
    
    public float GetMaxHealth() {
        return maxHealth;
    }
    
    // Used to override the default health values, Heal method should be used for setting the _currentHealth
    public void SetHealth(float health) {
        maxHealth = health;
        currentHealth = maxHealth;
    }
}

