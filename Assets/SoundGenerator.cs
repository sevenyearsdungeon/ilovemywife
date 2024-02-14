using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundGenerator : MonoBehaviour
{
    public List<AudioClip> clips;
    public AudioSource audioSourcePrefab;

    public void PlayRandomFart()
    {
        AudioSource source = Instantiate(audioSourcePrefab);
        source.clip = clips[Random.Range(0, clips.Count - 1)];
        source.Play();
        Destroy(source.gameObject, source.clip.length);
    }


}