/using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class OptionsPanelBehaviour : MonoBehaviour
{

    [SerializeField] GameObject optionsGO;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    static float musicVolume = 1;
    static float sfxVolume = 1;
    [SerializeField] AudioSource musicAudioSource;
    Tweener idTweener;
    private void Start()
    {
        musicVolume = DataSerialization.Instance.SaveDataContainer.PlayerSaveData.musicVolume;
        sfxVolume = DataSerialization.Instance.SaveDataContainer.PlayerSaveData.sfxVolume;

        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;
        optionsGO.SetActive(false);
        SoundSystem.instance.ChangeVolumeMultiplier(sfxSlider.value);
        sfxSlider.onValueChanged.AddListener((volume) =>
        {
            SoundSystem.instance.ChangeVolumeMultiplier(volume);
            sfxVolume = volume;
        });
        musicAudioSource.volume *= musicSlider.value;
        musicSlider.onValueChanged.AddListener((volume) =>
        {
            musicAudioSource.volume = volume;
            musicVolume = volume;
        });
        Application.quitting += Application_quitting;
    }

    private void Application_quitting()
    {
        DataSerialization.Instance.SaveDataContainer.PlayerSaveData.musicVolume = musicVolume;
        DataSerialization.Instance.SaveDataContainer.PlayerSaveData.sfxVolume = sfxVolume;
    }

    public void OpenOptionsMenu()
    {
        optionsGO.SetActive(true);
        if (!idTweener.IsActive() || idTweener==null) idTweener = optionsGO.GetComponent<RectTransform>().DOPunchScale(Vector3.one*0.5f,.5f,10,.2f);
    }

    public void CloseMenu()
    {
        TweenCallback tweenCallback = () =>
        {
            optionsGO.GetComponent<RectTransform>().localScale = Vector3.one * .9f;
            optionsGO.SetActive(false);
            
        };
           

        optionsGO.GetComponent<RectTransform>().DOScale(0f, .5f).OnComplete(tweenCallback);
        

    }

    private void OnDestroy()
    {
        DataSerialization.Instance.SaveDataContainer.PlayerSaveData.musicVolume = musicVolume;
        DataSerialization.Instance.SaveDataContainer.PlayerSaveData.sfxVolume = sfxVolume;
    }

}
