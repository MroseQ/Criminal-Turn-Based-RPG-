using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OutlineSelection : MonoBehaviour
{
    public Transform highlight;
    public Transform selection;
    void Update()
    {
        // Highlight
        if (highlight != null)
        {
            highlight.gameObject.GetComponent<Outline>().enabled = false;
            highlight = null;
        }
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D raycastHit = Physics2D.Raycast(new Vector2(mousePosition.x, mousePosition.y), Vector2.zero);
        if (!EventSystem.current.IsPointerOverGameObject() && raycastHit && raycastHit.transform.tag == "Character")
        {
            {

                highlight = raycastHit.transform;
                if (highlight != selection)
                {
                    if (highlight.gameObject.GetComponent<Outline>() != null)
                    {
                        highlight.gameObject.GetComponent<Outline>().enabled = true;
                    }
                    else
                    {
                        Outline outline = highlight.gameObject.AddComponent<Outline>();
                        outline.enabled = true;
                        highlight.gameObject.GetComponent<Outline>().OutlineColor = new Color(1f, 0.55f,0f);
                        highlight.gameObject.GetComponent<Outline>().OutlineWidth = 7.0f;
                    }
                }
                else
                {
                    highlight = null;
                }
            }

            // Selection
            if (Input.GetMouseButtonDown(0))
            {
                if (highlight)
                {
                    if (selection != null)
                    {
                        selection.gameObject.GetComponent<Outline>().enabled = false;
                    }
                    selection = raycastHit.transform;
                    selection.gameObject.GetComponent<Outline>().enabled = true;
                    highlight = null;
                }
                else if (selection)
                {
                    selection.gameObject.GetComponent<Outline>().enabled = false;
                    selection = null;

                }
            }
        }
    }
}