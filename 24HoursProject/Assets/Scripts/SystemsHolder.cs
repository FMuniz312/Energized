using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class it's a manager for the DONTDESTROY prefab, which holds all the permanent prefabs
/// </summary>
public class SystemsHolder : MonoBehaviour
{

   
    void Awake()
    {
        
        Application.wantsToQuit += Application_wantsToQuit;
    }

     
   
    private bool Application_wantsToQuit()
    {
        DataSerialization.Instance.Save();
        return true;
    }

     

}
