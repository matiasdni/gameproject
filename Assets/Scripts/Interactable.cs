using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public bool inRange;
    public KeyCode interactKey;
    public UnityEvent interactAction;
    public int interactableCountMax;
    private int _interactableCount;


    private void Update() {
        if (!inRange) return;
        if(_interactableCount >= interactableCountMax) return;
        if (!Input.GetKeyDown(interactKey)) return;
        
        interactAction.Invoke();
        _interactableCount++;
    }


    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Player")) {
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.CompareTag("Player")) {
            inRange = false;
        }
    }
}
