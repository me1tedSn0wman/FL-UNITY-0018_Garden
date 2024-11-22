using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;
using Utils.PoolControl;
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

    [SerializeField] public FlowerLibrary flowerLibrary;
    [SerializeField] public EnemyLibrary enemyLibrary;
    [SerializeField] public GameplayUpgradesLibrary gameplayUpgradesLibrary;

    [Header("Game Object Anchors")]
    [SerializeField] private Transform flowersAnchor;
    [SerializeField] private Transform enemiesAnchor;
    [SerializeField] private Transform centerFlowersAnchor;
    [SerializeField] public Transform projectileAnchor;

    [Header("Set Dynamically")]

    [SerializeField] private EnemyWaveDef[] enemyWaves;

    [SerializeField] private CenterFlower centerFlowerPrefab;

    [SerializeField] private float timeBetweenFlowerSpawns;
    [SerializeField] private float timeBetweenEnemiesSpawns;
    [SerializeField] private float timeBetweenWaves;

    [Header("Set Dynamically")]

    [SerializeField] private GameplayState _gameplayState;
    public GameplayState gameplayState {
        get { return _gameplayState; }
        set { _gameplayState = value; }
    }

    [SerializeField] private Flower selectedFlower;
    [SerializeField] public CenterFlower centerFlower;

    [SerializeField] public List<Flower> listOfCurrentFlowers;
    [SerializeField] public List<Enemy> listOfCurrentEnemies;

    [SerializeField] private float crntTimeFlowerSpawn;
    [SerializeField] private float crntTimeEnemiesSpawn;
    [SerializeField] private float crntTimeWaveSpawn;

    



    [Header("Variables")]

    public int money;
    public int health;
    public float gameTime;
    public int crntWave;

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
        Subscribe();
        InitVariables();
        InitTimers();

        gameplayState = GameplayState.Gameplay;
    }

    public void Update()
    {
        if (gameplayState == GameplayState.Gameplay)
        {
            TrySpawnFlowers();
            TrySpawnEnemies();
            TrySpawnWave();
            UpdateSelectedFlowerPos();

            ChangeGameTimeValue(Time.deltaTime);
        }
    }

    public void InitVariables() {
        money = 0;
        health = 5;
        gameTime = 0.0f;
        crntWave = 0;

        AddMoney(0);
        ChangeGameTimeValue(0);
        ChangeHealth(0);
    }

    public void InitTimers() {
        crntTimeFlowerSpawn = 0.0f;
        crntTimeEnemiesSpawn = 0.0f;
        crntTimeWaveSpawn = 0.0f;
    }

    public void Subscribe() {
        gameplayUIManager.OnPauseClicked += PauseGame;
        gameplayUIManager.OnResumeClicked += ResumeGame;
    }

    public void Unsubscribe() {
        gameplayUIManager.OnPauseClicked -= PauseGame;
        gameplayUIManager.OnResumeClicked -= ResumeGame;
    }

    public void SpawnCenterFlower() {
        centerFlower = Instantiate(centerFlowerPrefab);
        centerFlower.transform.position = boardManager.CenterOfGrid();
        centerFlower.transform.SetParent(centerFlowersAnchor);
    }

    public void SpawnFlower() {
        Vector3 pos = boardManager.GetRandomSpawnPos();

        Flower newFlower = Poolable.TryGetPoolable<Flower>(flowerLibrary.GetRandomSpawnFlowerGO());
        newFlower.transform.position = pos;
        newFlower.transform.SetParent(flowersAnchor);
        listOfCurrentFlowers.Add(newFlower);
        GameManager.Instance.soundLibrary.PlayOneShoot("spawnFlower");
    }

    public void RemoveFlowerFromList(Flower flower) {
        if (listOfCurrentFlowers.Contains(flower)) {
            listOfCurrentFlowers.Remove(flower);
        }
    }

    public void SpawnFlower(string uid, Vector3 pos) {
        Flower newFlower = Poolable.TryGetPoolable<Flower>(flowerLibrary.GetFlowerGO(uid));

        newFlower.transform.position = pos;
        newFlower.transform.SetParent(flowersAnchor);
        listOfCurrentFlowers.Add(newFlower);
        GameManager.Instance.soundLibrary.PlayOneShoot("spawnFlower");
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
        ReleaseFlower(flowerDragged);

        string newUID = flowerLibrary.GetUIDCombinationOfFlowers(flowerStation, flowerDragged);
        if (newUID != null)
        {
            SpawnFlower(newUID, pos);
            flowerDragged.Death();
            flowerStation.Death();
            GameManager.Instance.soundLibrary.PlayOneShoot("combineFlowers");
        }
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
        string uid = enemyLibrary.GetRandomEnemySpawnUID();
        SpawnEnemy(uid);
    }

    public void SpawnEnemy(string uid) { 
        Vector3 pos = boardManager.GetRandomOffScreenSpawnPos();
        SpawnEnemy(uid, pos);
    }

    public void SpawnEnemy(string uid, Vector3 pos) {
        Enemy newEnemy = Poolable.TryGetPoolable<Enemy>(enemyLibrary.GetEnemyGO(uid));
        
        newEnemy.transform.position = pos;
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

    public void TrySpawnWave() {
        if (crntTimeWaveSpawn > timeBetweenWaves) {
            crntTimeWaveSpawn = 0.0f;
            SpawnWave(crntWave);
            crntWave= Mathf.Min(crntWave+1, enemyWaves.Length-1);
            return;
        }
        crntTimeWaveSpawn += Time.deltaTime;
        return;
    }

    public void SpawnWave(int waveNum) {
        for (int i = 0; i < enemyWaves[waveNum].enemies.Count; i++) {
            for (int k = 0; k < enemyWaves[waveNum].enemies[i].count; k++) {
                SpawnEnemy(enemyWaves[waveNum].enemies[i].uid);
            }
        }
        GameManager.Instance.soundLibrary.PlayOneShoot("spawnWave");
    }

    public void AddMoney(int count) {
        money += count;
        gameplayUIManager.UpdateMoneyValue(money);
    }

    public void ChangeHealth(int value) {
        health += value;
        GameManager.Instance.soundLibrary.PlayOneShoot("centralFlowerDamage");


        if (health <= 0)
        {
            GameOver();
        }
        else
        {
            gameplayUIManager.UpdateHealthValue(health);
        }
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

    public void PauseGame() {
        gameplayState = GameplayState.Pause;
    }

    public void ResumeGame() {
        gameplayState = GameplayState.Gameplay;
    }
}
