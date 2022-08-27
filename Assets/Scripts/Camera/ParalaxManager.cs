using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxManager : MonoBehaviour {

    [SerializeField] PlayerMovement player;

    [SerializeField] GameObjectMovement[] backgrounds;
    [SerializeField] GameObjectMovement[] foregrounds;

    [SerializeField] float backgroundScrollSpeed = 0.5f;
    [SerializeField] float foregroundScrollSpeed = 1.0f; 

    private float cachedMovement = 0.0f;

    void Awake() {
        foreach(var background in backgrounds) {
            background.SetScrollSpeed(0);
        }
        
        foreach(var foreground in foregrounds) {
            foreground.SetScrollSpeed(0);
        }
        
    }

    public void SetScrollSpeed(float movement) {
        foreach(var background in backgrounds) {
            background.SetScrollSpeed(backgroundScrollSpeed * -movement);
        }
        
        foreach(var foreground in foregrounds) {
            foreground.SetScrollSpeed(foregroundScrollSpeed * -movement);
        }
    }

}
