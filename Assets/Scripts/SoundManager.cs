using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public enum Sound
    {
        playerMove,
        playerMove2,
        playerMove3,
        playerHurt,
        enemyHurt1,
        enemyHurt2,
        enemyHurt3,
        powerup_swipe,
        button,
    }

    private static GameObject oneShotGameObject;

    private static AudioSource oneShotAudioSource;

    private static Dictionary<Sound, float> soundTimerDictionary;
    

    public static void Initialize()
    {
        // Call this function in gameHandler Awake()
        soundTimerDictionary = new Dictionary<Sound, float>();
        soundTimerDictionary[Sound.playerMove] = 0f;
    }

    private static bool CanPlaySound(Sound sound)
    {
        switch(sound)
        {
            default:
                return true;

                case Sound.playerMove: 
                    if(soundTimerDictionary.ContainsKey(sound))
                    {
                        float lastTimePlayed = soundTimerDictionary[sound];
                        float playerMoveTimerMax = .05f;
                        if(lastTimePlayed + playerMoveTimerMax < Time.time)
                        {
                            soundTimerDictionary[sound] = Time.time;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return true;
                    }
               // break;
        }
    }

    public static void PlayerMoveSound(Vector3 pos)
    {
        int randNumber = UnityEngine.Random.Range(0, 2);
        switch (randNumber)
        {
            case 0:
                SoundManager.PlaySound(SoundManager.Sound.playerMove, pos);
                break;

            case 1:
                SoundManager.PlaySound(SoundManager.Sound.playerMove2, pos);
                break;

            case 2:
                SoundManager.PlaySound(SoundManager.Sound.playerMove3, pos);
                break;

            default:

                break;
        }
    }

    public static void PlaySound(Sound sound, Vector3 position)
    {

        if(CanPlaySound(sound))
        {
            GameObject soundGameObject = new GameObject("Sound");
            soundGameObject.transform.position = position;
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();

            audioSource.clip = GetAudioClip(sound);

             audioSource.maxDistance = 70f;
            audioSource.spatialBlend = 1f;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.dopplerLevel = 0f;


            audioSource.Play();
            Object.Destroy(soundGameObject, audioSource.clip.length);
        }
        

        
    }


    public static void PlaySound(Sound sound)
    {
        if(CanPlaySound(sound))
        {
            if(oneShotGameObject == null)
            {
                oneShotGameObject = new GameObject("Sound");
                oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
            }
            
            oneShotAudioSource.PlayOneShot(GetAudioClip(sound));
        }
        
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach (SoundAssets.SoundAudioClip soundAudioClip in SoundAssets.i.soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("Sound " + sound + " not found!");
        return null;

    }

}
