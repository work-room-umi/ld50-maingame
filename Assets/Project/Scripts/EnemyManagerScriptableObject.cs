using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnemyManagerScriptableObject", order = 1)]
public class EnemyManagerScriptableObject : ScriptableObject
{
    public List<GameObject> prefabs;
    public int numberOfPrefabsToCreate;
    public float spawnDist;

    [Tooltip("ゲーム開始時の最初のスポーンまでの秒数[s]")]
    public float firstSpawnDeley = 60f;

    [Tooltip("スポーンの間隔の秒数[s]")]
    public float spawnInterval = 1f;
}
