using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BackgroundVisualEffect : MonoBehaviour
{
    [SerializeField] Gradient gradient;
    SpriteRenderer spriteRenderer;

    const int FIRST_MILESTONE = 100;
    int pointsToChange;
     private void Awake()
    {
        pointsToChange = FIRST_MILESTONE;
        spriteRenderer = GetComponent<SpriteRenderer>();
        PlayerManager.instance.GetScoreSystem().OnPointsChanged += BackgroundVisualEffect_OnPointsChanged;
    }

    private void BackgroundVisualEffect_OnPointsChanged(object sender, PointsSystem.OnPointsDataEventArgs e)
    {
        float darkness = ((float)e.CurrentPointsEventArgs / pointsToChange);
        spriteRenderer.color = gradient.Evaluate(darkness);
        if(darkness == 1)
        {
            //change background and light it up
            pointsToChange *= 2;
        }
    }

    

   
}
