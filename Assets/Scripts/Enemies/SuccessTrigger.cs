using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SuccessTrigger : MonoBehaviour {

    public Action OnPlayerSuccess = delegate {};
    
    void OnTriggerExit2D(Collider2D collider) {
        OnPlayerSuccess();
    }
}
