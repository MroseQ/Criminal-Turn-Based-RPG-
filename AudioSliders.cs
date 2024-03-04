using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSliders : MonoBehaviour
{
    [SerializeField] private StartGameAudio musicSource;
    [SerializeField] private AudioSource magicSource, characterSource;
    [SerializeField] private Slider musicSlider, magicSlider, characterSlider;
    void Start()
    {
        musicSlider.value = musicSource.musicMaxVolume;
        magicSlider.value = magicSource.volume;
        characterSlider.value = characterSource.volume;
    }

    
    void Update()
    {
        musicSource.musicMaxVolume = musicSlider.value;
        magicSource.volume = magicSlider.value;
        characterSource.volume = characterSlider.value;
    }
}
