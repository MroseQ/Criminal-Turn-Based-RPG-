using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOutline : MonoBehaviour
{
    [SerializeField] private GameObject outline;
    [SerializeField] private OutlineHandler mainCameraOutline;
    private bool mouseOver;

    private void OnMouseEnter()
    {
        outline.SetActive(true);
    }
    private void OnMouseExit()
    {
        mouseOver = false;
        if(outline.GetComponent<SpriteRenderer>().material.name.Split(" ")[0] != "OutlineWhite")
        {
            outline.SetActive(false);
        }
    }

    private void LateUpdate()
    {
            if (mainCameraOutline.selected != outline && outline.GetComponent<SpriteRenderer>().material.name.Split(" ")[0] == "OutlineWhite")
            {
                if (Resources.Load<CharactersParameters>("Characters/" + outline.transform.parent.name).enemy)
                {
                    outline.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/OutlineRed");
                }
                else
                {
                    outline.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/OutlineGreen");
                }
            }
            if (!mouseOver && Input.GetMouseButtonDown(0) && mainCameraOutline.selected == outline && mainCameraOutline.selected != null)
            {
                outline.SetActive(false);
                mainCameraOutline.selected = null;
            }
    }
    private void OnMouseOver()
    {
        mouseOver = true;
        if (Input.GetMouseButtonDown(0) && mainCameraOutline.gameObject.GetComponent<CalculateTurns>().pickInfo.gameObject.activeSelf)
        {
            if(mainCameraOutline.selected != outline)
            {
                outline.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/OutlineWhite");
                mainCameraOutline.ChangeSelection(outline);
            }
            else
            {
                StartCoroutine(mainCameraOutline.ConfirmedSelection(outline));
                outline.SetActive(false);
            }
        }
    }
}
