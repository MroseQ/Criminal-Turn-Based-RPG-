using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwordData : MonoBehaviour
{
    public CharactersParameters target;
    public bool hit;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("ProjectileHit") && other == target.charCollider)
        {
            hit = true;
        }
    }
}
