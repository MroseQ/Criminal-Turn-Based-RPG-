using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class WaitForChangeOfScenes : MonoBehaviour
{
    public GameObject skybox, floor, camera, video, spriteLoader, videoPlayer;
    public SpawnScriptableObject SO;
    private void Awake()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
        SO.chars = new string[4];
        SO.objectsMoved = false;
        StartCoroutine(WaitForChange());
        video.SetActive(true);
        videoPlayer.SetActive(true);
        videoPlayer.GetComponent<VideoPlayer>().Play();
    }

    private void Start()
    {
        videoPlayer.SetActive(false);
        video.SetActive(false);
    }
    private IEnumerator WaitForChange()
    {
        yield return new WaitUntil(() => SO.objectsMoved);
        StartCoroutine(spriteLoader.GetComponent<SpriteLoader>().Load());
        video.SetActive(true);
        camera.SetActive(true);
        camera.GetComponent<OutlineHandler>().enabled = true;
        skybox.SetActive(true);
        floor.SetActive(true);
        SO.objectsMoved = false;
    }
}
