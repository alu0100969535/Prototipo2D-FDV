using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PoolObject : MonoBehaviour {

    public Action<GameObject> OnDespawnColliderExited = delegate {};

    private Collider2D targetCollider;

    public void SetDespawnCollider(Collider2D collider) {
        targetCollider = collider;
    }

    void OnTriggerExit2D(Collider2D collider) {
        if(collider == targetCollider) {
            OnDespawnColliderExited(this.gameObject);
        }
    }
}
