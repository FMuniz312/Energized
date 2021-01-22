using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DEBUGSCRIPT : MonoBehaviour
{
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {

            PlayerManager.instance.lifeEnergySystem.AddValue(10);
            PlayerManager.instance.powerChargeSystem.AddValue(10);

        }
    }
}
