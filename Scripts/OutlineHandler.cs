using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineHandler : MonoBehaviour
{
    public GameObject selected;
    public CharactersParameters confirmed;
    public void ChangeSelection(GameObject now)
    {
        if(selected != null)
        {
            selected.SetActive(false);
        }
        selected = now;
    }
    public IEnumerator ConfirmedSelection(GameObject now)
    {
        selected = null;
        if(GetComponent<CalculateTurns>().targets.Count > 0)
        {
            confirmed = GetComponent<CalculateTurns>().targets.Find(target => target.name == now.transform.parent.name);
            if(confirmed != null)
            {
                yield return new WaitUntil(() => GetComponent<CalculateTurns>().targets.Count == 1);
            }
        }
        confirmed = null;
    }

}
