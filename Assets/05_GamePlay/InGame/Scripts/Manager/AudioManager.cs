using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource; // 소리 재생을 위한 AudioSource 컴포넌트를 가집니다.

    private float masterVolume = 1.0f; // 마스터 볼륨 설정 변수
    private float bgmVolume = 1.0f; // 배경음 볼륨 설정 변수
    private float sfxVolume = 1.0f; // 효과음 볼륨 설정 변수

    private void Start()
    {
        // AudioSource 컴포넌트를 추가하고 참조를 가져옵니다.
        audioSource = GetComponent<AudioSource>();
    }

    // 소리 클립을 재생하는 메서드
    public void PlaySound(AudioClip clip, float volume)
    {
        audioSource.PlayOneShot(clip, volume * masterVolume);
    }

    // 마스터 볼륨 설정 메서드
    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
    }
}
