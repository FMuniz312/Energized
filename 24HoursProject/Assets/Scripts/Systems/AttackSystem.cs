using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSystem
{
    public void Attack(IAttackTarget character,int damage)
    {
        character.GetHealthSystem().RemovePoints(damage);

    }    
}
