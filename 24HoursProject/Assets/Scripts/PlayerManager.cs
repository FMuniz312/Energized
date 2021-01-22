using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class PlayerManager : MonoBehaviour
{

    Transform jumpTargetTransform;
    Transform enemyTargetTransform;
    [SerializeField] GameObject fingerRep;
    [SerializeField] LayerMask enemyProjectileLayerMask;
    [SerializeField] LayerMask enemyLayerMask;


    public PointsSystem powerChargeSystem { get; private set; }
    public PointsSystem lifeEnergySystem { get; private set; }
    PointsSystem scoreSystem;
    int highScore;

    [SerializeField] GameObject jumpWarningParticlePrefab;
    [SerializeField] GameObject enemyWarningParticlePrefab;

    float timerLifeEnergy;


    GameObject copyJumpWarningParticle;
    GameObject copyEnemyWarningParticle;

    public bool isCharged { get; private set; }

    [SerializeField] float MinDistance;
    Vector3 defaultSize;
    Rigidbody2D rigidBody2D;
    static public PlayerManager instance;
    private void Awake()
    {
        defaultSize = transform.localScale;
        powerChargeSystem = new PointsSystem(100);
        lifeEnergySystem = new PointsSystem(60, 60);
        scoreSystem = new PointsSystem();
        highScore = DataSerialization.Instance.SaveDataContainer.PlayerSaveData.highScoreValue;
        powerChargeSystem.OnPointsMax += Energized;
        InstantiatePermanentParticleEffects(ref copyEnemyWarningParticle, enemyWarningParticlePrefab);
        InstantiatePermanentParticleEffects(ref copyJumpWarningParticle, jumpWarningParticlePrefab);
        rigidBody2D = GetComponent<Rigidbody2D>();
        if (instance == null) instance = this;

    }
    private void Energized(object sender, PointsSystem.OnPointsDataEventArgs e)
    {
        isCharged = true;
        
    }

    void Update()
    {

        VisualTouchManager();
        ParticleWarning(enemyTargetTransform, copyEnemyWarningParticle);
        ParticleWarning(jumpTargetTransform, copyJumpWarningParticle);

        if (!GameManager.GamePaused)
        {
            ChoosingJumpTarget();
            if (isCharged)
            {
                ChoosingEnemy();
            }
            timerLifeEnergy -= Time.deltaTime;
            if (timerLifeEnergy < 0)
            {
                timerLifeEnergy += 1;
                lifeEnergySystem.RemovePoints(6);
            }
        }


    }
    public void ForceEnergize()
    {
        powerChargeSystem.AddValue(powerChargeSystem.maxPoints);
    }
    void InstantiatePermanentParticleEffects(ref GameObject copyholder, GameObject prefab)
    {
        copyholder = Instantiate(prefab, transform.position, Quaternion.identity);
        if (copyholder.activeSelf) copyholder.SetActive(false);
    }
    void ChoosingJumpTarget()
    {

        RaycastHit2D[] castInfo = Physics2D.CircleCastAll(transform.position, MinDistance, Vector2.zero, 0, enemyProjectileLayerMask);
        RaycastHit2D targetRayCastInfo = new RaycastHit2D();
        if (castInfo != null)
        {
            float distance = Mathf.Infinity;
            foreach (RaycastHit2D item in castInfo)
            {
                float currentDistance = Vector2.Distance(item.collider.gameObject.transform.position, transform.position);
                if (currentDistance < distance)
                {
                    distance = currentDistance;
                    targetRayCastInfo = item;
                }

            }
            if (targetRayCastInfo != false) jumpTargetTransform = targetRayCastInfo.collider.gameObject.transform;
        }
    }
    void ChoosingEnemy()
    {

        RaycastHit2D[] castInfo = Physics2D.CircleCastAll(transform.position, Mathf.Infinity, Vector2.zero, 0, enemyLayerMask);
        RaycastHit2D targetRayCastInfo = new RaycastHit2D();
        if (castInfo != null)
        {
            float distance = Mathf.Infinity;
            foreach (RaycastHit2D item in castInfo)
            {
                float currentDistance = Vector2.Distance(item.collider.gameObject.transform.position, transform.position);
                if (currentDistance < distance)
                {
                    distance = currentDistance;
                    targetRayCastInfo = item;
                }

            }
            if (targetRayCastInfo != false) enemyTargetTransform = targetRayCastInfo.collider.gameObject.transform;
        }
    }
    void VisualTouchManager()
    {
        //Player touched the screen and is moving the finger in any direction
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Moved)
        {
            if (!fingerRep.activeSelf) fingerRep.SetActive(true);
            //If the player touches the screen and moves the finger through the projectile, the character should move
            Vector3 fingerpos = Input.GetTouch(0).position;

            Vector3 worldFingerPos = Camera.main.ScreenToWorldPoint(new Vector3(fingerpos.x, fingerpos.y, 10));

            fingerRep.transform.position = worldFingerPos;

        }
        else
        {
            if (fingerRep.activeSelf) fingerRep.SetActive(false);

        }
    }
    public void ResetAttackAbility()
    {
        isCharged = false;
        powerChargeSystem.ResetPoints();
    }
    public void PlayerScoreAddPoints(int points)
    {
        scoreSystem.AddValue(points);
        if (scoreSystem.currentPoints > highScore)
        {
            highScore = scoreSystem.currentPoints;
            DataSerialization.Instance.SaveDataContainer.PlayerSaveData.highScoreValue = highScore;

        }
        string text = "+" + points.ToString() + " points";
        MunizUtilities.TextPopUp.CreateTextPopUp(text, (transform.position + Vector3.up * 1.5f), 2, Color.green, 30f);
    }
    public void PlayerScoreResetPoints()
    {
        scoreSystem.ResetPoints();
    }
    void ParticleWarning(Transform targettransform, GameObject particlecopygameobject)
    {
        if (targettransform != null)
        {
            particlecopygameobject.SetActive(true);
            particlecopygameobject.transform.position = targettransform.position;
        }
        else
        {
            particlecopygameobject.SetActive(false);
        }
    }
    public void DashMovement(float dashforce)
    {
        try
        {
            Vector2 fingerDir = Input.touches[0].deltaPosition.normalized;
            rigidBody2D.AddForce(fingerDir * dashforce, ForceMode2D.Impulse);
            transform.localScale = defaultSize;
            transform.DOPunchScale(fingerDir * 5, 1f);

        }
        catch
        {

        }
    }
    public void DashMovement(Vector3 direction, float dashforce)
    {
        try
        {

            rigidBody2D.AddForce(direction * dashforce, ForceMode2D.Impulse);
        }
        catch
        {

        }
    }
    public void ResetPlayer()
    {
        scoreSystem.ResetPoints();
        powerChargeSystem.ResetPoints();
        lifeEnergySystem.AddValue(lifeEnergySystem.maxPoints);
        enemyWarningParticlePrefab.SetActive(false);
        jumpWarningParticlePrefab.SetActive(false);
        GetComponent<TrailRenderer>().enabled = false;
        transform.position = Vector3.zero;
        GetComponent<TrailRenderer>().enabled = true;
    }
    public Transform GetJumpTarget()
    {
        return jumpTargetTransform;
    }
    public GameObject GetEnemyTarget()
    {
        if (enemyTargetTransform.gameObject != null) { return enemyTargetTransform.gameObject; }
        return null;
    }
    public Vector3 GetPlayerPosition()
    {
        return transform.position;
    }
    public Vector3 GetDirectionToPlayer(Vector3 position)
    {
        return (transform.position - position).normalized;
    }
    public PointsSystem GetScoreSystem()
    {
        return scoreSystem;
    }
    public int GetHighScore()
    {
        return highScore;
    }
}
