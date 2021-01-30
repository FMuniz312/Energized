using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class CanvasPlayerHUD : MonoBehaviour
{
    float lifeEnergyFillAmount;
    float powerEnergyFillAmount;
    [SerializeField] PlayerManager playerManager;
    [SerializeField] Image powerEnergyBarImage;
    [SerializeField] Image lifeEnergyBarImage;
    [SerializeField] GameObject lifeEnergyBarHolder;
    [SerializeField] RectTransform lifeEnergyWarningRectTransform;

    PointsSystem energySystemAnimatedVersion;

    [SerializeField] Vector3 lifeEnergyBarHolderDefaultPosition;
    [SerializeField] Vector3 lifeEnergyBarHolderStartPosition;
    bool playerCharged;

    private void Start()
    {
        energySystemAnimatedVersion = new PointsSystem();
        energySystemAnimatedVersion.OnPointsChanged += EnergySystemAnimatedVersion_OnPointsChanged;
        playerManager.powerChargeSystem.OnPointsChanged += EnergySystem_OnPointsChanged;
        powerEnergyBarImage.fillAmount = 0;
        lifeEnergyFillAmount = 1;
        lifeEnergyBarImage.fillAmount = lifeEnergyFillAmount;
        energySystemAnimatedVersion.AddValue(playerManager.lifeEnergySystem.maxPoints);
        playerManager.lifeEnergySystem.OnPointsChanged += LifeEnergySystem_OnPointsChanged;
        playerManager.lifeEnergySystem.OnPointsIncreased += LifeEnergySystem_OnPointsIncreased;
        SpecialEffectsAtStart();


    }

    private void EnergySystemAnimatedVersion_OnPointsChanged(object sender, PointsSystem.OnPointsDataEventArgs e)
    {
        lifeEnergyBarImage.fillAmount = lifeEnergyFillAmount;

    }

    void SpecialEffectsAtStart()
    {
        lifeEnergyWarningRectTransform.DOScale(1.5f, GameManager.timerBeforeGameStart);
        lifeEnergyWarningRectTransform.gameObject.GetComponent<Text>().DOFade(0, GameManager.timerBeforeGameStart * .8f).OnComplete(() =>
          {
              Destroy(lifeEnergyWarningRectTransform.gameObject);
              RectTransform rectTransformBarHolder = lifeEnergyBarHolder.GetComponent<RectTransform>();
              rectTransformBarHolder.anchoredPosition = lifeEnergyBarHolderStartPosition;
              rectTransformBarHolder.transform.localScale = Vector3.one * 2.5f;
              rectTransformBarHolder.DOAnchorPos(lifeEnergyBarHolderDefaultPosition, GameManager.timerBeforeGameStart / 2);
              rectTransformBarHolder.DOScale(1, GameManager.timerBeforeGameStart);
          });



    }
    private void LifeEnergySystem_OnPointsIncreased(object sender, PointsSystem.OnPointsDataEventArgs e)
    {
        lifeEnergyBarImage.rectTransform.anchoredPosition = Vector3.zero;
        lifeEnergyBarImage.rectTransform.DOPunchAnchorPos((lifeEnergyBarImage.rectTransform.anchoredPosition + new Vector2(0, 1)) * 20, .2f);
    }

    private void LifeEnergySystem_OnPointsChanged(object sender, PointsSystem.OnPointsDataEventArgs e)
    {
        float beforeChange = lifeEnergyFillAmount;
        lifeEnergyBarImage.fillAmount = beforeChange;

        lifeEnergyFillAmount = (float)e.CurrentPointsEventArgs / playerManager.lifeEnergySystem.maxPoints;

        DOTween.To(() => lifeEnergyBarImage.fillAmount, (value) => lifeEnergyBarImage.fillAmount = value, lifeEnergyFillAmount, .5f);


        if (lifeEnergyFillAmount <= .25f)
        {
            string text = "Life energy at " + (lifeEnergyFillAmount * 100).ToString("F0") + "%!";
            MunizUtilities.TextPopUp.CreateTextPopUp(text, (playerManager.gameObject.transform.position + Vector3.up * 1.5f), 2, Color.yellow, 50f, 2f);
        }


    }

    private void EnergySystem_OnPointsChanged(object sender, PointsSystem.OnPointsDataEventArgs e)
    {
        if (!playerManager.isCharged)
        {
            float beforeChange = powerEnergyFillAmount;
            powerEnergyBarImage.fillAmount = beforeChange;

            powerEnergyFillAmount = (float)e.CurrentPointsEventArgs / playerManager.powerChargeSystem.maxPoints;

            DOTween.To(() => powerEnergyBarImage.fillAmount, (value) => powerEnergyBarImage.fillAmount = value, powerEnergyFillAmount, .5f);


            if (powerEnergyFillAmount >= .8f)
            {
                string text = "Energy at " + powerEnergyFillAmount * 100 + "%!";
                MunizUtilities.TextPopUp.CreateTextPopUp(text, (playerManager.gameObject.transform.position + Vector3.down * 1.5f), 2, Color.yellow, 50f, 2f);
            }

        }

    }




}
