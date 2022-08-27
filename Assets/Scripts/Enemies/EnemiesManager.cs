using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemiesManager : MonoBehaviour {
    
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private Collider2D despawnCollider;

    [SerializeField] private Transform enemiesParent;
    [SerializeField] private float spawnDelay = 2.0f;
    [SerializeField] private float lowEnemyBias = 0.5f;
    
    [SerializeField] private EnemyInfo lowEnemy;
    [SerializeField] private EnemyInfo highEnemy;
    
    private float elapsedTimeSinceLastSpawn = 0;

    private bool isRunning = true;

    private GameObjectPool lowEnemyPool;
    private GameObjectPool highEnemyPool;

    private void Awake() {
        lowEnemyPool = new GameObjectPool(CreateNewLowEnemyInstance);
        highEnemyPool = new GameObjectPool(CreateNewHighEnemyInstance);
    }

    public void StopSpawn() {
        isRunning = false;
    }

    public void StartSpawn() {
        isRunning = true;
        elapsedTimeSinceLastSpawn = 0;
    }

    private GameObject CreateNewLowEnemyInstance(GameObjectPool pool) {
        return CreateNewEnemyInstance(pool, lowEnemy.prefab);
    }
    
    private GameObject CreateNewHighEnemyInstance(GameObjectPool pool) {
        return CreateNewEnemyInstance(pool, highEnemy.prefab);
    }

    private GameObject CreateNewEnemyInstance(GameObjectPool pool, GameObject prefab) {
        var instance = GameObject.Instantiate(prefab);

        void BackToPool(GameObject gameObject) {
            pool.Push(gameObject);
        };
        
        var poolObj = instance.GetComponent<PoolObject>();
        poolObj.SetDespawnCollider(despawnCollider);
        poolObj.OnDespawnColliderExited += BackToPool;

        var enemy = instance.GetComponent<Enemy>();
        enemy.Initialize(gameManager, playerCollider);
        enemy.OnPlayerFail += BackToPool;

        instance.transform.parent = enemiesParent;
        return instance;
    }
    
    private void FixedUpdate() {
        if (!isRunning) {
            return;
        }
        
        if (ShouldSpawnEnemy()) {
            var isLowEnemy = Random.Range(0,2) < lowEnemyBias;

            if (isLowEnemy) {
                SpawnLowEnemy();
            }
            else {
                SpawnHighEnemy();
            }
        }

        elapsedTimeSinceLastSpawn += Time.fixedDeltaTime;
    }

    private bool ShouldSpawnEnemy() {
        return elapsedTimeSinceLastSpawn > spawnDelay;
    }
    
    private void SpawnLowEnemy() {
        SpawnEnemy(lowEnemyPool, lowEnemy.spawnPoint);
    }
    
    private void SpawnHighEnemy() {
        SpawnEnemy(highEnemyPool, highEnemy.spawnPoint);
    }

    private void SpawnEnemy(GameObjectPool pool, Transform spawnPoint) {
        var enemy = pool.Pop();
        enemy.GetComponent<Enemy>().Start();
        
        enemy.transform.position = spawnPoint.position;
        elapsedTimeSinceLastSpawn = 0;
    }
    
    [Serializable]
    private struct EnemyInfo {
        public GameObject prefab;
        public Transform spawnPoint;
    }

}
