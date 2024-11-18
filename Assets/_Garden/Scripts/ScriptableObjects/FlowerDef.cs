using UnityEngine;

[CreateAssetMenu(fileName = "FlowerDef", menuName = "Scriptable Objects/FlowerDef", order = 1)]
public class FlowerDef : ScriptableObject
{
    public string uid;

    public GameObject prefab;
    public Sprite sprite;

    public string title;
    public string description;
}
