using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentObjectScript : MonoBehaviour
{
    static int index;
    int uniqueIndex;
    void Awake()
    {
        uniqueIndex = index;
        index++;
        GameObject[] objs = GameObject.FindGameObjectsWithTag(gameObject.tag);


        if (objs.Length > 1)
        {
            if (uniqueIndex != 0)
            {
                Destroy(this.gameObject);
            }
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
