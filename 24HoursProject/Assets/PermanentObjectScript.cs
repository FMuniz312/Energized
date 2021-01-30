using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentObjectScript : MonoBehaviour
{
    static PermanentObjectScript instance;
    int uniqueIndex;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }
}
