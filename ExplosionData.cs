using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExplosionData : MonoBehaviour
{
    public AudioHandle magicSource;
    public bool explode;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        AudioClip clip = GetComponent<AudioSource>().clip;
        float scale = GetComponent<AudioSource>().volume;
        magicSource.SetNewClip(clip, scale);
    }
}
