using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq.Expressions;
using UnityEngine.UI;

public class HandleEndGame : MonoBehaviour
{
    public Sprite[] sprites;
    public bool activate;
    [SerializeField] private TextMeshProUGUI prompt, detailedText;
    [SerializeField] private SpawnScriptableObject parameters;
    [SerializeField] private Animator canvasAnimator,levelNumberAnimator;
    [SerializeField] private Image backgroundImage, levelUpMaskImage, levelNumberEffectImage;
    [SerializeField] private RectTransform fillAnchorTransform;
    [SerializeField] private GameObject headerObject;
    private bool battleOutcome, isClosable;
    void Start()
    {
        sprites = new Sprite[3];
        battleOutcome = parameters.battleOutcome;
        //battleOutcome true = player wins
        canvasAnimator.enabled = true;
        if (battleOutcome)
        { 
            Color lightGreen = new Color(0f, 0.8f, 0, 1);
            Color darkGreen = new Color(0, 0.4f, 0, 1);
            VertexGradient grad = new(lightGreen, lightGreen, darkGreen, darkGreen);
            AudioClip clip = Resources.Load<AudioClip>("Audio/Music/Win");
            SetPrompt("Zwyciêstwo!", grad, clip);
            prompt.GetComponent<AudioSource>().loop = true;
            prompt.GetComponent<AudioSource>().volume = 0.25f;
            canvasAnimator.SetBool("Win Rotation",true);
            prompt.GetComponent<Animator>().SetTrigger("Win Rotation");
            sprites[0] = Resources.Load<Sprite>("Sprites/Meeko");
            sprites[1] = Resources.Load<Sprite>("Sprites/" + parameters.chars[0]);
            sprites[2] = Resources.Load<Sprite>("Sprites/" + parameters.chars[2]);
            StartCoroutine(SetLevelUpAnimation(grad));
        }
        else
        {
            sprites[0] = Resources.Load<Sprite>("Sprites/Mro¿on");
            sprites[1] = Resources.Load<Sprite>("Sprites/" + parameters.chars[1]);
            sprites[2] = Resources.Load<Sprite>("Sprites/"+ parameters.chars[3]);
            Color lightRed = new Color(0.8f,0,0,1);
            Color darkRed = new Color(0.4f,0,0,1);
            backgroundImage.color = lightRed;
            VertexGradient grad = new(lightRed, lightRed, darkRed, darkRed); ;
            AudioClip clip = Resources.Load<AudioClip>("Audio/Music/Loss");
            levelUpMaskImage.color = lightRed;
            SetPrompt("Pora¿ka!", grad, clip);
            StartCoroutine(HideCanvas(grad));
            isClosable = true;
        }
        prompt.GetComponent<Animator>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && isClosable) 
        {
            Application.Quit();
        }
        if (activate)
        {
            activate = false;
            canvasAnimator.enabled = true;
        }
    }

    IEnumerator SetLevelUpAnimation(VertexGradient grad)
    {
        yield return new WaitUntil(() => canvasAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        while(levelUpMaskImage.color.a > 0)
        {
            Color newColor = levelUpMaskImage.color;
            newColor.a -= Time.deltaTime;
            levelUpMaskImage.color = newColor;
            yield return new WaitForEndOfFrame();
        }
        while(fillAnchorTransform.localScale.x < 2f)
        {
            Vector3 newScale = fillAnchorTransform.localScale;
            newScale.x += Time.deltaTime;
            fillAnchorTransform.localScale = newScale;
            yield return new WaitForEndOfFrame();
        }
        headerObject.SetActive(true);
        levelNumberAnimator.enabled = true;
        prompt.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Audio/Magic/ElectricHit"),1.3f);
        yield return new WaitWhile(() => levelNumberEffectImage.color.a == 1);
        levelNumberAnimator.GetComponent<TextMeshProUGUI>().text = "MAX";
        levelNumberAnimator.GetComponent<TextMeshProUGUI>().fontMaterial = Resources.Load<Material>("Materials/YellowMaterial");
        yield return new WaitUntil(() => Input.anyKey);
        while (levelUpMaskImage.color.a < 1)
        {
            Color newColor = levelUpMaskImage.color;
            newColor.a += Time.deltaTime;
            levelUpMaskImage.color = newColor;
            yield return new WaitForEndOfFrame();
        }
        SetDetails("Gratulacje! \n\n Znasz ju¿ wszystkie tajniki jakich mog³em Ciê nauczyæ. \n\nZakoñczy³aœ PROJEKT M\n\nSpójrz na swojego steam'a by odebraæ nagrodê.\n\nDziêkujê za grê!", grad);
        yield return new WaitForSecondsRealtime(2f);
        isClosable = true;
        yield return new WaitUntil(() => Input.anyKey);
        Application.Quit();
    }

    void SetDetails(string text, VertexGradient grad)
    {
        Material fontMaterial = Resources.Load<Material>("Materials/FinishingText");
        detailedText.text = text;
        detailedText.colorGradient = grad;
        detailedText.fontMaterial = fontMaterial;
        detailedText.GetComponent<Animator>().enabled = true;
    }


    void SetPrompt(string text,VertexGradient grad, AudioClip clip)
    {
        TextMeshProUGUI promptShadow = prompt.transform.Find("PromptShadow").GetComponent<TextMeshProUGUI>();
        Material fontMaterial = Resources.Load<Material>("Materials/FinishingText");
        prompt.text = text;
        prompt.colorGradient = grad;
        promptShadow.text = text;
        promptShadow.colorGradient = grad;
        prompt.GetComponent<AudioSource>().clip = clip;
        prompt.GetComponent<AudioSource>().Play();
        prompt.fontMaterial = fontMaterial;
        promptShadow.fontMaterial = fontMaterial;
    }

    IEnumerator HideCanvas(VertexGradient grad)
    {
        yield return new WaitForSeconds(3f);
        SetDetails("Niestety, nie uda³o ci siê pokonaæ Mro¿ona. \n\nSpróbuj jeszcze raz.", grad);
        yield return new WaitUntil(() => prompt.GetComponent<AudioSource>().time > 10.273f);
        canvasAnimator.SetTrigger("Hide");
        yield return new WaitUntil(() => canvasAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && canvasAnimator.GetCurrentAnimatorStateInfo(0).IsName("CanvasMove"));
        Application.Quit();
    }
}
