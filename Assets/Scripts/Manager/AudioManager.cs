using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class AudioManager : BaseSingleton<AudioManager>
{
   [SerializeField] AudioPool MainAudioPool;
   [SerializeField] AudioPool UIAudio;
   
   [Header("LoopedAudio")]
   [SerializeField] AudioMixer MainAudioMixer;
   [SerializeField] AudioMixer UIAudioMixer;

   bool IsFadingAudio = false;
   float DefaultAudioVolume = 0.0f;
   private void Start()
   {
      MainAudioMixer.GetFloat("VolumeMaster", out DefaultAudioVolume);
   }
   
   public void FadeMainAudio(float FadeTime, bool In)
   {
      if(IsFadingAudio) return;
      
      IsFadingAudio = true;
      StartCoroutine(FadeLoopAudio(FadeTime, In));
   }

   public void ResetMainAuido()
   {
      MainAudioMixer.SetFloat("VolumeMaster", DefaultAudioVolume);
   }
   IEnumerator FadeLoopAudio(float FadeTime, bool In)
   {
      float PassedTime = 0.0f;
      float TargetValue = In ? DefaultAudioVolume : ConvertToDecibel(0.0f);
      float StartValue = In ? ConvertToDecibel(0.0f) : DefaultAudioVolume;
      
      while (PassedTime < FadeTime)
      {
         PassedTime += Time.deltaTime;
         float InterpValue = PassedTime / FadeTime;
         float NewVolume = Mathf.Lerp(StartValue, TargetValue, InterpValue);
         
         MainAudioMixer.SetFloat("VolumeMaster", NewVolume);
         yield return null;
      }

      IsFadingAudio = false;
   }
   public void PlayClipAtPosition(Vector2 PositionToPlayClipAt, AudioClip ClipToPlay, ESoundGroup SoundPoolToUse, float Volume, bool UseRandomPitch = false, float MinPitch = 0.0f, float MaxPitch = 0.0f)
   {
      AudioSource SourceToPlay = null;
      switch (SoundPoolToUse)
      {
         case ESoundGroup.Main:
         {
            SourceToPlay = MainAudioPool.GetObjectFromPool(PositionToPlayClipAt);
            break;
         }
         case ESoundGroup.UI:
         {
            SourceToPlay = UIAudio.GetObjectFromPool(PositionToPlayClipAt);
            break;
         }
         default:
            throw new ArgumentOutOfRangeException(nameof(SoundPoolToUse), SoundPoolToUse, null);
      }

      if (UseRandomPitch)
      {
         SourceToPlay.pitch = Random.Range(MinPitch, MaxPitch);
      }
      
      SourceToPlay.PlayOneShot(ClipToPlay, Volume);

      if (!SourceToPlay)
      {
         Debug.Log("No source given");
         return;
      }
      StartCoroutine(DespawnAudioSourceAfterTime(SourceToPlay, ClipToPlay.length));
   }

   //expects a value between zero and one
   float ConvertToDecibel(float InValue)
   {
      return Mathf.Lerp(-80, 20, InValue);
   }

   IEnumerator DespawnAudioSourceAfterTime(AudioSource SourceToDespawn, float ClipLength)
   {
      yield return new WaitForSeconds(ClipLength);
      
      SourceToDespawn.gameObject.SetActive(false);
   }
}
