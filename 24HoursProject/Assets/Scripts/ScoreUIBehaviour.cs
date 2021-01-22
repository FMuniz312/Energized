using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class ScoreUIBehaviour : MonoBehaviour
{
    [SerializeField] Text scoreText;
    [SerializeField] PlayerManager playerManager;
    PointsSystem playerScoreSystem;
    const string PREP_TEXT = "Points: ";

    Vector3 scoreTextDefaultPos;
    void Start()
    {
        scoreTextDefaultPos = scoreText.rectTransform.anchoredPosition;
        playerScoreSystem = playerManager.GetScoreSystem();
        scoreText.text = PREP_TEXT + playerScoreSystem.currentPoints.ToString();
        playerScoreSystem.OnPointsChanged += PlayerScoreSystem_OnPointsChanged;
    }

    private void PlayerScoreSystem_OnPointsChanged(object sender, PointsSystem.OnPointsDataEventArgs e)
    {
        scoreText.text = PREP_TEXT + e.CurrentPointsEventArgs.ToString();
 
         scoreText.rectTransform.anchoredPosition = scoreTextDefaultPos;
         scoreText.rectTransform.DOShakePosition(.4f);

    }


}
