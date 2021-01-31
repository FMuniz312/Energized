using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BackgroundVisualEffect : MonoBehaviour
{
    [Header("Tweening")]
    [SerializeField] int mileStone;
    [SerializeField] int mileStoneMultiplier;


    [Header("Resource Income")]
    [SerializeField] Gradient gradient;
    [SerializeField] Sprite[] backGrounds;
    SpriteRenderer spriteRenderer;
    int backGroundIndex;


    int pointsToChange;
    private void Awake()
    {
        pointsToChange = mileStone;
        spriteRenderer = GetComponent<SpriteRenderer>();
        PlayerManager.instance.GetScoreSystem().OnPointsChanged += BackgroundVisualEffect_OnPointsChanged;
        GameManager.onGameReset += GameManager_onGameReset;

        
    }

    private void GameManager_onGameReset(object sender, System.EventArgs e)
    {
        backGroundIndex = 0;
        spriteRenderer.sprite = backGrounds[backGroundIndex];
        pointsToChange = mileStone;

    }

    private void BackgroundVisualEffect_OnPointsChanged(object sender, PointsSystem.OnPointsDataEventArgs e)
    {if(PlayerManager.instance.GetScoreSystem().currentPoints >= pointsToChange)PlayerManager.instance.mapLevelSystem.AddValue(1);
        if (backGroundIndex >= backGrounds.Length - 1) return;
        float darkness = ((float)e.CurrentPointsEventArgs / pointsToChange);
        spriteRenderer.color = gradient.Evaluate(darkness);
        if (darkness >= 1)
        {
            //change background and light it up
            pointsToChange *= mileStoneMultiplier;
            backGroundIndex++;
           
            spriteRenderer.sprite = backGrounds[backGroundIndex];
            spriteRenderer.color = gradient.Evaluate(((float)e.CurrentPointsEventArgs / pointsToChange));
        }
    }




}
