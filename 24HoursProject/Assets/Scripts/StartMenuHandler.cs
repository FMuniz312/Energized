using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using DG.Tweening;

public class StartMenuHandler : MonoBehaviour
{

    
    private void Start()
    {
         
    }
    public void StartGame()
    {
        //Start the game
        LoadingScreenBehaviour.instance.LoadSceneLogic(1);

    }
    public void LoadTutorial()
    {
        LoadingScreenBehaviour.instance.LoadSceneLogic(2);
    }

}
