using System.Collections.Generic;
using UnityEngine;

public class FlowerLibrary : MonoBehaviour
{
    [SerializeField] private Flower[] flowersList;
    [SerializeField] private List<string> flowerUIDSpawn;
    [SerializeField] private FlowerCombination[] flowerCombinationList;

    Dictionary<string, Flower> flowerDictionary;

    public void Awake()
    {
        InitDictionary();
    }

    public void InitDictionary() {
        flowerDictionary = new Dictionary<string, Flower>();
        for (int i = 0; i < flowersList.Length; i++) {
            flowerDictionary.Add(flowersList[i].uid, flowersList[i]);
        }
    }

    public Flower GetFlower(string uid) {
        if (!flowerDictionary.ContainsKey(uid)) {
            Debug.Log("There are no Flower with that uid: " + uid);
            return null;
        }
        return flowerDictionary[uid];
    }

    public GameObject GetFlowerGO(string uid) {
        if (!flowerDictionary.ContainsKey(uid))
        {
            Debug.Log("There are no Flower with that uid: " + uid);
            return null;
        }
        return flowerDictionary[uid].gameObject;
    }


    public Flower GetRandomSpawnFlower() { 
        int indFlower = Random.Range(0, flowerUIDSpawn.Count);

        return GetFlower(flowerUIDSpawn[indFlower]);
    }

    public GameObject GetRandomSpawnFlowerGO() {
        int indFlower = Random.Range(0, flowerUIDSpawn.Count);

        return GetFlower(flowerUIDSpawn[indFlower]).gameObject;
    }


    public string GetUIDCombinationOfFlowers(string uid_one, string uid_two)
    {
        for (int i = 0; i < flowerCombinationList.Length; i++) {
            if (flowerCombinationList[i].uid_one == uid_one && flowerCombinationList[i].uid_two == uid_two)
                return flowerCombinationList[i].uid_result;

            if (flowerCombinationList[i].uid_one == uid_two && flowerCombinationList[i].uid_two == uid_one)
                return flowerCombinationList[i].uid_result;
        }

        return null;
    }
    public string GetUIDCombinationOfFlowers(Flower one, Flower two) {
        return GetUIDCombinationOfFlowers(one.uid, two.uid);
    }

}
