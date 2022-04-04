using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnemyManagerScriptableObject", order = 1)]
public class EnemyManagerScriptableObject : ScriptableObject
{
    public List<GameObject> prefabs;
    public int numberOfPrefabsToCreate;
    public float spawnDist;
}
