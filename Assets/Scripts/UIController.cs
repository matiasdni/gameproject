using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI startWavesText, scoreText, reloadingText, deathScoreText;
    public GameObject gameOverPanel;
    public bool wavesStarted = false;
    public bool gameOver = false;
    private bool _coroutineStarted = false;
    public bool reloading = false;

    private void Update() {
        if(TextTracker.instance.reloading) 
            StartCoroutine(ReloadingText());
        if (!TextTracker.instance.gameOver) return;
        gameOverPanel.gameObject.SetActive(true);
        startWavesText.gameObject.SetActive(false);
        reloadingText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        deathScoreText.text = "Score: " + TextTracker.instance.score;
    }

    public void StartWaves() {
        wavesStarted = true;
        StartCoroutine(StartWavesText());
    }
    private void Start() {
        startWavesText.gameObject.SetActive(false);
        reloadingText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        gameOverPanel.gameObject.SetActive(false);
    }

    private void LateUpdate() {
        if(wavesStarted)
            scoreText.gameObject.SetActive(true);
        UpdateScore();
    }

    private IEnumerator StartWavesText() {
        startWavesText.gameObject.SetActive(true);
        wavesStarted = true;
        var text = "Waves Starting in";
        string[] dots = {".", "..", "..."};
        
        for(var i = 0; i < 3; i++) {
            startWavesText.text = text + dots[i];
            yield return new WaitForSeconds(0.75f);
        }

        text += dots[2];
        
        for(var i = 0; i < 3; i++) {
            startWavesText.text = text + " " + (3 - i);
            yield return new WaitForSeconds(1);
        }
        startWavesText.text = "Good Luck!";
        yield return new WaitForSeconds(2);
        startWavesText.gameObject.SetActive(false);

        TextTracker.instance.wavesStartTextDisplayed = true;
    }

    private IEnumerator ReloadingText(float reloadTime = 3f) {
        if(_coroutineStarted) yield break;
        _coroutineStarted = true;
        reloadingText.gameObject.SetActive(true);
        const string text = "Reloading";
        string[] dots = {".", "..", "..."};
        var i = 0;
        while (TextTracker.instance.reloading) {
            if(i==3) i = 0;
            reloadingText.text = text + dots[i];
            i++;
            yield return new WaitForSeconds(reloadTime/3);
        }
        reloadingText.gameObject.SetActive(false);
        _coroutineStarted = false;
    }

    private void UpdateScore() {
            scoreText.text = "Score: " + TextTracker.instance.score;
    }
}
