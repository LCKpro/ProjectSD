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
        // ���� �� ����� ���� �� �ҷ�����
        /*float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        masterVolumeSlider.value = savedVolume;

        // ����� �����ڿ� ���� ���� ����
        Core.Instance.audioManager.SetMasterVolume(savedVolume);*/
    }

    private void OnEnable()
    {
        Time.timeScale = 0;
    }

    public void OnMasterVolumeChanged()
    {
        float volume = masterVolumeSlider.value;

        // ����� �����ڿ� ���� ���� ����
        Core.Instance.audioManager.SetMasterVolume(volume);

        // ���� �� ����
        PlayerPrefs.SetFloat("MasterVolume", volume);
        PlayerPrefs.Save();
    }

    public void OnBGMVolumeChanged()
    {
        float volume = bgmVolumeSlider.value;

        // ����� �����ڿ� ���� ���� ����
        Core.Instance.audioManager.SetBGMVolume(volume);

        // ���� �� ����
        PlayerPrefs.SetFloat("BGMVolume", volume);
        PlayerPrefs.Save();
    }

    public void OnSFXVolumeChanged()
    {
        float volume = sfxVolumeSlider.value;

        // ����� �����ڿ� ���� ���� ����
        Core.Instance.audioManager.SetSFXVolume(volume);

        // ���� �� ����
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
