using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Game Balance")]
    [SerializeField] float timerBtwAttacks;

    [Header("Tweening")]
    [SerializeField] Color firstPhase;
    [SerializeField] Color SecondPhase;
    [SerializeField] Color thirdPhase;
    static Color currentColor;

    static float timerMax;
    float timer;
    public static int amountOfEnemiesAlive;
    public const int MAX_ENEMIES_ALIVE = 10;
    public static event System.EventHandler OnEnemySpawnChange;
    public event System.EventHandler OnEnemySpawnDestroyed;
    public static List<GameObject> enemiesAliveList;
    public bool idleEnemy { get; set; }

    const float PLAYER_MIN_DISTANCE = 10f;
    private void Start()
    {
        if (enemiesAliveList == null)
        {
            enemiesAliveList = new List<GameObject>();
            currentColor = firstPhase;
            timerMax = timerBtwAttacks;
        }
        enemiesAliveList.Add(this.gameObject);
        timer = timerMax;
        amountOfEnemiesAlive += 1;
         
        GetComponent<SpriteRenderer>().color = currentColor;

        OnEnemySpawnChange?.Invoke(this, System.EventArgs.Empty);
        GameManager.onGameReset += GameManager_onGameReset;
        PlayerManager.instance.mapLevelSystem.OnPointsIncreased += MapLevelSystem_OnPointsIncreased;
        try
        {
            float defaultScaleX = transform.localScale.x;
            transform.localScale = Vector3.zero;
            TweenCallback tweenCallback = () => transform.DOPunchScale(new Vector3(.4f, 1.6f), .4f);
            transform.DOScale(defaultScaleX, .2f).OnComplete(tweenCallback);
        }
        catch
        {

        }

    }

    private void MapLevelSystem_OnPointsIncreased(object sender, PointsSystem.OnPointsDataEventArgs e)
    {
        if (e.CurrentPointsEventArgs == 2)
        {
            timerMax *= .7f;
            currentColor = SecondPhase;
        }
        else if(e.CurrentPointsEventArgs == 3) currentColor = thirdPhase;

    }

    private void GameManager_onGameReset(object sender, System.EventArgs e)
    {
        timerMax = timerBtwAttacks;
        currentColor = firstPhase;
    }

    void Update()
    {
        if (!GameManager.GamePaused && !idleEnemy)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                timer += timerMax;
                //Instantiate attack

                ActivateProjectileLogic();

            }

            CheckPlayerNear();
        }
    }

    void ActivateProjectileLogic()
    {
        float randomX = Random.Range(-3f, 3f);
        float randomY = Random.Range(-3f, 3f);
        Vector3 spawnPos = transform.position + new Vector3(randomX, randomY, 0);
        int level = PlayerManager.instance.mapLevelSystem.currentPoints;
        switch (level)
        {

            case 3:
                {
                    ProjectileFactory.instance.CreateProjectile(spawnPos, PlayerManager.instance.GetDirectionToPlayer(spawnPos));
                    ProjectileFactory.instance.CreateProjectile(spawnPos, PlayerManager.instance.GetDirectionToPlayer(spawnPos));
                }; break;
            default: ProjectileFactory.instance.CreateProjectile(spawnPos, PlayerManager.instance.GetDirectionToPlayer(spawnPos)); break;
        }
    }

    void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, PlayerManager.instance.transform.position) > GameManager.MAX_X_POS + 40)
        {
            Destroy(gameObject);
        }

    }
    public static void ClearAllEnemiesAlive()
    {
        if (enemiesAliveList != null)
        {
            enemiesAliveList.ForEach((enemy) => Destroy(enemy));
            enemiesAliveList.Clear();
        }
    }
    private void CheckPlayerNear()
    {
        //If Player is too close, push him away and steal some energy
        float distance = Vector2.Distance(PlayerManager.instance.GetPlayerPosition(), transform.position);
        if (distance < PLAYER_MIN_DISTANCE)
        {
            //show visual warning
            PlayerManager.instance.DashMovement(PlayerManager.instance.GetDirectionToPlayer(transform.position), 5f);
        }
    }
    private void OnDestroy()
    {
        enemiesAliveList.Remove(this.gameObject);
        amountOfEnemiesAlive -= 1;
        OnEnemySpawnChange?.Invoke(this, System.EventArgs.Empty);
        OnEnemySpawnDestroyed?.Invoke(this, System.EventArgs.Empty);

    }



}
