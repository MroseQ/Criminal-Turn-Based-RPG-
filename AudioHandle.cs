using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandle : MonoBehaviour
{
    public List<AudioClip> clipList = new();
    private IEnumerator DeleteAfterFinish(AudioClip clip)
    {
        yield return new WaitForSeconds(clip.length);
        clipList.Remove(clip);
    }

    public void SetNewClip(AudioClip clip, float scale = 1f)
    {
        if(!clipList.Exists(c => c == clip))
        {
            clipList.Add(clip);
            GetComponent<AudioSource>().PlayOneShot(clip, scale);
            StartCoroutine(DeleteAfterFinish(clip));
        }
    }
}
