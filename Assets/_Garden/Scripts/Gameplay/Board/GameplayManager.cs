using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

using Random = UnityEngine.Random;

public enum GameplayState { 
    Pregame,
    SpawnGrid,
    Gameplay,
    Pause,
    GameOver
}

public class GameplayManager : Singleton<GameplayManager>
{
    [Header("GameObjects")]

    [SerializeField] private BoardManager boardManager;
    [SerializeField] public Camera mainCamera;
    [SerializeField] private PlayerControlManager playerControlManager;
    [SerializeField] public GameplayUIManager gameplayUIManager;

    [Header("Game Object Anchors")]
    [SerializeField] private Transform flowersAnchor;
    [SerializeField] private Transform enemiesAnchor;
    [SerializeField] private Transform centerFlowersAnchor;
    [SerializeField] public Transform projectileAnchor;

    [Header("Set Dynamically")]
    [SerializeField] private GameplayState _gameplayState;

    [SerializeField] private Flower[] flowersPrefabs;
    [SerializeField] private Enemy[] enemyPrefabs;
    [SerializeField] private CenterFlower centerFlowerPrefab;

    [SerializeField] private Flower selectedFlower;
    [SerializeField] public CenterFlower centerFlower;


    [SerializeField] private float crntTimeFlowerSpawn;
    [SerializeField] private float timeBetweenFlowerSpawns;

    [SerializeField] private float crntTimeEnemiesSpawn;
    [SerializeField] private float timeBetweenEnemiesSpawns;

    [SerializeField] private Flower resultFlowerPrefab;

    public GameplayState gameplayState {
        get { return _gameplayState; }
        set { _gameplayState = value; }
    }

    [SerializeField] private List<Flower> listOfCurrentFlowers;
    [SerializeField] private List<Enemy> listOfCurrentEnemies;

    [Header("Variables")]

    public int money;
    public int health;
    public float gameTime;

    /*
    public event Action<int> OnMoneyChange;
    public event Action<int> OnMoneyChange;
    public event Action<int> OnMoneyChange;
    */

    public override void Awake()
    {
        base.Awake();

        gameplayState = GameplayState.Pregame;

    }

    public void Start()
    {
        gameplayState = GameplayState.SpawnGrid;
        boardManager.Init();
        mainCamera.transform.position = boardManager.CenterOfGrid() + new Vector3(0,0,-10);
        SpawnCenterFlower();
        InitVariables();

        gameplayState = GameplayState.Gameplay;
    }

    public void Update()
    {
        if (gameplayState == GameplayState.Gameplay)
        {
            TrySpawnFlowers();
            TrySpawnEnemies();
            UpdateSelectedFlowerPos();

            ChangeGameTimeValue(Time.deltaTime);
        }
    }

    public void InitVariables() {
        money = 0;
        health = 5;
        gameTime = 0.0f;

        AddMoney(0);
        ChangeGameTimeValue(0);
        ChangeHealth(0);

    }

    public void Subscription() { 
        
    }

    public void SpawnCenterFlower() {
        centerFlower = Instantiate(centerFlowerPrefab);
        centerFlower.transform.position = boardManager.CenterOfGrid();
        centerFlower.transform.SetParent(centerFlowersAnchor);
    }

    public void SpawnFlower() {
        Vector3 pos = boardManager.GetRandomSpawnPos();

        int flowerInd = Random.Range(0, flowersPrefabs.Length);

        Flower newFlower = Instantiate(flowersPrefabs[flowerInd]);
        newFlower.transform.position = pos;
        newFlower.transform.SetParent(flowersAnchor);
        listOfCurrentFlowers.Add(newFlower);
    }

    public void RemoveFlowerFromList(Flower flower) {
        if (listOfCurrentFlowers.Contains(flower)) {
            listOfCurrentFlowers.Remove(flower);
        }
    }

    public void SpawnFlower(int id, Vector3 pos) {
        Flower newFlower = Instantiate(resultFlowerPrefab);
        newFlower.transform.position = pos;
        newFlower.transform.SetParent(flowersAnchor);
        listOfCurrentFlowers.Add(newFlower);
    }

    public void TrySpawnFlowers()
    {
        if (crntTimeFlowerSpawn > timeBetweenFlowerSpawns) {
            SpawnFlower();
            crntTimeFlowerSpawn = 0;
            return;
        }
        crntTimeFlowerSpawn += Time.deltaTime;
        return;
    }

    public void UpdateSelectedFlowerPos() {
        if (selectedFlower != null) {
            selectedFlower.immediatePos = playerControlManager.crntMouseWorldPositon;
        }
    }

    public void SetSelectedFlower(Flower flower) {
        if (selectedFlower == null)
        {
            selectedFlower = flower;
            selectedFlower.StartDragging();
        }
    }

    public void ReleaseFlower(Flower flower) {
        if (selectedFlower == flower) 
        {
            selectedFlower.ReleaseFlower();
            selectedFlower = null;
        }
    }

    public void TryCombineFlowers(Flower flowerDragged, Flower flowerStation, Vector3 pos) {
        flowerDragged.ReleaseFlower();
        SpawnFlower(0, pos);
        flowerDragged.Death();
        flowerStation.Death();
    }


    public void TrySpawnEnemies()
    {
        if (crntTimeEnemiesSpawn > timeBetweenEnemiesSpawns)
        {
            SpawnEnemy();
            crntTimeEnemiesSpawn = 0;
            return;
        }
        crntTimeEnemiesSpawn += Time.deltaTime;
        return;
    }

    public void SpawnEnemy() {
        Vector3 pos = boardManager.GetRandomSpawnPos();

        int enemyInd = Random.Range(0, enemyPrefabs.Length);
        Enemy newEnemy = Instantiate(enemyPrefabs[enemyInd]);
        newEnemy.transform.position = boardManager.GetRandomOffScreenSpawnPos();
        newEnemy.transform.SetParent(enemiesAnchor);
        newEnemy.moveAim = centerFlower.transform.position;
        listOfCurrentEnemies.Add(newEnemy);
    }

    public void RemoveEnemyFromList(Enemy enemy) {
        if (listOfCurrentEnemies.Contains(enemy)) {
            listOfCurrentEnemies.Remove(enemy);
        }
    }

    public Vector3 GetNearestEnemyPos() {
        float dist = float.MaxValue;
        Vector3 pos = Vector3.zero; 
        for (int i = 0; i < listOfCurrentEnemies.Count; i++) {

            float crntDist = (listOfCurrentEnemies[i].pos - centerFlower.pos).magnitude;
            if (crntDist < dist) {
                dist = crntDist;
                pos = listOfCurrentEnemies[i].pos;
            }
        }

        return pos;
    }

    public void AddMoney(int count) {
        money += count;
        gameplayUIManager.UpdateMoneyValue(money);
    }

    public void ChangeHealth(int value) {
        health += value;
        gameplayUIManager.UpdateHealthValue(health);
    }

    public void ChangeGameTimeValue(float value)
    {
        gameTime += value;
        gameplayUIManager.UpdateTimerValue(gameTime);
    }

    public void GameOver() {
        gameplayState = GameplayState.GameOver;
        gameplayUIManager.GameOver();
    }
}
