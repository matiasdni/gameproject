using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Image healthBar;
    public GameObject player;
    private Camera _camera;
    
    private void Start() {
        _camera = Camera.main;
    }

    private void Update() {
        transform.LookAt(_camera.transform);
        transform.position = player.transform.position + new Vector3(0, 1.8f, 0);
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth) => healthBar.fillAmount = currentHealth / maxHealth;
}
