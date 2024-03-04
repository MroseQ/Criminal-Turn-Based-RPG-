using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ErrorCreation : MonoBehaviour
{
    public void Create(string message)
    {
        GameObject error = Instantiate(Resources.Load<GameObject>("ErrorText"),transform);
        error.GetComponent<TextMeshProUGUI>().text = message;
        StartCoroutine(Destruction(error));
    }

    private IEnumerator Destruction(GameObject error)
    {
        yield return new WaitUntil(() => error.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        Destroy(error);
    }
}
