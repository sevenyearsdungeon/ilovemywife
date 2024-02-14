using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Toggle = UnityEngine.UI.Toggle;

public class SoundGenerator : MonoBehaviour
{
    public List<AudioClip> clips;
    public List<AudioClip> otherClips;
    public AudioSource audioSourcePrefab;
    public Toggle soundToggle;
    public void PlayRandomFart()
    {
        AudioSource source = Instantiate(audioSourcePrefab);
        if (soundToggle.isOn)
        source.clip = otherClips[Random.Range(0, otherClips.Count - 1)];
        else
        {
            source.clip = clips[Random.Range(0, clips.Count - 1)];
            
        }
        source.Play();
        Destroy(source.gameObject, source.clip.length);
    }


}