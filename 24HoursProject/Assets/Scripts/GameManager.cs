using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class GameManager : MonoBehaviour
{[Header("Mechanics Balance")]
    [SerializeField] float timerMaxToEnemySpawn;
    [SerializeField] float timerMaxToBlackHoleSpawn;
    [SerializeField] float warningTime;
    [SerializeField] int maxAmountOfTargetPointsAlive;
    [SerializeField] int lessTargetAmountPerLevel;
    int amountOfTargetPointsAlive;

    [Header("Resource Income")]
    [SerializeField] GameMenuHandler GameMenuHandler;

    public static event EventHandler OnGamePauseStatusChange;
    public static event  EventHandler onGameReset;

    float timerSpawnEnemy;
    float timerSpawnBlackHole;
    public const float MAX_X_POS = 80;
    public const float MAX_Y_POS = 45;

    public const float timerBeforeGameStart = 4;

    public static bool GamePaused;

    void Start()
    {
        timerSpawnEnemy = 0;
        timerSpawnBlackHole = timerMaxToBlackHoleSpawn;
        PlayerManager.instance.lifeEnergySystem.OnPointsZero += LoseGame;
        GamePaused = true;
        StartCoroutine(LogicBeforeGameStart());
        OnGamePauseStatusChange += GameManager_OnGamePauseStatusChange;
        PlayerManager.instance.mapLevelSystem.OnPointsIncreased += MapLevelSystem_OnPointsIncreased;
    }

    private void MapLevelSystem_OnPointsIncreased(object sender, PointsSystem.OnPointsDataEventArgs e)
    {
       if(maxAmountOfTargetPointsAlive > 3) maxAmountOfTargetPointsAlive -= lessTargetAmountPerLevel;
    }

    private void GameManager_OnGamePauseStatusChange(object sender, EventArgs e)
    {
        StartCoroutine(LogicBeforeGameStart());

    }

    private void LoseGame(object sender, System.EventArgs e)
    {
        GamePaused = true;
        DataSerialization.Instance.Save();
        GameMenuHandler.CloseAllGameMenu();
        GameMenuHandler.OpenGameOverMenu();

    }
    static public void ResetGame()
    {
        //Clear enemies, reset player position, reset score
        EnemyBehaviour.ClearAllEnemiesAlive();
        BlackHoleBehaviour.ClearAllBlackHolesAlive();
        ProjectileBehaviour.ClearAllProjectilesAlive();
        PlayerManager.instance.ResetPlayer();
        onGameReset?.Invoke(default,  EventArgs.Empty);
        OnGamePauseStatusChange?.Invoke(default, EventArgs.Empty);


    }

    static public void PauseGameStatus(bool value)
    {
        GamePaused = value;
    }
    private void Update()
    {
        if (!GamePaused)
        {

            timerSpawnEnemy -= Time.deltaTime;
            timerSpawnBlackHole -= Time.deltaTime;
            if (timerSpawnEnemy <= 0)
            {
                timerSpawnEnemy += timerMaxToEnemySpawn;
                //Spawn enemy
                StartCoroutine(SpawningEnemyLogic());
            }
            if (timerSpawnBlackHole <= 0)
            {
                timerSpawnBlackHole += timerMaxToBlackHoleSpawn;
                //Spawn enemy

                StartCoroutine(SpawningBlackHoleLogic());


            }
        }
    }
    void FixedUpdate()
    {
        if (!GamePaused)
        {
            if(amountOfTargetPointsAlive == 0)
            {
                amountOfTargetPointsAlive++;
                ProjectileBehaviour projectileBehaviour = PrefabFactory.instance.CreateItem(PrefabFactory.FactoryProduct.JumpTarget, GetValidPosition(15f ,  15f)).GetComponent<ProjectileBehaviour>();
                projectileBehaviour.SetAutoDestroyByDistance(MAX_X_POS);
                projectileBehaviour.OnProjectileDestroyed += ProjectileBehaviour_OnProjectileDestroyed;
            }
            
            if (amountOfTargetPointsAlive < maxAmountOfTargetPointsAlive)
            {
                amountOfTargetPointsAlive++;
                ProjectileBehaviour projectileBehaviour = PrefabFactory.instance.CreateItem(PrefabFactory.FactoryProduct.JumpTarget, GetValidPosition(MAX_X_POS + 150, MAX_Y_POS + 50)).GetComponent<ProjectileBehaviour>();
                projectileBehaviour.SetAutoDestroyByDistance(MAX_X_POS);
                projectileBehaviour.OnProjectileDestroyed += ProjectileBehaviour_OnProjectileDestroyed;
            }

        }
    }

    private void ProjectileBehaviour_OnProjectileDestroyed(object sender, System.EventArgs e)
    {
        amountOfTargetPointsAlive--;
    }

    IEnumerator LogicBeforeGameStart()
    {
        yield return new WaitForSeconds(timerBeforeGameStart);
        GamePaused = false;
    }
    IEnumerator SpawningEnemyLogic()
    {
        Vector3 spawnPos = GetValidPositionCloseToPlayer();
        GameObject enemyGO = EnemyFactory.instance.SpawnEnemy(EnemyFactory.EnemyType.Normal, spawnPos);

        enemyGO.GetComponent<SpriteRenderer>().enabled = false;
        enemyGO.GetComponent<EnemyBehaviour>().enabled = false;
        //instantiate  spawn-warning sign
        ParticleEffectFactory.instance.CreateParticleEffect(ParticleEffectFactory.Particle.SpawnWarning, spawnPos);
        yield return new WaitForSeconds(warningTime);
        //spawn enemy
        enemyGO.GetComponent<SpriteRenderer>().enabled = true;
        enemyGO.GetComponent<EnemyBehaviour>().enabled = true;
    }
    IEnumerator SpawningBlackHoleLogic()
    {
        Vector3 spawnPos = GetValidPositionCloseToPlayer();

        //instantiate  spawn-warning sign
        ParticleEffectFactory.instance.CreateParticleEffect(ParticleEffectFactory.Particle.SpawnWarning, spawnPos);
        yield return new WaitForSeconds(warningTime);
        //spawn random
        PrefabFactory.instance.CreateItem(PrefabFactory.FactoryProduct.BlackHole, spawnPos);

    }

    IEnumerator SpawningJumpTarget()
    {
        Vector3 spawnPos = GetValidPositionCloseToPlayer();

        //instantiate  spawn-warning sign
        ParticleEffectFactory.instance.CreateParticleEffect(ParticleEffectFactory.Particle.SpawnWarning, spawnPos);
        yield return new WaitForSeconds(warningTime);
        //spawn random
        PrefabFactory.instance.CreateItem(PrefabFactory.FactoryProduct.JumpTarget, spawnPos);

    }

    Vector3 GetValidPositionCloseToPlayer()
    {
          
         return GetValidPosition(MAX_X_POS, MAX_Y_POS);
    }

    Vector3 GetValidPosition(float max_x, float max_y)
    {
        Vector3 randomPos;
        RaycastHit2D raycastHit2D;
        Vector3 playerPos = PlayerManager.instance.transform.position;
        do
        {
            randomPos = new Vector3(UnityEngine.Random.Range(-max_x, max_x) + playerPos.x, UnityEngine.Random.Range(-max_y, max_y) + playerPos.y);

            raycastHit2D = Physics2D.CircleCast(randomPos, 10, Vector2.zero);
        }
        while (raycastHit2D);

        return randomPos;
    }



}
