using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    
    public event Action OnPlayGameClicked = delegate { };
    
    [SerializeField] private PlayerLives playerLives;
    [SerializeField] private PlayerScore playerScore;

    [SerializeField] private Text livesCounter;
    [SerializeField] private Text highScoreCounter;
    [SerializeField] private Text scoreCounter;

    [SerializeField] private GameObject loseBanner;
    [SerializeField] private Text finalScore;
    [SerializeField] private GameObject newRecordPivot;

    public void ShowLoseScreen() {
        livesCounter.gameObject.SetActive(false);
        highScoreCounter.gameObject.SetActive(false);
        scoreCounter.gameObject.SetActive(false);
        loseBanner.SetActive(true);
        UpdateFinalScoreCounter(playerScore.CurrentScore);
        newRecordPivot.SetActive(playerScore.CurrentScore == playerScore.CurrentHighScore);
    }

    public void ShowGameScreen() {
        livesCounter.gameObject.SetActive(true);
        highScoreCounter.gameObject.SetActive(true);
        scoreCounter.gameObject.SetActive(true);
        loseBanner.SetActive(false);
    }

    private void Awake() {
        ShowGameScreen();
    }

    private void Start() {
        playerLives.OnChange += UpdateLivesCounter;
        UpdateLivesCounter(playerLives.CurrentLives);

        playerScore.OnHighScoreChange += UpdateHighScoreCounter;
        UpdateHighScoreCounter(playerScore.CurrentHighScore);

        playerScore.OnScoreChange += UpdateScoreCounter;
        UpdateScoreCounter(playerScore.CurrentScore);
    }

    private void UpdateLivesCounter(int lives) {
        livesCounter.text = "Vidas: " + lives;
    }

    private void UpdateHighScoreCounter(int score) {
        highScoreCounter.text = "HighScore: " + score;
    }
    
    private void UpdateScoreCounter(int score) {
        scoreCounter.text = "Score: " + score;
    }

    private void UpdateFinalScoreCounter(int score) {
        finalScore.text = "Your Score: " + score;
    }

    [UsedImplicitly]
    public void OnPlayButtonClicked() {
        OnPlayGameClicked();
    }
}
