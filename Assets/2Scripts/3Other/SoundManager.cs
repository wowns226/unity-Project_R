using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private void Awake()
    {
        if ( Instance == null )
            Instance = this;
    }

    [SerializeField]
    private List<AudioSource> audioSources;
   
    public AudioMixer masterMixer;

    public IReadOnlyList<AudioSource> AudioSources => audioSources;

    public void PlaySound( string music )
    {
        AudioSource source = GetSound(music);

        source.Play();
    }

    public void StopSound( string music )
    {
        AudioSource source = GetSound(music);

        source.Stop();
    }

    public AudioSource GetSound( string sfx )
    {
        foreach( AudioSource source in audioSources )
        {
            if ( source.clip.name == sfx )
            {
                return source;
            }
                
        }

        return null;
    }
}
