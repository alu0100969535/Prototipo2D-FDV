using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField] private PlayerMovement player;
    [SerializeField] private PlayerLives playerLives;
    [SerializeField] private PlayerScore playerScore;
    
    [SerializeField] private ParalaxManager paralaxManager;
    [SerializeField] private EnemiesManager enemiesManager;
    [SerializeField] private HUD hud;
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private AudioManager audioManager;

    [SerializeField] private float initialScrollSpeed = 1.0f;
    [SerializeField] private float maxScrollSpeed = 12.5f;

    [SerializeField] private float speedUpInterval = 5.0f;
    [SerializeField] private float scrollSpeedDelta = 0.15f;

    private float scrollSpeed;
    private float elapsedTimeSinceSpeedUp;
    private float playerSpeedMultiplier;
    private bool running = true;
    private bool isNewHighscore = false;

    public void OnPlayerHit() {
        playerLives.ConsumeLive();
        cameraShake.ShakeCamera();
        audioManager.PlayOnFail();
        
        if (playerLives.CurrentLives == 0) {
            GameOver();
        }
    }

    public void OnPlayerSuccess() {
        playerScore.AddPoint();
        if (playerScore.CurrentScore == playerScore.CurrentHighScore && !isNewHighscore) {
            audioManager.PlayOnNewHighScore();
            isNewHighscore = true;
        }
        else {
            audioManager.PlayOnSuccess();
        }
    }

    private void Awake() {
        NewGame();
        hud.OnPlayGameClicked += NewGame;
    }

    private void NewGame() {
        running = true;
        scrollSpeed = initialScrollSpeed;
        SetSpeed();
        playerLives.RestoreLive();
        playerScore.SetPoints(0);
        hud.ShowGameScreen();
        player.Alive();
        enemiesManager.StartSpawn();
    }

    private void GameOver() {
        running = false;
        scrollSpeed = 0;
        SetSpeed();
        player.Dead();
        enemiesManager.StopSpawn();
        hud.ShowLoseScreen();
        if (isNewHighscore) {
            audioManager.PlayOnLoseNewHighScore();
        } else {
            audioManager.PlayOnLose();
        }
    }

    private void FixedUpdate() {
        if (!running) {
            return;
        }
        
        if(ShouldSpeedUpScrollSpeed()) {
            SpeedUp();
        }

        if(ShouldMovePlayer()) {
            MoveGameObject(player.gameObject);
        }

        elapsedTimeSinceSpeedUp += Time.fixedDeltaTime;
    }

    private bool ShouldSpeedUpScrollSpeed() {
        return elapsedTimeSinceSpeedUp > speedUpInterval && scrollSpeed < maxScrollSpeed;
    }

    private void SpeedUp() {
        scrollSpeed += scrollSpeedDelta;

        if(scrollSpeed > maxScrollSpeed) { 
            scrollSpeed = maxScrollSpeed;
        }

        SetSpeed();
        elapsedTimeSinceSpeedUp = 0;
    }

    private void SetSpeed() {
        paralaxManager.SetScrollSpeed(scrollSpeed);

        if (playerSpeedMultiplier == 0) {
            playerSpeedMultiplier = player.MovementSpeed / initialScrollSpeed;
        }
        player.MovementSpeed = scrollSpeed * playerSpeedMultiplier;
    }

    private bool ShouldMovePlayer() {
        return scrollSpeed > 0;
    }

    private void MoveGameObject(GameObject gameObject) {
        var position = gameObject.transform.position;
        var delta =  scrollSpeed * Time.fixedDeltaTime;
        // Moving with trasform as we need to preserve velocity
        gameObject.transform.position = new Vector3(position.x - delta, position.y, position.z);
    }

}
