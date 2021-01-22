using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TutorialCanvasController : MonoBehaviour
{
    [SerializeField] Text step1Text;
    [SerializeField] Text step2Text;
    [SerializeField] Text step3Text;
    [SerializeField] GameObject lifeBarHolder;
    [SerializeField] GameObject powerBarHolder;
    [SerializeField] GameObject attackbuttonHolder;
    [SerializeField] Image lifeBarImage;
    [SerializeField] Image powerBarImage;
    [SerializeField] GameObject QuitTutorialButtonGO;
    [SerializeField] GameObject loadingScreenGO;
    [SerializeField] ProjectileBehaviour projectileBehaviour;
    EnemyBehaviour enemyBehaviour;

    const float TIMER_TO_FORM_SETENCE = 2.5f;
    const float TIMER_TO_FADE_SETENCE = 2f;

    
    bool CanShowFinishButton;


    private void Start()
    {
        projectileBehaviour.OnProjectileDestroyed += ProjectileBehaviour_OnProjectileDestroyed;
         ActivateStepOne();
    }

    private void EnemyBehaviour_OnEnemySpawnDestroyed(object sender, System.EventArgs e)
    {
        CanShowFinishButton = true;
    }

    private void ProjectileBehaviour_OnProjectileDestroyed(object sender, System.EventArgs e)
    {
        StartCoroutine(TutorialLogic(5));
    }

    void ActivateStepOne()
    {
        step1Text.DOFade(1, TIMER_TO_FADE_SETENCE);
        step1Text.DOText("Use your finger to dash\nthrough the energy ball\nin any direction", TIMER_TO_FORM_SETENCE);
    }

    void ActivateStepTwo()
    {
        step2Text.DOFade(1, TIMER_TO_FADE_SETENCE);
        step2Text.DOText("You will be thrown\nin the direction that\nyou dashed with you finger", TIMER_TO_FORM_SETENCE);

    }
    void ActivateStepThree()
    {
        step3Text.DOFade(1, TIMER_TO_FADE_SETENCE);
        step3Text.DOText("Each energy ball\nwill help you charge!", TIMER_TO_FORM_SETENCE);

    }

    //Step 4 is the animation of life energy bar
    void ActivateStepFour()
    {
        step1Text.DOFade(0, .5f);
        step2Text.DOFade(0, .5f);
        lifeBarHolder.SetActive(true);
        RectTransform rectTransform = lifeBarHolder.GetComponent<RectTransform>();

        rectTransform.DOScale(Vector3.one * 2.5f, 1f).OnComplete(() =>
        {
            const int timer = 3;
            TweenCallback fillImageCallBack = () =>
            {
                DOTween.To(() => lifeBarImage.fillAmount, (value) => lifeBarImage.fillAmount = value, .6f, 3);
                
            };


            rectTransform.DOAnchorPos(new Vector3(380, -100), timer / 2);
            rectTransform.DOScale(1, timer).OnComplete(fillImageCallBack);
        });

    }

    void ActivateStepFive()
    {
        
        step3Text.DOText("And you will also\ncharge your power attack!", TIMER_TO_FORM_SETENCE);
    }

    //Step 4 is the animation of power energy bar
    void ActivateStepSix()
    {
        powerBarHolder.SetActive(true);
        RectTransform rectTransform = powerBarHolder.GetComponent<RectTransform>();
        const int timer = 3;
        rectTransform.DOScale(Vector3.one, timer).OnComplete(() =>
        {
            DOTween.To(() => powerBarImage.fillAmount, (value) => powerBarImage.fillAmount = value, 1f, 3).OnComplete(() =>
            {
                attackbuttonHolder.SetActive(true);
                attackbuttonHolder.transform.DOScale(1, 1.5f);
                PlayerManager.instance.ForceEnergize();

            });

        });
    }

    void ActivateStepSeven()
    {
        enemyBehaviour = EnemyFactory.instance.SpawnEnemy(EnemyFactory.EnemyType.Normal, PlayerManager.instance.transform.position + Vector3.up * 20f).GetComponent<EnemyBehaviour>();
        enemyBehaviour.idleEnemy = true;
        enemyBehaviour.OnEnemySpawnDestroyed += EnemyBehaviour_OnEnemySpawnDestroyed;    
    }

    

    void ShowFinishTutorialButton()
    {
        QuitTutorialButtonGO.SetActive(true);
        QuitTutorialButtonGO.transform.DOScale(1, 1.5f);
    }

    IEnumerator TutorialLogic(float timerBtwTips)
    {


        ActivateStepTwo();
        yield return new WaitForSeconds(timerBtwTips);
        ActivateStepThree();
        yield return new WaitForSeconds(timerBtwTips + 1);
        ActivateStepFour();
        yield return new WaitForSeconds(timerBtwTips + 1);
        ActivateStepFive();
        yield return new WaitForSeconds(timerBtwTips*.7F);
        ActivateStepSix();
        yield return new WaitForSeconds(timerBtwTips);
        ActivateStepSeven();
        yield return new WaitUntil(()=>CanShowFinishButton);
        yield return new WaitForSeconds(2);
        ShowFinishTutorialButton();


    }
    public void GoBackToMenu()
    {
        ConfirmationPopUPHandler.instance.ShowPopUp(() => LoadingScreenBehaviour.instance.LoadSceneLogic(0));

    }
}
