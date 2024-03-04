using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLoader : MonoBehaviour
{
    [SerializeField] private Texture2D[] textures;
    [SerializeField] private List<Sprite> sprites;

    public void Start()
    {
        sprites.AddRange(Resources.LoadAll<Sprite>("Sprites"));
    }

    public IEnumerator Load()
    {
        foreach(Sprite s in sprites)
        {
            print("a");
            GetComponent<SpriteRenderer>().sprite = s;
            yield return new WaitForFixedUpdate();
        }
    }
}
