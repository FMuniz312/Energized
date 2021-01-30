using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using DG.Tweening;


public class ConfirmationPopUPHandler : MonoBehaviour
{
    [SerializeField] Button yesButton;
    [SerializeField] Button noButton;
    [SerializeField] GameObject popUpGO;
    [SerializeField] Text warningText;
    public static ConfirmationPopUPHandler instance;
    Tweener idTweener;
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void ShowPopUp(UnityAction onyesclickaction, UnityAction onnoclickaction, string warningtext = "Are you Sure?")
    {
        popUpGO.SetActive(true);
        warningText.DOText(warningtext,.5f,true,ScrambleMode.Lowercase);
        yesButton.onClick.AddListener(onyesclickaction);
        yesButton.onClick.AddListener(ClosePopUpConfirmation);
        noButton.onClick.AddListener(onnoclickaction);
        noButton.onClick.AddListener(ClosePopUpConfirmation);
        try
        {
            if (!idTweener.IsActive() || idTweener == null) idTweener = popUpGO.GetComponent<RectTransform>().DOPunchScale(Vector3.one * 0.5f, .5f, 10, .2f);
        }

        catch
        {

        }

    }

    public void ShowPopUp(UnityAction onyesclickaction, string warningtext = "Are you Sure?")
    {
        ShowPopUp(onyesclickaction, NoActionFunction, warningtext);
    }


    void ClosePopUpConfirmation()
    {
        try
        {
            Vector3 defaultScale = popUpGO.transform.localScale;
            TweenCallback tweenCallback = () =>
            {
                popUpGO.GetComponent<RectTransform>().localScale = defaultScale;
                popUpGO.SetActive(false);

            };


            popUpGO.GetComponent<RectTransform>().DOScale(0f, .5f).OnComplete(tweenCallback);
        }
        catch
        {

        }
        popUpGO.SetActive(false);
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();

    }
    void NoActionFunction()
    {

    }

}
