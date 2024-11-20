using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct EnemyWavePos {
    public string uid;
    public int count;
}

[CreateAssetMenu(fileName = "EnemyWaveDef", menuName = "Scriptable Objects/Enemy Wave", order = 1)]
public class EnemyWaveDef : ScriptableObject
{
    public List<EnemyWavePos> enemies;
}
