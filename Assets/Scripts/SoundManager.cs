using MyUtils;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField] List<StringAudioPair> _audioPairs;
    private Dictionary<string, AudioClip> _audioClips = new();
    private void Start()
    {
        foreach (var pair in _audioPairs)
        {
            if (!_audioClips.ContainsKey(pair.AudioKey))
            {
                _audioClips.Add(pair.AudioKey, pair.AudioClip);
            }
            else
            {
                Debug.LogWarning($"Duplicate audio key found: {pair.AudioKey}. Skipping.");
            }
        }
    }
    public void PlaySFX(string sfxName)
    {
        _audioSource.PlayOneShot(_audioClips[sfxName]);
    }
    private bool _shouldPlayZombieHurtSound = true;
    public void PlayZombieHurtSound()
    {
        if (!_shouldPlayZombieHurtSound) return;
        _audioSource.PlayOneShot(_audioClips[ZOMBIE_HURT]);
        StartCoroutine(CooldownCoroutine());
        IEnumerator CooldownCoroutine()
        {
            _shouldPlayZombieHurtSound = false;
            yield return new WaitForSeconds(0.75f);
            _shouldPlayZombieHurtSound = true;
        }
    }
    public const string EXPLOSION = "Explosion";
    public const string ZOMBIE_BITE = "ZombieBite";
    public const string ZOMBIE_DEATH = "ZombieDeath";
    public const string ZOMBIE_HURT = "ZombieHurt";
}
[System.Serializable]
public class StringAudioPair
{
    public string AudioKey;
    public AudioClip AudioClip;
}
