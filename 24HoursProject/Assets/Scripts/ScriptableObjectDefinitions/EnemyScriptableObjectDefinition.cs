using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "Enemy")]
public class EnemyScriptableObjectDefinition : ScriptableObject
{
    [Header("Game Balance")]
    public float timerBtwAttacks;

    [Header("Tweening")]
    public Color firstPhase;
    public Color SecondPhase;
    public Color thirdPhase;
}
