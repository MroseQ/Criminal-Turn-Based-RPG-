using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Parameters", menuName = "ScriptableObjects/SpawnManager", order = 1)]
public class SpawnScriptableObject : ScriptableObject
{
    public bool objectsMoved, battleOutcome;
    public string[] chars = new string[4];
}
