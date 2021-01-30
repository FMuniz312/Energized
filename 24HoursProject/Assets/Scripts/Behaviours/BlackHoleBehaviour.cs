using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlackHoleBehaviour : MonoBehaviour
{
    Rigidbody2D playerRigidBody;
    Vector3 dir;
    [SerializeField] float pullForce;
    [SerializeField] float timerAutoDestroy;
    static  List<GameObject> listActiveBlackHole;
    private void Start()
    {
        if (listActiveBlackHole == null) listActiveBlackHole = new List<GameObject>();
        playerRigidBody = PlayerManager.instance.GetComponent<Rigidbody2D>();
        Destroy(gameObject, timerAutoDestroy);
        listActiveBlackHole.Add(this.gameObject);
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

    private void Update()
    {
        if (!GameManager.GamePaused)
        {
            dir = (transform.position - playerRigidBody.transform.position).normalized;

            playerRigidBody.AddForce(dir * pullForce, ForceMode2D.Force);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerManager playermanager = collision.GetComponent<PlayerManager>();
            int amountOfPointsLost = 20;
            playermanager.GetScoreSystem().RemovePoints(amountOfPointsLost);
            MunizUtilities.TextPopUp.CreateTextPopUp("-" + amountOfPointsLost + " score points!", transform.position);
            playerRigidBody.AddForce(-dir * 6, ForceMode2D.Impulse);
            ParticleEffectFactory.instance.CreateParticleEffect(ParticleEffectFactory.Particle.RedBallsExplosion, transform.position);
            Destroy(gameObject);
        }
    }
    public static void ClearAllBlackHolesAlive()
    {
        if (listActiveBlackHole != null)
        {
            listActiveBlackHole.ForEach((blackhole) => Destroy(blackhole));
            listActiveBlackHole.Clear();
        }
    }
    private void OnDestroy()
    {
        listActiveBlackHole.Remove(this.gameObject);
    }
}
