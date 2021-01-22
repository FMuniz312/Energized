using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FingerRepManager : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    [SerializeField] float dashForce;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            try
            {
                GameObject target = playerManager.GetJumpTarget().gameObject;

                if (collision.gameObject == target)
                {
                    //Player Dash
                    playerManager.DashMovement(dashForce);
                    SoundSystem.instance.PlaySound(SoundSystem.Sound.PlayerDash);
                    Destroy(target);
                    ParticleEffectFactory.instance.CreateParticleEffect(ParticleEffectFactory.Particle.RedBallsExplosion, transform.position);



                    ProjectileBehaviour projectileBehaviour = target.GetComponent<ProjectileBehaviour>();
                    if (projectileBehaviour != null)
                    {
                        if (projectileBehaviour.isItEnemyProjectile)
                        {
                            int score = Random.Range(2, 5);
                            MunizUtilities.TextPopUp.CreateTextPopUp("+" + score + " points!", target.transform.position);

                            playerManager.powerChargeSystem.AddValue(10);
                            playerManager.lifeEnergySystem.AddValue(14);
                            playerManager.PlayerScoreAddPoints(score);
                        }

                    }

                    Camera cameraMain = Camera.main;
                    cameraMain.transform.position = playerManager.transform.position + new Vector3(0, 0, -10);

                    cameraMain.DOShakePosition(.02f);
                }
            }
            catch
            {
                Debug.Log("something wrong in the FingerRep trigger");
            }
        }
    }
}
