using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    static SoundManager _uniqueInstance;

    float _bgmVolume = 1;
    float _sfxVolume = 1;
    float _voiceVolume = 1;
    bool _bgmMute;
    bool _sfxMute;
    bool _voiceMute;

    AudioSource _bgmPlayer;
    AudioSource _sfxPlayer;
    AudioSource _voicePlayer;

    public static SoundManager _instance
    {
        get { return _uniqueInstance; }
    }

    void Awake()
    {
        _uniqueInstance = this;
        DontDestroyOnLoad(gameObject);

        _bgmPlayer = GetComponent<AudioSource>();
        _sfxPlayer = GetComponent<AudioSource>();
        _voicePlayer = GetComponent<AudioSource>();
    }

    public void PlayBGMSound(DefineUtillHelper.eBGMClipKind k, bool isLoop = true)
    {
        _bgmPlayer.clip = GameResourcePoolManager._instance.GetBGMClipFrom(k);
        _bgmPlayer.volume = _bgmVolume;
        _bgmPlayer.mute = _bgmMute;
        _bgmPlayer.loop = isLoop;

        _bgmPlayer.Play();
    }

    public AudioSource PlaySFXSound(DefineUtillHelper.eSFXClipKind k, bool isLoop = false)
    {
        if (isLoop)
        {
            GameObject go = new GameObject("sfxPlayer");
            go.transform.parent = transform;
            AudioSource sfxPlayer = go.AddComponent<AudioSource>();
            sfxPlayer.clip = GameResourcePoolManager._instance.GetSFXClipFrom(k);
            sfxPlayer.volume = _sfxVolume;
            sfxPlayer.mute = _sfxMute;
            sfxPlayer.loop = isLoop;
            return sfxPlayer;
        }
        else
        {
            _sfxPlayer.volume = _sfxVolume;
            _sfxPlayer.mute = _sfxMute;
            _sfxPlayer.loop = isLoop;
            _sfxPlayer.PlayOneShot(GameResourcePoolManager._instance.GetSFXClipFrom(k));
            return null;
        }
    }

    public AudioSource PlayVOICESound(DefineUtillHelper.eVOICEClipKind k, bool isLoop = false)
    {
        if (isLoop)
        {
            GameObject go = new GameObject("voicePlayer");
            go.transform.parent = transform;
            AudioSource voicePlayer = go.AddComponent<AudioSource>();
            voicePlayer.clip = GameResourcePoolManager._instance.GetVOICEClipFrom(k);
            voicePlayer.volume = _sfxVolume;
            voicePlayer.mute = _sfxMute;
            voicePlayer.loop = isLoop;
            return voicePlayer;
        }
        else
        {
            _voicePlayer.volume = _voiceVolume;
            _voicePlayer.mute = _voiceMute;
            _voicePlayer.loop = isLoop;
            _voicePlayer.PlayOneShot(GameResourcePoolManager._instance.GetVOICEClipFrom(k));
            return null;
        }
    }
}
