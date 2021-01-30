using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BackgroundVisualEffect : MonoBehaviour
{
    [Header("Tweening")]
    [SerializeField] float mileStone;
    [SerializeField] float mileStoneMultiplier;


    [Header("Resource Income")]
    [SerializeField] Gradient gradient;
    [SerializeField] Sprite[] backGrounds;
    SpriteRenderer spriteRenderer;
    int backGroundIndex;

    const int FIRST_MILESTONE = 100;
    int pointsToChange;
     private void Awake()
    {
        pointsToChange = FIRST_MILESTONE;
        spriteRenderer = GetComponent<SpriteRenderer>();
        PlayerManager.instance.GetScoreSystem().OnPointsChanged += BackgroundVisualEffect_OnPointsChanged;
        GameManager.onGameReset += GameManager_onGameReset;
    }

    private void GameManager_onGameReset(object sender, System.EventArgs e)
    {
        backGroundIndex = 0;
        spriteRenderer.sprite = backGrounds[backGroundIndex];
        pointsToChange = FIRST_MILESTONE;

    }

    private void BackgroundVisualEffect_OnPointsChanged(object sender, PointsSystem.OnPointsDataEventArgs e)
    {
        if(backGroundIndex >= backGrounds.Length - 1) return;
        float darkness = ((float)e.CurrentPointsEventArgs / pointsToChange);
        spriteRenderer.color = gradient.Evaluate(darkness);
        if(darkness == 1   )
        {
            //change background and light it up
            pointsToChange *= 2;
            backGroundIndex++;
            spriteRenderer.sprite = backGrounds[backGroundIndex];
        }
    }

    

   
}
