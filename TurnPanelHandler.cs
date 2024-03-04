using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static CalculateTurns;

public class TurnPanelHandler : MonoBehaviour
{
    [SerializeField] private List<float> top;
    [SerializeField] private List<GameObject> objects = new List<GameObject>();
    [SerializeField] private string[] evil;
    public bool moving,toRemoveMoving;
    private List<bool> taskFinishing = new();
    [SerializeField] private CalculateTurns cameraTurns;
    private List<Task> taskList = new();
    private void OnEnable()
    {
        MessageToFill += Fill;
        ChangeTurn += DeletePassedTurn;
        Deletion += DeleteTurnOf;
    }
    void Fill(TurnID message)
    {
        message.obj = Instantiate(Resources.Load<GameObject>("PanelPrefab"),transform);
        objects.Add(message.obj);
        message.obj.GetComponent<RectTransform>().anchoredPosition = new(0f, top[3]);
        StartCoroutine(FillThePanelWith(message));
    }

    async void DeletePassedTurn(TurnID message)
    {
        int toIndex = 4;
        int fromIndex = 0;
        await PanelMove(message.obj.GetComponent<RectTransform>(), new Vector2(0f, top[toIndex]), toIndex, fromIndex,false);
        if (message.obj != null)
        {
            objects.Remove(message.obj);
            Destroy(message.obj);
        }
    }

    async void DeleteTurnOf(TurnID message)
    {
        int fromIndex = top.IndexOf(message.obj.GetComponent<RectTransform>().anchoredPosition.y);
        int toIndex = (fromIndex == 0)? 4 : 3;
        await PanelMove(message.obj.GetComponent<RectTransform>(), new Vector2(0f, top[toIndex]), toIndex, fromIndex, false);
        if (message.obj != null)
        {
            objects.Remove(message.obj);
            Destroy(message.obj);
        }
    }

    void LateUpdate()
    {
        if (!moving && taskFinishing.Count == 0 && cameraTurns.gameStarted)
        {
            foreach (TurnID turn in cameraTurns.turnAllocation.Where(a => a.obj != null))
            {
                int turnIndex = cameraTurns.turnAllocation.IndexOf(turn);
                int objIndex = top.IndexOf(turn.obj.GetComponent<RectTransform>().anchoredPosition.y);
                if (turnIndex != objIndex)
                {
                    moving = true;
                    if (turnIndex >= 3)
                    {
                        if(objIndex < turnIndex)
                        {
                            taskList.Add(PanelMove(turn.obj.GetComponent<RectTransform>(), new Vector2(0f, top[3]), 3, objIndex));
                        }
                        continue;
                    }
                    else if(turnIndex >= 0)
                    {
                        taskList.Add(PanelMove(turn.obj.GetComponent<RectTransform>(), new Vector2(0f, top[turnIndex]),turnIndex,objIndex));
                    }
                }
            }
            if(taskList.Count != 0)
            {
                StartCoroutine(IsAnyMoving());
            }  
        }
    }

    private IEnumerator IsAnyMoving()
    {
        while (taskList.Count != taskFinishing.Count)
        {
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitUntil(() => taskList.Count == taskFinishing.Count);
        taskList.Clear();
        taskFinishing.Clear();
        moving = false;
    }

    private async Task PanelMove(RectTransform trans, Vector2 destination,int toIndex,int fromIndex,bool passedTurn = true)
    {
        int speedDelta = 800, speedStep = 5;
        int speed = speedDelta * (Math.Abs(toIndex - fromIndex) < 4 ? Math.Abs(toIndex - fromIndex) : 1);
        while (Math.Abs(destination.y-trans.anchoredPosition.y) > 0.01f)
        {
            trans.anchoredPosition = Vector2.MoveTowards(trans.anchoredPosition, destination, speed*Time.deltaTime);
            if (speed - speedStep - 50 >= 0)
            {
                speed -= speedStep;
            }
            await Task.Delay(1);
            //yield return new WaitForEndOfFrame();
        }
        trans.anchoredPosition = Vector2.Lerp(trans.anchoredPosition, destination, 1);
        if (passedTurn)
        {
            taskFinishing.Add(true);
        }
    }

    private IEnumerator FillThePanelWith(TurnID turn)
    {
        yield return new WaitWhile(() => moving);
        toRemoveMoving = false;
        var imageComponent = turn.obj.transform.GetChild(0).GetComponent<Image>();
        turn.obj.GetComponent<GetID>().ID = turn.ID;
        imageComponent.sprite = Resources.Load<Sprite>("Sprites/"+turn.name);
        if (evil.Contains(turn.name))
        {
            imageComponent.color = new Color(0.7f, 0.4f, 0.4f, 1f);
            turn.obj.GetComponent<Image>().color = new Color(0.5f, 0.09f, 0.08f, 0.3f);
        }
        else
        {
            imageComponent.color = new Color(1f, 1f, 1f, 1f);
            turn.obj.GetComponent<Image>().color = new Color(0.5f, 0.9f, 0.3f, 0.3f);
        }
    }
}
