using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSaveData
{
    public int highScoreValue;
    CharacterUnlockedSituation characterUnlocked;
    public bool PassedThroughTutorial;
    public float musicVolume = 1;
    public float sfxVolume = 1;
}

[System.Serializable]
public class CharacterUnlockedSituation
{
    public bool normalCharacter;
   

    
}