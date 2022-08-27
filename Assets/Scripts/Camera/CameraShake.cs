using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraShake : MonoBehaviour {
    
    private Vector3 originalCameraPos;
    
    [SerializeField] private float shakeDuration = 1f;
    [SerializeField] private float shakeAmount = 0.2f;

    private bool canShake;
    private float timer;
    
    void Start() {
        originalCameraPos = transform.localPosition;
    }
    
    void Update() {
        if (canShake) {
            StartCameraShakeEffect();
        }
    }

    public void ShakeCamera() {
        canShake = true;
        timer = shakeDuration;
    }

    private void StartCameraShakeEffect() {
        if (timer > 0) {
            transform.localPosition = originalCameraPos + Random.insideUnitSphere * shakeAmount;
            timer -= Time.deltaTime;
        } else {
            timer = 0f;
            transform.position = originalCameraPos;
            canShake = false;
        }
    }

}
