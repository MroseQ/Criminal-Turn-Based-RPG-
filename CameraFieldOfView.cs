using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static CalculateTurns;

public class CameraFieldOfView : MonoBehaviour
{
    private Camera cameraComponent;
    private GameObject movingToTarget;
    public List<GameObject> targets = new();
    [SerializeField] private bool moving, scaling,actionPerformed;
    [SerializeField]private float startSize;
    [SerializeField]private float scaleSpeed, moveSpeed;
    [SerializeField] private Vector2 startPosition;
    void OnEnable()
    {
        cameraComponent = GetComponent<Camera>();
        startPosition = transform.position;
        startSize = cameraComponent.orthographicSize;
        MessageOfActionPerformed += ActionPerformed;
        movingToTarget = null;
    }

    void ActionPerformed(bool msg)
    {
        actionPerformed = msg;
    }
    
    void Update()
    {
        if(actionPerformed)
        {
            if (!scaling && cameraComponent.orthographicSize != startSize)
            {
                StartCoroutine(ScaleSize(startSize));
            }
            if (!moving && new Vector2(transform.position.x,transform.position.y) != startPosition)
            {
                StartCoroutine(MoveTo(null));
            } 
        }
    }

    private void LateUpdate()
    {
        if(targets.Count > 0 && (movingToTarget == null || movingToTarget.GetComponent<ProjectileFade>().hit))
        {
            StopAllCoroutines();
            moving = false;
            scaling = false;
            movingToTarget = targets[0];
            if (!moving)
            {
                StartCoroutine(MoveTo(targets[0].transform));
            }
            if (!scaling)
            {
                StartCoroutine(ScaleSize(4.5f));
            }
            targets.RemoveAt(0);
        } 
    }

    private IEnumerator MoveTo(Transform objectTransform)
    {
        Vector2 destination;
        if (objectTransform == null)
        {
            destination = startPosition;
        }
        else
        {
            destination = objectTransform.position;
        }
        bool canFinish = false;
        while (!(canFinish && Vector2.Distance(transform.position, destination) <= 0.01f))
        {
            if(objectTransform != null)
            {
                destination = objectTransform.position*0.3f;
            }
            else
            {
                canFinish = true;
                destination = startPosition;
            }
            moving = true;
            Vector3 moveDelta = Vector2.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
            moveDelta.z = -10f;
            transform.position = moveDelta;
            if (actionPerformed)
            {
                canFinish = true;
            }
            yield return new WaitForFixedUpdate();
        }
        Vector3 lastMoveDelta = Vector2.Lerp(transform.position, destination*(objectTransform==null?1f:0.3f), 1);
        lastMoveDelta.z = -10f;
        transform.position = lastMoveDelta;
        movingToTarget = null;
        moving = false;
    }

    private IEnumerator ScaleSize(float toSize)
    {
        float startSize = cameraComponent.orthographicSize;
        while (!CheckIfSizeOverride(cameraComponent.orthographicSize,toSize,startSize))
        {
            scaling = true;
            if(cameraComponent.orthographicSize < toSize)
            {
                cameraComponent.orthographicSize += scaleSpeed*Time.deltaTime;
            }
            else
            {
                cameraComponent.orthographicSize -= scaleSpeed * Time.deltaTime;
            }
            yield return new WaitForFixedUpdate();
        }
        scaling = false;
        cameraComponent.orthographicSize = toSize;
    }

    private bool CheckIfSizeOverride(float a, float b, float start)
    {
        if (b < start)
        {
            if(a <= b)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (b > start)
        {
            if(a > b)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }
}
