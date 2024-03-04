using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileFade : MonoBehaviour
{
    private float alphaSpeed;
    public CharactersParameters target;
    public bool hit, miss;
    public Vector3 startPosition;
    void Start()
    {
        alphaSpeed = 5;
        StartCoroutine(Fade('+'));
    }

    private void Update()
    {
        if (target != null && target.IsDead)
        {
            hit = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ProjectileHit") && other == target.charCollider)
        {
            hit = true;
            if (!miss)
            {
                gameObject.TryGetComponent(out Animator component);
                if (component != null)
                {
                    component.SetTrigger("OutLoop");
                }
                if (gameObject.name.Split("(")[0] != "FireDragon")
                {
                    StartCoroutine(Fade());
                }
            }
        }
    }

    private IEnumerator Fade(char sign = '-')
    {
        var color = GetComponentInChildren<Image>().color;
        float alpha = color.a;
        do
        {
            if (sign == '+')
            {
                alpha+=Time.deltaTime * alphaSpeed;
                if (alpha > 1f)
                {
                    alpha = 1f;
                }
            }
            else
            {
                alpha -=Time.deltaTime * alphaSpeed*5;
                if (alpha < 0f)
                {
                    alpha = 0f;
                }
            }
            GetComponentInChildren<Image>().color = new(color.r, color.g, color.b, alpha);
            yield return new WaitForFixedUpdate();
        } while (alpha > 0 && alpha < 1);
    }
}
