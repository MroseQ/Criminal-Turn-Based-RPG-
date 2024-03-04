using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OrbAnimation : MonoBehaviour
{
    public GraphicRaycaster raycaster;
    public PointerEventData data;
    public int speed;
    public GameObject nowSkill, toDisable;
    public List<GameObject> prevSkills = new List<GameObject>();
    public delegate void SendingSkill(string message);
    public static event SendingSkill PickedSkill;
    void Start()
    {
        speed = 2;
        data = new PointerEventData(EventSystem.current);
    }

    void Update()
    {
        data.position = Input.mousePosition;
        var results = new List<RaycastResult>();
        raycaster.Raycast(data, results);
        if (results.Count != 0 && !Camera.main.GetComponent<CalculateTurns>().isCorrectPick)
        {
            var result = results.Find(result => result.gameObject.name == "InsideOrb");
            if (nowSkill != result.gameObject.transform.parent.transform.parent.gameObject || nowSkill == null)
            {
                nowSkill = result.gameObject.transform.parent.transform.parent.gameObject;
                if (nowSkill.transform.Find("SkillDescription").gameObject.activeSelf)
                {
                    StopCoroutine(HideDescription(nowSkill.transform.Find("SkillDescription").gameObject));
                    nowSkill.transform.Find("SkillDescription").gameObject.GetComponent<Animator>().ResetTrigger("Hide");
                    nowSkill.transform.Find("SkillDescription").gameObject.GetComponent<Animator>().SetBool("StartedHiding", false);
                    nowSkill.transform.Find("SkillDescription").gameObject.GetComponent<Animator>().Play("SkillDescription");
                }
                else
                {
                    nowSkill.transform.Find("SkillDescription").gameObject.SetActive(true);
                }
                prevSkills.Add(nowSkill);
            }
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                nowSkill.transform.Find("SkillDescription").gameObject.SetActive(false);
                PickedSkill.Invoke(nowSkill.name);
            }
        }
        else if (nowSkill != null)
        {
            if (!nowSkill.transform.Find("SkillDescription").GetComponent<Animator>().GetBool("StartedHiding"))
            {
                StartCoroutine(HideDescription(nowSkill.transform.Find("SkillDescription").gameObject));
            }
            if (nowSkill.transform.localScale.x > 1f)
            {
                nowSkill.transform.localScale -= new Vector3(speed * Time.deltaTime, speed * Time.deltaTime, 0);
            }
            else
            {
                nowSkill.transform.localScale = new Vector3(1f, 1f, nowSkill.transform.localScale.z);
                nowSkill = null;
            }
        }
        
        if (nowSkill != null && results.Count != 0)
        {
            if (nowSkill.transform.localScale.x < 1.2f)
            {
                nowSkill.transform.localScale += new Vector3(speed * Time.deltaTime, speed * Time.deltaTime, 0);
            }
            else
            {
                nowSkill.transform.localScale = new Vector3(1.2f, 1.2f, nowSkill.transform.localScale.z);
            }
        }
        foreach(GameObject skill in prevSkills)
        {
            if (skill != nowSkill)
            {
                if (skill.transform.localScale.x > 1f)
                {
                    skill.transform.localScale -= new Vector3(speed * Time.deltaTime, speed * Time.deltaTime, 0);
                }
                else
                {
                    skill.transform.localScale = new Vector3(1f, 1f, skill.transform.localScale.z);
                    prevSkills.Remove(skill);
                    break;
                }
                if (!skill.transform.Find("SkillDescription").GetComponent<Animator>().GetBool("StartedHiding"))
                {
                    StartCoroutine(HideDescription(skill.transform.Find("SkillDescription").gameObject));
                }
            }
        }
    }

    private IEnumerator HideDescription(GameObject desc)
    {
        desc.GetComponent<Animator>().SetBool("StartedHiding", true);
        desc.GetComponent<Animator>().SetTrigger("Hide");
        yield return new WaitUntil(() => desc.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("SkillDescriptionHide") && desc.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        desc.GetComponent<Animator>().SetBool("StartedHiding", false);
        desc.SetActive(false);
    }
}
