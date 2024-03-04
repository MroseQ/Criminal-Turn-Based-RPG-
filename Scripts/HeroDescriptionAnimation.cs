using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroDescriptionAnimation : MonoBehaviour
{
    public void PerformAnimation(Toggle toggle)
    {
        if (toggle.isOn)
        {
            GetComponent<Animator>().SetBool("Show", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("Show", false);
        }
    }
}
