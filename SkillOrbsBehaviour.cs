using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainAssets
{
    public class SkillOrbsBehaviour : MonoBehaviour
    {
        public CharactersParameters characterInfo;
        private float oneAngle;
        public float radius, speed;
        public List<Vector3> listPos;
        public List<Transform> skillObjects;
        private bool started;

        private void Awake()
        {
            int quantity = characterInfo.skills.Count;
            radius = 2.3f;
            oneAngle = (130f / quantity);
            for (int i = 0; i < quantity; i++)
            {
                listPos.Add(CalculatePosition(oneAngle * i));
                skillObjects.Add(transform.Find(characterInfo.skills[i].skillName).transform);
            }
        }
        private void OnEnable()
        {
            StopAllCoroutines();
            StartCoroutine(Moving(listPos));
        }

        private void Update()
        {
            if (Camera.main.GetComponent<CalculateTurns>().isCorrectPick && !started)
            {
                List<Vector3> vectorZeroList = new();
                for (int i = 0; i < listPos.Count; i++)
                {
                    vectorZeroList.Add(Vector3.zero);
                }
                StartCoroutine(Moving(vectorZeroList));
            }
            foreach(Transform obj in skillObjects)
            {
                SkillDetails skill = characterInfo.skills.Find(s => s.skillName == obj.name);
                GameObject cd = obj.Find("InsideOrbMask").Find("Cooldown").gameObject;
                if (skill.nowSkillCooldown != 0 && !Camera.main.GetComponent<CalculateTurns>().isCorrectPick)
                {
                    cd.SetActive(true);
                    cd.transform.Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = skill.nowSkillCooldown.ToString();
                }
                else
                {
                    cd.SetActive(false);
                }
            }
        }

        public IEnumerator Moving(List<Vector3> to)
        {
            started = true;
            bool sign = false;
            if(to[0] == Vector3.zero)
            {
                sign = true;
            }
            int angle = 0;
            while(skillObjects[^1].localPosition != to[skillObjects.Count - 1])
            {
                for (int i = 0; i < listPos.Count; i++)
                {
                    ChangeAlpha(skillObjects[i].Find("InsideOrbMask").transform.Find("InsideOrb").gameObject,sign);
                    skillObjects[i].localPosition = Vector3.MoveTowards(skillObjects[i].localPosition,to[i],Mathf.Cos(angle*Mathf.Deg2Rad)* speed * Time.deltaTime);
                }
                if (angle + 5 > 89)
                {
                    angle = 80;
                }
                else
                {
                    angle += 5;
                }
                yield return new WaitForFixedUpdate();
            }
            for (int i = 0; i < listPos.Count; i++)
            {
                skillObjects[i].localPosition = Vector3.Lerp(skillObjects[i].localPosition, to[i], 1);
            }
            yield return new WaitForFixedUpdate();
            if (to[0] == Vector3.zero)
            {
                gameObject.SetActive(false);
            }
            started = false;
        }

        public Vector3 CalculatePosition(float angle)
        {
            Vector3 newPos;
            newPos.x = radius * Mathf.Sin(angle * Mathf.Deg2Rad);
            newPos.y = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            newPos.z = 0;
            return newPos;
        }

        public void ChangeAlpha(GameObject obj, bool sign)
        {
            Color objColor = obj.GetComponent<Image>().color;
            if (sign && objColor.a > 0f)
            {
                objColor.a -= speed * Time.deltaTime;
                if(objColor.a < 0f)
                {
                    objColor.a = 0f;
                }
            }
            else if(!sign && objColor.a < 1f)
            {
                objColor.a += speed * Time.deltaTime;
                if (objColor.a > 1f)
                {
                    objColor.a = 1f;
                }
            }
            obj.GetComponent<Image>().color = objColor;
        }
    }
}
