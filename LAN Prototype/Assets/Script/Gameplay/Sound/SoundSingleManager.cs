using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


//This script is designed to act as a component, add this to objects you want to emit noise along with an audio source component.
//Call from other scripts the file you want to play based on array number and it will play that sound across all clients.
//Make sure to initiate only once through checks such as isOwner or isServer/isClient to prevent multiple of the same sound occuring.
public class SoundSingleManager : NetworkBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] audioClips;

    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        if (source == null )
        {
            Debug.LogWarning("GameObject does not have an AudioSourceComponent");
        }
    }

    //Calls the Rpcs to ensure sound is synchronised across all clients
    public void PlayAudioClip(int clipNum)
    {

        if (IsServer) //if script was called from a server-side gameObject, the call to ServerRpc is unecessary
        {
            PlayAudioClipClientRpc(clipNum);
        }
        else
        {
            PlayAudioClipServerRpc(clipNum);
        }
    }


    //Only Servers can call a clientRpc. Allows clients to call sound cues.
    [ServerRpc]
    private void PlayAudioClipServerRpc(int clipNum)
    {
        PlayAudioClipClientRpc(clipNum);
    }

    //Plays audio across all clients
    [ClientRpc]
    private void PlayAudioClipClientRpc(int clipNum)
    {
        if(audioClips != null)
        {
            source.clip = audioClips[clipNum];
            if (source.clip != null)
            {
                source.Play();
            }
        }
        else
        {
            Debug.LogWarning("No AudioClips were assigned or the reference was missing, cannot play audio");
        }

    }


}
