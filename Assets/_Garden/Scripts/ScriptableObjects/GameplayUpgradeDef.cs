using UnityEngine;

[CreateAssetMenu(fileName = "GameplayUpgradeDef_", menuName = "Scriptable Objects/GameplayUpgradeDef", order = 1)]
public class GameplayUpgradeDef : ScriptableObject
{
    public string uid;

    public string title;
    [TextArea]
    public string description;

    public int priceBase;
    public int priceStep;

    public int valuePerLevel;

    public Sprite sprite;
    public int maxLevel;
}
