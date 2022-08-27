using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraLimits : MonoBehaviour {

    void Awake() {
        var camera = GetComponent<Camera>();

        var leftBottom = (Vector2)camera.ScreenToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
        var leftTop = (Vector2)camera.ScreenToWorldPoint(new Vector3(0, camera.pixelHeight, camera.nearClipPlane));
        var rightTop = (Vector2)camera.ScreenToWorldPoint(new Vector3(camera.pixelWidth, camera.pixelHeight, camera.nearClipPlane));
        var rightBottom = (Vector2)camera.ScreenToWorldPoint(new Vector3(camera.pixelWidth, 0, camera.nearClipPlane));
        
        CreateCollider(new[] {leftBottom, leftTop, leftBottom});
        CreateCollider(new[] {rightTop, rightBottom, rightTop});
    }

    private void CreateCollider(Vector2[] edgePoints) {
        var edgeCollider = gameObject.AddComponent<EdgeCollider2D>();
        edgeCollider.points = edgePoints;
    }
}
