using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectMovement : MonoBehaviour {

    [Header("Scroll")]
    [SerializeField] float scrollSpeed = 1.0f;

    public void SetScrollSpeed(float scrollSpeed) {
        this.scrollSpeed = scrollSpeed;
    }

    private void FixedUpdate() {

        if(scrollSpeed == 0) {
            return;
        }

        var position = gameObject.transform.position;
        gameObject.transform.position = new Vector3(position.x + scrollSpeed * Time.fixedDeltaTime, position.y, position.z);

    }

}
