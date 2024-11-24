using UnityEngine;

[CreateAssetMenu(fileName = "InfoDef", menuName = "Scriptable Objects/InfoDef", order = 1)]
public class InfoDef : ScriptableObject
{
    public Sprite sprite;
    public string title;
    [TextArea(15,20)]
    public string description;
}
