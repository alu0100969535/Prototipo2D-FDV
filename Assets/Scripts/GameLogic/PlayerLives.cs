using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLives : MonoBehaviour {

    public Action<int> OnChange = delegate {};

    [SerializeField] private int startAmount = 3;
    [SerializeField] private int maxAmount = 3;

    public int CurrentLives {get; private set;}

    private void Awake() {
        CurrentLives = startAmount;
    }
 
    public void SetAmount(int amount) {
        CurrentLives = amount;
        OnChange(CurrentLives);
    }

    public void ConsumeLive() {
        CurrentLives--;
        OnChange(CurrentLives);
    }

    public void RestoreLive() {
        if(CurrentLives >= maxAmount) {
            return;
        }

        CurrentLives = startAmount;
        OnChange(CurrentLives);
    }
}
