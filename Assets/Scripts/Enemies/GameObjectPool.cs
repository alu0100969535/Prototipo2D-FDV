using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool {

    private Queue<GameObject> queue = new Queue<GameObject>();

    Func<GameObjectPool, GameObject> CreateInstance;

    public GameObjectPool(Func<GameObjectPool, GameObject> createInstance, int initialInstances = 5) {
        this.CreateInstance = createInstance;
        
        for(var i = 0; i < initialInstances; i++) {
            CreateNewInstance();
        }
    }

    public GameObject Pop() {
        if(queue.Count == 0){
            CreateNewInstance();
        }

        var instance = queue.Dequeue();
        instance.SetActive(true);
        return instance;
    }

    public void Push(GameObject instance) {
        instance.SetActive(false);
        queue.Enqueue(instance);
    }

    private void CreateNewInstance() {
        var instance = CreateInstance(this);
        Push(instance);
    }
    
}