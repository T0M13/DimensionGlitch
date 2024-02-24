using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotAudioPlayer : MonoBehaviour
{
    [SerializeField, Range(-3f, 3f)] float MinPitchValue = -1;
    [SerializeField, Range(-3f, 3f)] float MaxPitch = -1;
    [SerializeField, Range(0,1)] float Volume;
    [SerializeField] private bool UseRandomPitch = false;
    [SerializeField] AudioClip ClipToPlay;
    [SerializeField] ESoundGroup SoundGroup;
    
    public void PlayOneShotAudioClip()
    {
        AudioManager.Instance.PlayClipAtPosition(transform.position, ClipToPlay, SoundGroup, Volume, UseRandomPitch, MinPitchValue, MaxPitch);
    }
}
