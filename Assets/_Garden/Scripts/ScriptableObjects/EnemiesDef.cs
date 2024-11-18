using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDef", menuName = "Scriptable Objects/EnemyDef", order = 1)]
public class EnemyDef : ScriptableObject
{
    public string uid;

    public GameObject prefab;
    public Sprite sprite;

    public string title;
    public string description;
}
