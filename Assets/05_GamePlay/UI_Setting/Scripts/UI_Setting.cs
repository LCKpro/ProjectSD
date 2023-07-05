using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Setting : MonoBehaviour
{
    public Slider masterVolumeSlider;
    public Slider bgmVolumeSlider;
    public Slider sfxVolumeSlider;

    private void Start()
    {
        // 시작 시 저장된 설정 값 불러오기
        /*float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        masterVolumeSlider.value = savedVolume;

        // 오디오 관리자에 볼륨 설정 적용
        Core.Instance.audioManager.SetMasterVolume(savedVolume);*/
    }

    private void OnEnable()
    {
        Time.timeScale = 0;
    }

    public void OnMasterVolumeChanged()
    {
        float volume = masterVolumeSlider.value;

        // 오디오 관리자에 볼륨 설정 적용
        Core.Instance.audioManager.SetMasterVolume(volume);

        // 설정 값 저장
        PlayerPrefs.SetFloat("MasterVolume", volume);
        PlayerPrefs.Save();
    }

    public void OnBGMVolumeChanged()
    {
        float volume = bgmVolumeSlider.value;

        // 오디오 관리자에 볼륨 설정 적용
        Core.Instance.audioManager.SetBGMVolume(volume);

        // 설정 값 저장
        PlayerPrefs.SetFloat("BGMVolume", volume);
        PlayerPrefs.Save();
    }

    public void OnSFXVolumeChanged()
    {
        float volume = sfxVolumeSlider.value;

        // 오디오 관리자에 볼륨 설정 적용
        Core.Instance.audioManager.SetSFXVolume(volume);

        // 설정 값 저장
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }

    public void OnClick_Exit()
    {
        SoundManager.instance.PlaySound("NormalClick");
        Time.timeScale = 1;
        Core.Instance.uiPopUpManager.Hide("UI_Setting");
    }
}
