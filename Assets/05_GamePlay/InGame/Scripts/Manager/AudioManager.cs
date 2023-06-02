using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource; // �Ҹ� ����� ���� AudioSource ������Ʈ�� �����ϴ�.

    private float masterVolume = 1.0f; // ������ ���� ���� ����
    private float bgmVolume = 1.0f; // ����� ���� ���� ����
    private float sfxVolume = 1.0f; // ȿ���� ���� ���� ����

    private void Start()
    {
        // AudioSource ������Ʈ�� �߰��ϰ� ������ �����ɴϴ�.
        audioSource = GetComponent<AudioSource>();
    }

    // �Ҹ� Ŭ���� ����ϴ� �޼���
    public void PlaySound(AudioClip clip, float volume)
    {
        audioSource.PlayOneShot(clip, volume * masterVolume);
    }

    // ������ ���� ���� �޼���
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
