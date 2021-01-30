using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using System;

public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField] Color enemyProjectileColor;
    [SerializeField] Color playerProjectileColor;
    static List<GameObject> projectileAliveList;

    public event EventHandler OnProjectileDestroyed;

    bool destroyByDistanceToPlayer;
    float maxDistance;

    Vector3 direction;
    float speed;
    public bool isItEnemyProjectile;
    bool setupReady;
    Rigidbody2D rigidBody2d;
    private void Awake()
    {
        rigidBody2d = GetComponent<Rigidbody2D>();
    }
    public void SetBehaviour(Vector3 dir, float speed, bool isitenemyprojectile, float autodestroytimer = 15f)
    {
        direction = dir;
        this.speed = speed;
        isItEnemyProjectile = isitenemyprojectile;
        Destroy(gameObject, autodestroytimer);

        if (isItEnemyProjectile)
        {
            GetComponent<SpriteRenderer>().color = enemyProjectileColor;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = playerProjectileColor;
            transform.localScale *= 2f;
            transform.GetComponent<CircleCollider2D>().radius /= 2f;
            gameObject.layer = 0;

        }
        setupReady = true;


    }
    void Start()
    {
        if (projectileAliveList == null) projectileAliveList = new List<GameObject>();
        projectileAliveList.Add(gameObject);
       
       //     Vector3 defaultScale = transform.localScale;
        //    transform.localScale = Vector3.zero;
        //    transform.DOScale(defaultScale, 5);
      
        
    }
    public void SetAutoDestroyByDistance(float maxdistance)
    {
        destroyByDistanceToPlayer = true;

        maxDistance = maxdistance;
    }
    // Update is called once per frame
    void Update()
    {
        if (setupReady)
        {
            rigidBody2d.AddForce(direction * speed, ForceMode2D.Force);
        }
        if (destroyByDistanceToPlayer)
        {
            if (Vector2.Distance(transform.position, PlayerManager.instance.transform.position) > maxDistance)
            {
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (setupReady)
        {
            GameObject collisionGO = collision.gameObject;
            if (collisionGO.CompareTag("Player") && isItEnemyProjectile)
            {
                PlayerManager playerManager = collisionGO.GetComponent<PlayerManager>();

                //Hit player and show visual effect
                Destroy(gameObject);

                SoundSystem.instance.PlaySound(SoundSystem.Sound.PlayerHurt);
                int scoreDamage = 20;
                MunizUtilities.TextPopUp.CreateTextPopUp("-" + scoreDamage + " points!", playerManager.transform.position, 2, Color.red);
                playerManager.powerChargeSystem.RemovePoints(scoreDamage);

                SoundSystem.instance.PlaySound(SoundSystem.Sound.ProjectileExplode);
                ParticleEffectFactory.instance.CreateParticleEffect(ParticleEffectFactory.Particle.RedBallsExplosion, transform.position);

            }
            else if (collisionGO.CompareTag("Enemy") && !isItEnemyProjectile)
            {
                //Destroy enemy and show effects
                Destroy(gameObject);

                Destroy(collisionGO);
                SoundSystem.instance.PlaySound(SoundSystem.Sound.ProjectileExplode);
                ParticleEffectFactory.instance.CreateParticleEffect(ParticleEffectFactory.Particle.EnemyDeath, collisionGO.transform.position);

            }

        }
    }
    public static void ClearAllProjectilesAlive()
    {
        if (projectileAliveList != null)
        {

            projectileAliveList.ForEach((projectile) => Destroy(projectile));

            projectileAliveList.Clear();
        }
    }

    void OnDestroy()
    {
        OnProjectileDestroyed?.Invoke(this, EventArgs.Empty);
        projectileAliveList.Remove(gameObject);

    }
}

