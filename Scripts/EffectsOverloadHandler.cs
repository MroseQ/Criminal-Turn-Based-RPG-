using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsOverloadHandler : MonoBehaviour
{
    public IEnumerator WaitForSpace(GameObject obj, CharactersParameters target)
    {
        obj.SetActive(false);
        yield return new WaitUntil(() => target.effects.FindIndex(effect => effect.effectObject == obj) < 12);
        int posIndex = target.effects.FindIndex(effect => effect.effectObject == obj);
        Vector2 position = obj.GetComponent<RectTransform>().anchoredPosition;
        if (target.enemy)
        {
            position.x = 200 - (35 * posIndex);
        }
        else
        {
            position.x = -200 + (35 * posIndex);
        }
        obj.GetComponent<RectTransform>().anchoredPosition = position;
        obj.GetComponent<EffectsMoveOnBar>().posIndex = posIndex;
        obj.GetComponent<EffectsMoveOnBar>().prevIndex = posIndex;
        obj.SetActive(true);
        obj.transform.Find("Description").Find("EffectDuration").Find("Number").GetComponent<TMPro.TextMeshProUGUI>().text = target.effects.Find(effect => effect.effectObject == obj).effectDuration.ToString();
    }

    public void Exchange(GameObject obj, CharactersParameters target)
    {
        StartCoroutine(WaitForSpace(obj, target));
    }
}
