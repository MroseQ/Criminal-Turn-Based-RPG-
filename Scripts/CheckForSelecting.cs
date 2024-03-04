using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CheckForSelecting : MonoBehaviour
{
    private int selectedChars;
    public GameObject button,fadeout;
    public Material none, whiteout;
    public AudioSource finishingAudio;
    public AudioClip buttonAudio;
    private bool ending, sceneloading;
    public SpawnScriptableObject SO;

    private void Start()
    {
        StartCoroutine(FadeOut(true));
    }

    private IEnumerator FadeOut(bool type)
    {
        do
        {
            Color c = fadeout.GetComponent<RawImage>().color;
            if (type)
            {
                c.a -= Time.deltaTime;
            }
            else
            {
                c.a += Time.deltaTime;
            }
            fadeout.GetComponent<RawImage>().color = c;
            yield return new WaitForFixedUpdate();
        } while ((fadeout.GetComponent<RawImage>().color.a <= 1f && !type) || (fadeout.GetComponent<RawImage>().color.a >= 0f && type));
        if (type)
        {
            fadeout.SetActive(false);
        }
    }

    void Update()
    {
        if (selectedChars == 2)
        {
            button.SetActive(true);
        }
        else
        {
            button.SetActive(false);
        }
        if (ending) {
            Camera.main.GetComponent<AudioSource>().volume -= 0.25f * Time.deltaTime;
            if(fadeout.GetComponent<RawImage>().color.a > 1f && !sceneloading)
                {
                    sceneloading = true;
                    Scene mainGameScene = SceneManager.GetSceneAt(0);
                    SceneManager.MoveGameObjectToScene(finishingAudio.transform.gameObject,mainGameScene);
                    SceneManager.MoveGameObjectToScene(GameObject.Find("EventSystem"), mainGameScene);
                    SO.objectsMoved = true;
                    SceneManager.UnloadSceneAsync(1);
                }
        }
    }

    public void AddSelected()
    {
        selectedChars = 0;
        foreach(Toggle elem in gameObject.GetComponentsInChildren<Toggle>())
        {
            if (elem.isOn)
            {
                selectedChars++;
                foreach (Image image in elem.GetComponentsInChildren<Image>())
                {
                    if (image.name == "Image")
                    {
                        image.GetComponent<Image>().material = none;
                    }
                }
            }
            else
            {
                foreach (Image image in elem.GetComponentsInChildren<Image>())
                {
                    if (image.name == "Image")
                    {
                        image.GetComponent<Image>().material = whiteout;
                    }
                }
            }
        }
    }

    public void PlayAudio(GameObject elem)
    {
        if (elem.GetComponent<Toggle>().isOn)
        {
            elem.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Audio/UI/interface1");
            elem.GetComponent<AudioSource>().Play();
        }
        else
        {
            elem.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Audio/UI/interface2");
            elem.GetComponent<AudioSource>().Play();
        }
        
    }

    public void Submit()
    {
        int index = 0;
        foreach (Toggle elem in gameObject.GetComponentsInChildren<Toggle>())
        {
            if (elem.isOn)
            {
                foreach (Image image in elem.GetComponentsInChildren<Image>())
                {
                    if (image.name == "Image")
                    {
                        //send image
                        if (image.sprite.name == "Keiji Shinogi")
                        {
                            SO.chars[index] = image.sprite.name;
                            index++;
                            SO.chars[index] = "Evil Keiji";
                            index++;
                        }
                        else if(image.sprite.name == "Hatsune Miku")
                        {
                            SO.chars[index] = image.sprite.name;
                            index++;
                            SO.chars[index] = "Nagito Komaeda";
                            index++;
                        }
                        else if (image.sprite.name == "Asu")
                        {
                            SO.chars[index] = image.sprite.name;
                            index++;
                            SO.chars[index] = "Evil Asu";
                            index++;
                        }
                        else if (image.sprite.name == "Marek")
                        {
                            SO.chars[index] = image.sprite.name;
                            index++;
                            SO.chars[index] = "Evil Marek";
                            index++;
                        }
                    }
                }
                fadeout.SetActive(true);
                StartCoroutine(FadeOut(false));
                ending = true;
            }
        }
        finishingAudio.Play();
    }
}
