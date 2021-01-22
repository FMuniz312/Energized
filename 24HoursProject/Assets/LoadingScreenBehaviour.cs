using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using DG.Tweening;

public class LoadingScreenBehaviour : MonoBehaviour
{
    Vector3 hidingPos;
    [SerializeField] GameObject loadingPanelGO;
    public static LoadingScreenBehaviour instance { get; private set; }

    public void Awake()
    {
        if (instance == null) instance = this;
        hidingPos = new Vector3(0, 1300, 0);
    }
    public void LoadSceneLogic(int index)
    {
        loadingPanelGO.SetActive(true);
 
        TweenCallback loadSceneCall = () =>
        {
            SceneManager.LoadScene(index);
            gameObject.GetComponent<RectTransform>().DOAnchorPos(hidingPos, .7f).OnComplete(() => loadingPanelGO.SetActive(false));

           
        };
        TweenCallback JumpCall = () => gameObject.GetComponent<RectTransform>().DOJumpAnchorPos(gameObject.GetComponent<RectTransform>().anchoredPosition, 15, 2, 1.5f).OnComplete(loadSceneCall);

        gameObject.GetComponent<RectTransform>().DOAnchorPos(Vector3.zero, .7f).OnComplete(JumpCall);
 

    }
}
