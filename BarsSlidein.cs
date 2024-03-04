using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarsSlidein : MonoBehaviour
{
    public GameObject leftSide, bossPanel, enemyUpPanel, enemyDownPanel, fadeout;
    [SerializeField] private float speed,minusStepSpeed;
    private Vector3 start,bossStart;
    [SerializeField] private bool restart, animationStarted;

    private void Start()
    {
        start = leftSide.GetComponent<RectTransform>().transform.localPosition;
        bossStart = bossPanel.GetComponent<RectTransform>().transform.localPosition;
    }

    void Update()
    {
        if (restart)
        {
            leftSide.GetComponent<RectTransform>().transform.localPosition = start;
            bossPanel.GetComponent<RectTransform>().transform.localPosition = bossStart;
            restart = false;
        }
        if (fadeout.GetComponent<Image>().color.a < 0.8f && !animationStarted)
        {
            animationStarted = true;
            StartCoroutine(MoveBars(leftSide.GetComponent<RectTransform>(),new Vector2(835,leftSide.GetComponent<RectTransform>().anchoredPosition.y)));
            StartCoroutine(MoveBars(bossPanel.GetComponent<RectTransform>(), new Vector2(3370, bossPanel.GetComponent<RectTransform>().anchoredPosition.y)));
        }
        //TOP - 4205
        //BOTTOM - 4455
    }

    private IEnumerator MoveBars(RectTransform rect,Vector2 finishingPosition)
    {
        var nowSpeed = speed;
        yield return new WaitForFixedUpdate();
        while (Math.Abs(rect.anchoredPosition.x - finishingPosition.x) > 1)
        {
            nowSpeed -= minusStepSpeed;
            if(nowSpeed <= 0)
            {
                nowSpeed = 50;
            }
            var step = nowSpeed * Time.fixedDeltaTime;
            rect.anchoredPosition = Vector2.MoveTowards(rect.GetComponent<RectTransform>().anchoredPosition, finishingPosition, step);
            yield return new WaitForFixedUpdate();
        }
        rect.anchoredPosition = Vector2.Lerp(rect.GetComponent<RectTransform>().anchoredPosition, finishingPosition, 1);
    }
}
