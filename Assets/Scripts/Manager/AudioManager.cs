using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClipButtonClick;
    public AudioSource gachaSource;
    public AudioClip audiogacha;
    public AudioSource attackSoure;
    public AudioClip audioAttack;
    public AudioSource bgmSoure;
    public AudioClip audioBgm;
    public AudioSource matchSoure;
    public AudioClip audioMatch;
    public AudioSource cardSoure;
    public AudioClip audioCard;

    public static AudioManager instance;

    void Awake()
    {
        if (AudioManager.instance == null)
        {
            AudioManager.instance = this;
        }
    }
    public void PlayClickSound()
    {
        audioSource.PlayOneShot(audioClipButtonClick);
    }

    public void PlayGacha()
    {
        gachaSource.PlayOneShot(audiogacha);
    }

    public void PlayAttack()
    {
        attackSoure.PlayOneShot(audioAttack);
    }

    public void PlayBgm()
    {
        bgmSoure.PlayOneShot(audioBgm);
    }

    public void PlayMatch()
    {
        matchSoure.PlayOneShot(audioMatch);
    }

    public void PlayCardSound()
    {
        cardSoure.PlayOneShot(audioCard);
    }
}
