using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour {
    
    public Action<int> OnScoreChange = delegate { };
    public Action<int> OnHighScoreChange = delegate { };

    [SerializeField] private int startAmount;
    [SerializeField] private int onSuccess = 1;
    [SerializeField] private int defaultHighScore = 50;

    public int CurrentScore {get; private set;}
    public int CurrentHighScore { get; private set;}

    private const string HIGHSCORE_PERSIST = "Highscore";

    private void Awake() {
        CurrentScore = startAmount;
        LoadHighScore();
    }
 
    public void AddPoints(int amount) {
        CurrentScore += amount;
        OnScoreChange(CurrentScore);
        if (CurrentScore > CurrentHighScore) {
            SetHighScore();
        }
    }
    
    public void AddPoint() {
        CurrentScore += onSuccess;
        OnScoreChange(CurrentScore);
        if (CurrentScore > CurrentHighScore) {
            SetHighScore();
        }
    }

    public void SetPoints(int amount) {
        CurrentScore = amount;
        OnScoreChange(CurrentScore);
        if (CurrentScore > CurrentHighScore) {
            SetHighScore();
        }
    }

    private void SetHighScore() {
        CurrentHighScore = CurrentScore;
        SaveHighScore();
        OnHighScoreChange(CurrentHighScore);
    }

    public void ClearHighScore() {
        CurrentHighScore = 0;
        SaveHighScore();
        OnHighScoreChange(CurrentScore);
    }

    private void LoadHighScore() {
        CurrentHighScore = PlayerPrefs.GetInt(HIGHSCORE_PERSIST, defaultHighScore);
    }

    private void SaveHighScore() {
        PlayerPrefs.SetInt(HIGHSCORE_PERSIST, CurrentHighScore);
    }

}
