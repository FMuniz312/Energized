using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DEBUGSCRIPT : MonoBehaviour
{
    
     
    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.E))
        {

            PlayerManager.instance.lifeEnergySystem.AddValue(10);
            PlayerManager.instance.powerChargeSystem.AddValue(10);
            PlayerManager.instance.PlayerScoreAddPoints(50);

        }
        if (Input.GetKeyDown(KeyCode.Q))
        {

            PlayerManager.instance.lifeEnergySystem.AddValue(10);
            PlayerManager.instance.powerChargeSystem.AddValue(10);
            PlayerManager.instance.GetScoreSystem().RemovePoints(50);

        }
#endif
    }
}
