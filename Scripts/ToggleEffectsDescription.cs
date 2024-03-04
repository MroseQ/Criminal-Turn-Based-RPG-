using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ToggleEffectsDescription : MonoBehaviour
{
    private GameObject descObject,descText,effectDuration,effectName;
    [SerializeField] private GameObject uiCanvas;
    private Vector3[] uiCorners = new Vector3[4], descCorners = new Vector3[4];
    private Rect uiRect = new Rect(), descRect = new Rect();
    private bool mouseTarget = false;
    void Start()
    {
        uiCanvas = GameObject.Find("UICanvas").gameObject;
        string frameName = " ";
        foreach(Transform obj in transform.parent)
        {
            if(obj.name == "Description")
            {
                descObject = obj.gameObject;
            }
            if(obj.name == "Frame")
            {
                frameName = obj.GetComponent<Image>().sprite.ToString().Split(" ")[0];
            }
        }
        effectName = descObject.transform.Find("EffectName").Find("Text").gameObject;
        descText = descObject.transform.Find("DescriptionText").Find("Text").gameObject;
        effectDuration = descObject.transform.Find("EffectDuration").Find("Number").gameObject;
        if (frameName == "efforange")
        {
            effectName.GetComponent<TextMeshProUGUI>().fontMaterial = Resources.Load<Material>("Materials/EffectOrangeMaterial");
        }
        else if(frameName == "effblue")
        {
            effectName.GetComponent<TextMeshProUGUI>().fontMaterial = Resources.Load<Material>("Materials/EffectBlueMaterial");
        }
        else if(frameName == "effblue2")
        {
            effectName.GetComponent<TextMeshProUGUI>().fontMaterial = Resources.Load<Material>("Materials/EffectBlue2Material");
        }
        else if(frameName == "effgreen")
        {
            effectName.GetComponent<TextMeshProUGUI>().fontMaterial = Resources.Load<Material>("Materials/EffectGreenMaterial");
        }
        else if (frameName == "effpurpleblue")
        {
            effectName.GetComponent<TextMeshProUGUI>().fontMaterial = Resources.Load<Material>("Materials/EffectPurpleBlueMaterial");
        }
        else if (frameName == "effpurple")
        {
            effectName.GetComponent<TextMeshProUGUI>().fontMaterial = Resources.Load<Material>("Materials/EffectPurpleMaterial");
        }
    }
    private void OnMouseExit()
    {
        descObject.SetActive(false);
    }

    private void OnMouseOver()
    {
        if(mouseTarget) descObject.SetActive(true);
    }

    private void Update()
    {
        if(transform.parent.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            mouseTarget = true;
        }
        else
        {
            mouseTarget = false;
        }

        if (IsDescriptionOutside(out Vector3 moveVector) && mouseTarget)
        {
            descObject.transform.Translate(moveVector);
        }
    }

    private bool IsDescriptionOutside(out Vector3 moveVector)
    {
        descObject.GetComponent<RectTransform>().GetWorldCorners(descCorners);
        uiCanvas.GetComponent<RectTransform>().GetWorldCorners(uiCorners);

        uiRect = CreateRect(uiCorners[0], uiCorners[2],0.05f);
        descRect = CreateRect(descCorners[0], descCorners[2]);

        DebugRects(descRect, uiRect); //

        float difference = 0;
        if(uiRect.xMin > descRect.xMin)
        {
            difference = uiRect.xMin - descRect.xMin;
        }
        else if(uiRect.xMax < descRect.xMax)
        {
            difference = uiRect.xMax - descRect.xMax;
        }
        if(difference != 0)
        {
            moveVector = new Vector3(difference, 0, 0);
            return true;
        }
        moveVector = Vector3.zero;
        return false;
    }

    private Rect CreateRect(Vector3 leftBottom, Vector3 rightTop, float margin = 0)
    {
        float x = Mathf.Min(leftBottom.x+margin, rightTop.x-margin);
        float y = Mathf.Min(leftBottom.y+margin, rightTop.y-margin);
        float width = Mathf.Abs(leftBottom.x+margin - rightTop.x+margin);
        float height = Mathf.Abs(leftBottom.y+margin - rightTop.y+margin);

        return new Rect(x, y, width, height);
    }

    private void DebugRects()
    {
        for (int i = 0; i < descCorners.Length; i++)
        {
            int j = i + 1 < descCorners.Length ? i + 1 : 0;
            Debug.DrawLine(descCorners[i], descCorners[j], Color.red);
            Debug.DrawLine(uiCorners[i], uiCorners[j], Color.blue);
        }
    }

    private void DebugRects(Rect desc, Rect ui)
    {
        Vector2[] descArray = GetCornersOfRect(desc);
        Vector2[] uiArray = GetCornersOfRect(ui);
        Debug.DrawLine(descArray[0], descArray[1], Color.green);
        Debug.DrawLine(uiArray[1], uiArray[1], Color.blue);
        for (int i = 1; i < descArray.Length; i++)
        {
            int j = i + 1 < descArray.Length ? i + 1 : 0;
            Debug.DrawLine(descArray[i], descArray[j], Color.red);
            Debug.DrawLine(uiArray[i], uiArray[j], Color.blue);
        }
    }

    private Vector2[] GetCornersOfRect(Rect rect)
    {
        Vector2[] array = new Vector2[4];
        array[0] = new Vector2(rect.xMin, rect.yMin);
        array[1] = new Vector2(rect.xMin, rect.yMax);
        array[2] = new Vector2(rect.xMax, rect.yMax);
        array[3] = new Vector2(rect.xMax, rect.yMin);
        return array;
    }
}
