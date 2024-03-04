using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsMoveOnBar : MonoBehaviour
{
    public int posIndex, prevIndex;
    [SerializeField]private float speed;
    public CharactersParameters target;
    void Start()
    {
        posIndex = target.effects.FindIndex(effect => effect.effectObject == gameObject);
        prevIndex = posIndex;
        if (target.effects.Count <= 12)
        {
            Vector2 position = GetComponent<RectTransform>().anchoredPosition;
            if (target.enemy)
            {
                position.x = 200 - (35 * posIndex);
            }
            else
            {
                position.x = -200 + (35 * posIndex);
            }
            GetComponent<RectTransform>().anchoredPosition = position;
        }
        else
        {
            transform.parent.GetComponent<EffectsOverloadHandler>().Exchange(gameObject, target);
        }
    }

    void LateUpdate()
    {
        try
        {
            if (target.effects[posIndex].effectObject != gameObject)
            {
                posIndex = target.effects.FindIndex(effect => effect.effectObject == gameObject);
                if(posIndex != -1)
                {
                    StartCoroutine(Move());
                }
            }
        }
        catch
        {
            posIndex = target.effects.FindIndex(effect => effect.effectObject == gameObject);
            if (posIndex != -1)
            {
                StartCoroutine(Move());
            }
        }
    }

    private IEnumerator Move()
    {
        Vector2 newPosition = GetComponent<RectTransform>().anchoredPosition;
        if (target.enemy)
        {
            newPosition.x = 200 - (35 * posIndex);
        }
        else
        {
            newPosition.x = -200 + (35 * posIndex);
        }
        while (GetComponent<RectTransform>().anchoredPosition.x != newPosition.x)
        {
            GetComponent<RectTransform>().anchoredPosition = Vector3.MoveTowards(GetComponent<RectTransform>().anchoredPosition, newPosition, (prevIndex-posIndex)*speed*Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
        prevIndex = posIndex;
        GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(GetComponent<RectTransform>().anchoredPosition, newPosition, 1);
    }
}
