using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class GameMenuHandler : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuGO;
    [SerializeField] GameObject gameOverMenuGO;
 
    [SerializeField] Text highScoreText;
    Tweener idTweener;
    Vector3 pauseMenuScaledefault;

    private void Start()
    {
        pauseMenuScaledefault = pauseMenuGO.transform.localScale;

    }
    public void OpenPauseMenu()
    {
        pauseMenuGO.SetActive(true);
        GameManager.PauseGameStatus(true);
        try
        {
            pauseMenuGO.transform.localScale = pauseMenuScaledefault;
            if (!idTweener.IsActive() || idTweener == null) pauseMenuGO.GetComponent<RectTransform>().DOPunchScale(Vector3.one * 0.5f, .5f, 10, .2f);
        }

        catch
        {

        }
    }
    public void OpenGameOverMenu()
    {
        gameOverMenuGO.SetActive(true);
        highScoreText.DOText(DataSerialization.Instance.SaveDataContainer.PlayerSaveData.highScoreValue.ToString(),2,true,ScrambleMode.Numerals);
    }
    public void ClosePauseMenu()
    {
        GameManager.PauseGameStatus(false);
        Vector3 scale = pauseMenuGO.transform.localScale;
        TweenCallback tweenCallback = () =>
        {
            pauseMenuGO.GetComponent<RectTransform>().localScale = scale;
            pauseMenuGO.SetActive(false);

        };


        pauseMenuGO.GetComponent<RectTransform>().DOScale(0f, .5f).OnComplete(tweenCallback);

    }
    public void ResetTheGame()
    {
        GameManager.ResetGame();

        CloseGameOverMenu();
    }
    public void CloseGameOverMenu()
    {
        gameOverMenuGO.SetActive(false);
    }
    public void CloseAllGameMenu()
    {
        pauseMenuGO.SetActive(false);
        gameOverMenuGO.SetActive(false);
    }
    public void QuitGameRequest()
    {
        ConfirmationPopUPHandler.instance.ShowPopUp(QuitGameFunction,"Are you sure?");
    }
    void QuitGameFunction()
    {
        Application.Quit();

    }

}
