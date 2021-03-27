using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*===========================DISCLAIMER=====================
 * I DID NOT WRITE THIS. THIS IS FROM CODEMONKEYS VIDEO.
 * ========================================================
 * 
 * 
 * 
 */
public class SoundAssets : MonoBehaviour
{

    private static SoundAssets _i;

    public static SoundAssets i
    {
        get
        {
            if (_i == null) _i = Instantiate(Resources.Load<SoundAssets>("SoundAssets"));
            return _i;
        }
    }

    public SoundAudioClip[] soundAudioClipArray;
    public SoundAudioClip[] soundAudioMusicArray;

    [System.Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }


}
