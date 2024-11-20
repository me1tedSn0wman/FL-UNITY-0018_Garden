using System.Collections.Generic;
using UnityEngine;

public class EnemyLibrary : MonoBehaviour
{
    [SerializeField] private Enemy[] enemyList;
    [SerializeField] private List<string> enemyUIDSpawn;

    Dictionary<string, Enemy> enemyDictionary;

    public void Awake()
    {
        InitDictionary();
    }

    public void InitDictionary() { 
        enemyDictionary = new Dictionary<string, Enemy>();
        for (int i = 0; i < enemyList.Length; i++) {
            enemyDictionary.Add(enemyList[i].uid, enemyList[i]);
        }
    }

    public Enemy GetEnemy(string uid)
    {
        if (!enemyDictionary.ContainsKey(uid)) {
            Debug.Log("There are no enemy with that uid: "+ uid);
            return null;
        }
        return enemyDictionary[uid];
    }

    public GameObject GetEnemyGO(string uid)
    {
        return GetEnemy(uid).gameObject;
    }

    public Enemy GetRandomEnemySpawn() { 
        return GetEnemy(GetRandomEnemySpawnUID());
    }

    public string GetRandomEnemySpawnUID() {
        int indEnemy = Random.Range(0, enemyUIDSpawn.Count);
        return enemyUIDSpawn[indEnemy];
    }
}