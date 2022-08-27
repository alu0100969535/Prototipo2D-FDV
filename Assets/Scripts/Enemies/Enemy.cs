using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour {

    [SerializeField] private SuccessTrigger successTrigger;
    
    public Action<GameObject> OnPlayerFail = delegate {};

    private Collider2D playerCollider;
    private GameManager gameManager;

    private bool playerHadSuccess;

    private void Awake() {
        successTrigger.OnPlayerSuccess += OnPlayerSuccess;
    }

    public void Start() {
        playerHadSuccess = false;
    }

    public void Initialize(GameManager gameManager, Collider2D playerCollider) {
        this.gameManager = gameManager;
        this.playerCollider = playerCollider;
    }

    private void OnPlayerSuccess() {
        if (playerHadSuccess) {
            return;
        }
        gameManager.OnPlayerSuccess();
        playerHadSuccess = true;
    }

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.collider == playerCollider){
            OnPlayerCollision();
        }
    }

    private void OnPlayerCollision() {           
        gameManager.OnPlayerHit();
        OnPlayerFail(this.gameObject);
    }
    
}
