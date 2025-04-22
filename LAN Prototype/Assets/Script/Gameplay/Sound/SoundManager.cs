using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public enum SoundType{GameStart, GameEnd, HitVR, HitM, ScoreVR, ScoreM }

public class SoundManager : MonoBehaviour
{
    [Header("Sound Effects")]
    [SerializeField] private AudioClip clip_GameStart;
    [SerializeField] private AudioClip clip_GameEnd;
    [SerializeField] private AudioClip clip_HitVR;
    [SerializeField] private AudioClip clip_HitM;
    [SerializeField] private AudioClip clip_ScoreVR;
    [SerializeField] private AudioClip clip_ScoreM;

    private AudioClip clipToPlay;

    private AudioSource m_AudioSource;

    public static SoundManager instance;

    void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();
        instance = this;
    }

    #region Accessible Functions

    //Call a sound of a specific type and play that specified sound globally
    public void PlaySound(SoundType type)
    {
        PlaySoundServerRpc(type);
    }

    //Call a sound of a specific type and play that sound at a specified location
    public void PlaySound(SoundType type, Vector3 location)
    {
        PlaySoundServerRpc(type, location);
    }

    #endregion

    #region RPC functions

    [ServerRpc]
    private void PlaySoundServerRpc(SoundType type)
    {
        PlaySoundClientRpc(type);
    }

    [ServerRpc]
    private void PlaySoundServerRpc(SoundType type, Vector3 location)
    {
        PlaySoundClientRpc(type, location);
    } 
    
    [ClientRpc]
    private void PlaySoundClientRpc(SoundType type)
    {
        switch (type)
        {
            case SoundType.GameStart:
                clipToPlay = clip_GameStart;
                break;

            case SoundType.GameEnd:
                clipToPlay = clip_GameEnd;
                break;
            case SoundType.HitVR:
                clipToPlay = clip_HitVR;
                break;
            case SoundType.HitM:
                clipToPlay = clip_HitM;

                break;
            case SoundType.ScoreVR
        :
                clipToPlay = clip_ScoreVR;
                break;
            case SoundType.ScoreM:
                clipToPlay = clip_ScoreM;
                break;
        }
        m_AudioSource.clip = clipToPlay;
        m_AudioSource.Play();
    }

    [ClientRpc]
    private void PlaySoundClientRpc(SoundType type, Vector3 location)
    {
        switch (type)
        {
            case SoundType.GameStart:
                clipToPlay = clip_GameStart;
                break;

            case SoundType.GameEnd:
                clipToPlay = clip_GameEnd;
                break;
            case SoundType.HitVR:
                clipToPlay = clip_HitVR;
                break;
            case SoundType.HitM:
                clipToPlay = clip_HitM;
                    
                break;
            case SoundType.ScoreVR
                : clipToPlay = clip_ScoreVR;
                break;
            case SoundType.ScoreM:
                clipToPlay = clip_ScoreM;
                break;
        }

        AudioSource.PlayClipAtPoint(clipToPlay, location);
    } 
    




    #endregion
}
