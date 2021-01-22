
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class EnemyFactory : MonoBehaviour
{
    [SerializeField] EnemyData[] enemiesData;
    public static EnemyFactory instance;
    
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public enum EnemyType
    {
        Normal
    }

    public GameObject SpawnEnemy(EnemyType enemyType)
    {
        return SpawnEnemy(enemyType, Vector3.zero);
    }
    public GameObject SpawnEnemy(EnemyType enemyType, Vector3 position)
    {
        

        return Instantiate(GetEnemyPrefab(enemyType), position, Quaternion.identity);
    }


    GameObject GetEnemyPrefab(EnemyType enemyType)
    {
        return enemiesData.Where(p => p.enemyType == enemyType).Select(p => p.enemyPrefab).FirstOrDefault();
    }


    [System.Serializable]
    public class EnemyData
    {
        public GameObject enemyPrefab;
        public EnemyType enemyType;
    }

  

}
