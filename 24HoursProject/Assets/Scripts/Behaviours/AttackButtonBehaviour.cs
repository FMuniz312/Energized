using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class AttackButtonBehaviour : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    [SerializeField] GameObject attackButton;
    [SerializeField] Sprite standBySprite;

    [SerializeField] bool canAttack;
    Tweener idTweener;

    private void Start()
    {
        playerManager.powerChargeSystem.OnPointsMax += OnEnergyCharged;
        AnimationActiveState(false);
        GameManager.onGameReset += ResetAttackButton; ;

    }

    private void ResetAttackButton(object sender, System.EventArgs e)
    {
        canAttack = false;
        AnimationActiveState(canAttack);
    }

    private void OnEnergyCharged(object sender, PointsSystem.OnPointsDataEventArgs e)
    {
        canAttack = true;
        AnimationActiveState(canAttack);
        if (!idTweener.IsActive() || idTweener == null) attackButton.transform.DOShakePosition(.3f);
    }
    void AnimationActiveState(bool canattack)
    {
        if (canattack)
        {
            attackButton.GetComponent<ImageAnimation>().enabled = true;

        }
        else
        {
            attackButton.GetComponent<ImageAnimation>().enabled = false;
            attackButton.GetComponent<Image>().sprite = standBySprite;
        }

    }
     

    public void PlayerAttack()
    {
        if (canAttack)
        {
            if (playerManager.IsThereEnemyClose())
            {
                Vector3 enemyTargetPos = playerManager.GetEnemyTarget().transform.position;
                Vector3 direction = -playerManager.GetDirectionToPlayer(enemyTargetPos);
                ProjectileFactory.instance.CreateProjectile(playerManager.GetPlayerPosition(), direction, 8, false);
                canAttack = false;
                playerManager.ResetAttackAbility();
                SoundSystem.instance.PlaySound(SoundSystem.Sound.PlayerAttack);

                AnimationActiveState(false);
            }

        }
    }
}
