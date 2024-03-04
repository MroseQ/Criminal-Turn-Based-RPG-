using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class StartGameAudio : MonoBehaviour
{
    [SerializeField]private GameObject musicGameObject,video,videoPlayer, fragments, videoText,fader;
    private AudioSource[] musicAudioSources;
    [SerializeField]private AudioSource buttonAudioSource,previousSource;
    [SerializeField] private AudioHandle magicSource;
    public bool sourceSwitch;
    [SerializeField]private int musicIndex;
    [SerializeField] public float musicMaxVolume,musicStepSpeedVolume;
    private float previousMaxVolume;
    private void Awake()
    {
        fader.SetActive(true);
        buttonAudioSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();
        musicAudioSources = new AudioSource[3];
        musicIndex = 0;
        StartCoroutine(WaitForButtonSourceToEnd());
    }
    private void Update()
    {
        if(previousMaxVolume != musicMaxVolume && musicAudioSources[^1] != null)
        {
            for(int i = 0; i < musicIndex; i++)
            {
                StartCoroutine(VolumeChange(musicAudioSources[i], previousMaxVolume < musicMaxVolume));
            }
            previousMaxVolume = musicMaxVolume;
        }
        if (sourceSwitch && musicIndex < 3)
        {
            musicIndex++;
            for (int i = 0; i < musicIndex; i++)
            {
                StartCoroutine(VolumeChange(musicAudioSources[i], musicAudioSources[i].volume < musicMaxVolume));
            }
            sourceSwitch = false;
        }
    }

    private IEnumerator VolumeChange(AudioSource source, bool shouldTurnUp)
    {
        if (shouldTurnUp)
        {
            while (source.volume != musicMaxVolume/ musicIndex)
            {
                source.volume += Time.deltaTime * musicStepSpeedVolume;
                if(source.volume > musicMaxVolume / musicIndex)
                {
                    source.volume = musicMaxVolume / musicIndex;
                }
                yield return new WaitForFixedUpdate();
            }
        }
        else
        {
            while(source.volume != musicMaxVolume / musicIndex)
            {
                source.volume -= Time.deltaTime * musicStepSpeedVolume;
                if(source.volume < musicMaxVolume / musicIndex)
                {
                    source.volume = musicMaxVolume / musicIndex;
                }
                yield return new WaitForFixedUpdate();
            }
        }
    }

    private IEnumerator WaitForButtonSourceToEnd()
    {
        AudioClip clip = Resources.Load<AudioClip>("Audio/UI/button");
        yield return new WaitUntil(() => clip.samples * 0.85f <= buttonAudioSource.timeSamples);
        int index = 0;
        StartCoroutine(WaitForVideoToFinish());
        foreach (AudioSource source in musicGameObject.GetComponents<AudioSource>())
        {
            musicAudioSources.SetValue(source, index);
            source.Play();
            index++;
        }
        sourceSwitch = true;
        yield return new WaitWhile(() => buttonAudioSource.isPlaying);
        Destroy(buttonAudioSource);
    }

    private IEnumerator WaitForVideoToFinish()
    {
        videoPlayer.SetActive(true);
        float textAlpha = 1f;
        while(video.GetComponent<RawImage>().color.r <= 1f)
        {
            textAlpha -= Time.deltaTime / 2;
            Color c = video.GetComponent<RawImage>().color;
            c.r += Time.deltaTime;
            video.GetComponent<RawImage>().color = new(c.r, c.r, c.r, c.a);
            videoText.GetComponent<TextMeshProUGUI>().color = new(c.r,c.r,c.r,textAlpha);
            Color faderColor = fader.GetComponent<Image>().color;
            faderColor.a -= Time.deltaTime;
            fader.GetComponent<Image>().color = faderColor;
            yield return new WaitForFixedUpdate();
        }
        video.GetComponent<RawImage>().color = new(1f, 1f, 1f, 1f);
        StartCoroutine(SkipableCutscene());
        yield return new WaitUntil(() => videoPlayer.GetComponent<VideoPlayer>().time >= videoPlayer.GetComponent<VideoPlayer>().length - 1f);
        Destroy(videoText);
        yield return new WaitWhile(() => videoPlayer.GetComponent<VideoPlayer>().isPlaying);
        magicSource.SetNewClip(Resources.Load<AudioClip>("Audio/Magic/IceShatter"),1.3f);
        fragments.SetActive(true);
        video.SetActive(false);
        Destroy(videoText);
    }

    private IEnumerator SkipableCutscene()
    {
        while (videoPlayer.GetComponent<VideoPlayer>().isPlaying)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                videoPlayer.GetComponent<VideoPlayer>().time = videoPlayer.GetComponent<VideoPlayer>().length - 1f;
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
