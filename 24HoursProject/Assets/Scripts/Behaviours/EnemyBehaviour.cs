using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] float timerBtwAttacks;
    float timer;
    public static int amountOfEnemiesAlive;
    public const int MAX_ENEMIES_ALIVE = 10;
    public static event System.EventHandler OnEnemySpawnChange;
    public  event System.EventHandler OnEnemySpawnDestroyed;
    public static List<GameObject> enemiesAliveList;
    public bool idleEnemy { get; set; }

    const float PLAYER_MIN_DISTANCE = 10f;
    private void Start()
    {
        if (enemiesAliveList == null) enemiesAliveList = new List<GameObject>();
        enemiesAliveList.Add(this.gameObject);
        timer = timerBtwAttacks;
        amountOfEnemiesAlive += 1;


        OnEnemySpawnChange?.Invoke(this, System.EventArgs.Empty);

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
    void Update()
    {
        if (!GameManager.GamePaused && !idleEnemy)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                timer += timerBtwAttacks;
                //Instantiate attack
                float randomX = Random.Range(-3f, 3f);
                float randomY = Random.Range(-3f, 3f);
                Vector3 spawnPos = transform.position + new Vector3(randomX, randomY, 0);

                ProjectileFactory.instance.CreateProjectile(spawnPos, PlayerManager.instance.GetDirectionToPlayer(spawnPos), 3);
            }


            CheckPlayerNear();
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
