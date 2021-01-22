using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssetsKeeper : MonoBehaviour
{
    static public GameAssetsKeeper instance;
    public GameObject prefabTextPopUp;
    private void Awake()
    {
        if (instance == null) instance = this;
    }
}
