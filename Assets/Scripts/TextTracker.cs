using System;
using System.Collections;
using UnityEngine;


public class TextTracker : MonoBehaviour {
    public bool wavesStartTextDisplayed = false;
    public float reloadTime;
    public bool reloading = false;
    public float score = 0;
    public bool gameOver = false; 
    public static TextTracker instance { get; private set; }
    private void Awake() {
        if (instance != null && instance != this) { 
            Destroy(this); 
        } 
        else { 
            instance = this; 
        } 
    }


    private TextTracker() {
    }


    public IEnumerator Reload(float time) {
        if (reloading) yield break;
        reloadTime = time;
        reloading = true;
        yield return new WaitForSeconds(reloadTime);
        reloading = false;
    }
    
    public void CancelReload() {
        StopAllCoroutines();
        reloading = false;
    }
}