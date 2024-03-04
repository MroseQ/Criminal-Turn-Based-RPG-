using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HandleParameters : MonoBehaviour
{
    [SerializeField] private List<string> handler = new();
    private Dictionary<string, Task> tasks = new();
    private Dictionary<string, CancellationTokenSource> cancellationTokenSources = new();
    [SerializeField] private List<GameObject> characterObjects,charactersPositions;
    [SerializeField] private SpawnScriptableObject SO;

    private void Awake()
    {
        handler.Clear();
        foreach(string name in SO.chars)
        {
            handler.Add("Characters/"+name);
        }
        handler.Add("Characters/Meeko");
        handler.Add("Characters/Mrożon");
        for (int i = 0; i < 4; i++)
        {
            string charName = SO.chars[i];
            GameObject charObject = characterObjects.Find(obj => obj.name == charName);
            charObject.SetActive(true);
            charObject.transform.SetParent(charactersPositions[i].transform);
            charObject.transform.localPosition = new(-8, charObject.transform.localPosition.y, charObject.transform.localPosition.z);
        }
        InsertIntoAssets();
    }

    private void Start()
    {
        foreach (string path in handler) 
        {
            CharactersParameters parameter = Resources.Load<CharactersParameters>(path);
            GameObject hp = parameter.charPanel.transform.Find("HPmask").gameObject;
            GameObject mana = parameter.charPanel.transform.Find("ManaMask").gameObject;
            hp.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = parameter.name;
            hp.transform.Find("MaxValue").GetComponent<TextMeshProUGUI>().text = "/ "+parameter.maxHP;
            mana.transform.Find("MaxValue").GetComponent<TextMeshProUGUI>().text = "/ " + parameter.maxMana;
        }
    }

   

    private async Task ChangeBarAsync(float to, float max, RectTransform bar, float time, CancellationToken cancellationToken)
    {
        float percentage = to / max;
        while (Mathf.Abs(bar.localScale.x - percentage) > 0.001f)
        {
            float difference = Mathf.Abs(bar.localScale.x - percentage);
            bar.localScale = Vector3.MoveTowards(bar.localScale, new Vector3(percentage, bar.localScale.y, bar.localScale.z), Time.deltaTime * difference / time);
            await Task.Delay(1, cancellationToken);
        }
        bar.localScale = new Vector3(percentage, bar.localScale.y, bar.localScale.z);
    }

    private void CancelAndReplaceTask(string key, CancellationTokenSource cts, float to, float max, RectTransform bar, float time)
    {
        cts?.Cancel();
        cts?.Dispose();

        var newCts = new CancellationTokenSource();
        cancellationTokenSources[key] = newCts;

        var newTask = ChangeBarAsync(to, max, bar, time, newCts.Token);
        Task.Run(async () => await newTask); // Start the new task
    }

    public void StartOrReplaceTask(string path, GameObject obj, string anchor, float time, int currentValue, int maxValue)
    {
        if (cancellationTokenSources.TryGetValue(path, out CancellationTokenSource token))
        {
            CancelAndReplaceTask(path, token, currentValue, maxValue, obj.transform.Find(anchor).GetComponent<RectTransform>(), time);
        }
        else
        {
            var newCts = new CancellationTokenSource();
            cancellationTokenSources[path] = newCts;
            var newTask = ChangeBarAsync(currentValue, maxValue, obj.transform.Find(anchor).GetComponent<RectTransform>(), 0.25f, newCts.Token);
            Task.Run(async () => await newTask); // Start the new task
        }
    }

private void LateUpdate()
    {
        float nowHP, nowMana;
        GameObject hp, mana;
        foreach (string path in handler)
        {
            CharactersParameters parameter = Resources.Load<CharactersParameters>(path);
            hp = parameter.charPanel.transform.Find("HPmask").gameObject;
            mana = parameter.charPanel.transform.Find("ManaMask").gameObject;
            nowHP = int.Parse(hp.transform.Find("Value").GetComponent<TextMeshProUGUI>().text);
            nowMana = int.Parse(mana.transform.Find("Value").GetComponent<TextMeshProUGUI>().text);
            if(nowHP != parameter.currentHP)
            {
                StartOrReplaceTask("hp" + path, hp, "AnchorHP", 0.25f,parameter.currentHP,parameter.maxHP);
                StartOrReplaceTask("hit" + path, hp, "AnchorHit", 0.5f, parameter.currentHP, parameter.maxHP);
                hp.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = parameter.currentHP.ToString();
            }
            if(nowMana != parameter.currentMana)
            {
                StartOrReplaceTask("mana" + path, mana, "AnchorMana", 0.35f, parameter.currentMana, parameter.maxMana);
                mana.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = parameter.currentMana.ToString();
            }

            /*
            if (nowHP != parameter.currentHP)
            {
                tasks.TryGetValue("hp" + path, out Task prevTask);
                if (prevTask != null)
                {
                    prevTask.Dispose();
                    tasks.Remove("hp" + path);
                }
                tasks.TryGetValue("hit" + path, out Task prevTaskHit);
                if (prevTaskHit != null)
                {
                    prevTask.Dispose();
                    tasks.Remove("hit" + path);
                }
                tasks.Add("hp" + path, ChangeBarAsync(parameter.currentHP, parameter.maxHP, hp.transform.Find("AnchorHP").GetComponent<RectTransform>(), 0.25f));
                tasks.Add("hit" + path, ChangeBarAsync(parameter.currentHP, parameter.maxHP, hp.transform.Find("AnchorHit").GetComponent<RectTransform>(), 0.5f));
                
            }
            if (nowMana != parameter.currentMana)
            {
                tasks.TryGetValue("mana"+path, out Task prevTask);
                if(prevTask != null)
                {
                    prevTask.Dispose();
                    tasks.Remove("mana" + path);
                }
                tasks.Add("mana" + path,ChangeBarAsync(parameter.currentMana, parameter.maxMana, mana.transform.Find("AnchorMana").GetComponent<RectTransform>(), 0.35f));
                
            }
            */
        }
    }

 

    public void InsertIntoAssets()
    {
        foreach (string path in handler)
        {
            CharactersParameters parameters = Resources.Load<CharactersParameters>(path);
            GetComponent<CalculateTurns>().charParameters.Add(parameters);
            parameters.skills.Clear();
            parameters.effects.Clear();
            parameters.characterObject = GameObject.Find(parameters.name);
            parameters.charCollider = parameters.characterObject.transform.Find("Hit").GetComponent<BoxCollider2D>();
            if (parameters.name == "Mrożon")
            {
                parameters.charPanel = GameObject.Find("BarsBoss");
                parameters.skills.Add(new SkillDetails("Ice Cone", 3,30, "enemy","debuff",15,5)); //stun
                parameters.skills.Add(new SkillDetails("Snowball", 0, 0, "enemy","projectile",30,5)); //Normalny atak
                parameters.skills.Add(new SkillDetails("Solid Ice", 5, 10,  "self","buff",85,5)); //Shield numeryczny
                parameters.skills.Add(new SkillDetails("Freezing Gun", 3, 20, "allEnemies","projectile",10,3)); //duzo hitów
                parameters.skills.Add(new SkillDetails("Shattered Ice", 2,25, "enemy","debuff")); //jakiś efekt
                parameters.skills.Add(new SkillDetails("Ice Wallow", 2,0, "allAllies","buff")); // Regen many
                parameters.skills.Add(new SkillDetails("Blood Freeze", 4, 50, "allEnemies","projectile",25,5)); //AoE 
                parameters.skills.Add(new SkillDetails("Slip And Slide", 4,10, "enemy","projectile",40,15)); //zmniejsza szanse na dodge
            }else if(parameters.name == "Meeko"){
                parameters.charPanel = GameObject.Find("BarsPlayer");
                parameters.skillsPos = GameObject.Find("Ally Spots").transform.Find("Spot 2").transform.Find("Skills").gameObject;
                parameters.skills.Add(new SkillDetails("Gacha Addict", 0,0, "self","buff",25,5)); 
                parameters.skills.Add(new SkillDetails("Power of Friendship", 3,30, "allAllies","buff"));
                parameters.skills.Add(new SkillDetails("Killjoy", 3, 10,  "self","buff")); // Gdy zostanie zaatakowana narzuca atakującemu 2 stack bleeda.
                parameters.skills.Add(new SkillDetails("High Ping", 4, 20, "ally","buff")); // Przywraca ostatnie utracone zdrowie przez wskazanego sojusznika oraz niweluje debuffy
                parameters.skills.Add(new SkillDetails("Cuteness Overload", 5,5, "boss","projectile",60,10)); //Tylko na Mrożona, zmniejszone obrażenia, potężne uderzenie
                parameters.skills.Add(new SkillDetails("Ace", 3, 20 , "allEnemies","projectile",12,4)); //AoE - 5 strzałów
            }
            else
            {
                for(int i=0;i<SO.chars.Length;i++)
                {
                    if (parameters.name == SO.chars[i])
                    {
                        switch (i)
                        {
                            case 0:
                                parameters.charPanel = GameObject.Find("BarsTopAlly");
                                parameters.skillsPos = GameObject.Find("Ally Spots").transform.Find("Spot 1").transform.Find("Skills").gameObject;
                                break;
                            case 1:
                                parameters.charPanel = GameObject.Find("BarsTopEnemy");
                                break;
                            case 2:
                                parameters.charPanel = GameObject.Find("BarsBottomAlly");
                                parameters.skillsPos = GameObject.Find("Ally Spots").transform.Find("Spot 3").transform.Find("Skills").gameObject;
                                break;
                            case 3:
                                parameters.charPanel = GameObject.Find("BarsBottomEnemy");
                                break;
                        }
                    }
                }
                if (parameters.name == "Keiji Shinogi")
                {
                    parameters.skills.Add(new SkillDetails("Shotgun", 0,0, "allEnemies", "projectile",25,5)); //atak na wszystkich, głowny cel 100%, reszta 40%
                    parameters.skills.Add(new SkillDetails("Defend The Queen", 4,10, "self","buff")); //Prowokacja, 20% obrażeń mniej.
                    parameters.skills.Add(new SkillDetails("Target", 4,20, "enemy","debuff")); //Zwiększone obrażenia na cel.
                    parameters.skills.Add(new SkillDetails("Precise Shot", 3,25, "enemy","projectile",30,10)); //Atak na jednego przeciwnika ze zwiększoną szansą oraz wartością krytycznych obrażeń.
                }
                else if (parameters.name == "Evil Keiji")
                {
                    parameters.skills.Add(new SkillDetails("Shotgun", 0,15, "allEnemies","projectile",25,10));
                    parameters.skills.Add(new SkillDetails("Precise Shot", 3,25, "enemy","projectile",35,10));
                    parameters.skills.Add(new SkillDetails("Target", 4,25, "enemy","debuff"));
                    parameters.skills.Add(new SkillDetails("Knife Stab", 0,0, "enemy","projectile",20,5)); //zwykły atak
                }
                else if (parameters.name == "Evil Marek")
                {
                    parameters.skills.Add(new SkillDetails("Fire Dragon", 3,30, "allEnemies","projectile",25,7)); //AoE
                    parameters.skills.Add(new SkillDetails("Hack", 6,20, "self","buff")); // ponowienie tury ale zmniejszone obrażenia.
                    parameters.skills.Add(new SkillDetails("Pawulon's Nemesis", 0,0, "enemy","projectile",20,5)); //zwykły atak
                    parameters.skills.Add(new SkillDetails("Insulin Shot", 5,10, "ally","buff",35,10)); //heal 
                }
                else if (parameters.name == "Marek")
                {
                    parameters.skills.Add(new SkillDetails("Pawulon's Nemesis", 0, 0, "enemy", "projectile", 25, 5));
                    parameters.skills.Add(new SkillDetails("Insulin Shot", 5, 0, "ally", "buff", 45, 10)); //heal
                    parameters.skills.Add(new SkillDetails("Cheer Up", 4,20, "allAllies","buff")); // zwiększone obrażenia
                    parameters.skills.Add(new SkillDetails("Braindead Play", 3,20, "enemy","projectile",35,10)); // Nakłada na jednego przeciwnika 'Brainless' utrzymujący się 3 tury, niwelując szansę na obrażenia krytyczne i unik.
                }
                else if (parameters.name == "Evil Asu")
                {
                    parameters.skills.Add(new SkillDetails("Richard's Blade", 0,0, "enemy","projectile",15,7)); // zwykły atak
                    parameters.skills.Add(new SkillDetails("Game's Bug", 5,20, "allEnemies","projectile",40,10)); //AoE
                    parameters.skills.Add(new SkillDetails("Woroh's Hair", 3,20, "enemy","projectile",9,1)); //speed zmniejszony
                    parameters.skills.Add(new SkillDetails("Mrożon's Turn", 3,30, "boss","buff")); //oddaje ture mrożonowi
                }
                else if (parameters.name == "Asu")
                {
                    parameters.skills.Add(new SkillDetails("Richard's Blade", 0, 0, "enemy", "projectile", 15, 7)); //zwykly atak
                    parameters.skills.Add(new SkillDetails("Woroh's Hair", 3, 20, "enemy", "projectile", 7, 1)); //speed zmniejszony
                    parameters.skills.Add(new SkillDetails("Woroh's Sword", 4,40, "allEnemies","projectile",30,7)); //bleed
                    parameters.skills.Add(new SkillDetails("Asu's Revenge", 3,10, "ally","buff")); // kontra z bleedem
                }
                else if (parameters.name == "Nagito Komaeda")
                {
                    parameters.skills.Add(new SkillDetails("Russian Roulette", 0,5, "enemy","projectile",80,10)); // 1/6 strzela w siebie, a przy braku wystrzału 1/5 w przeciwnika
                    parameters.skills.Add(new SkillDetails("Laugh", 4,15, "allEnemies","debuff")); // nakłada zmniejszone obrażenia na jeden atak.
                    parameters.skills.Add(new SkillDetails("Fixed Shot", 4,50, "enemy","projectile",70,5)); //potezny atak.
                    parameters.skills.Add(new SkillDetails("Knife Stab", 0,0, "enemy","projectile", 20, 5)); // tylko jezeli nie ma many. zwykly atak
                }
                else if (parameters.name == "Hatsune Miku")
                {
                    parameters.skills.Add(new SkillDetails("Let's Do It Again", 4,25, "allAllies","buff")); //Przywraca sojusznikom 25% many, poza sobą!
                    parameters.skills.Add(new SkillDetails("Streaming Heart", 4,25, "allAllies","buff",30, 5)); // Przywraca wszystkim zdrowie
                    parameters.skills.Add(new SkillDetails("Unhappy Refrain", 3,15, "enemy","debuff")); // Zmniejszone obrażenia przeciwników
                    parameters.skills.Add(new SkillDetails("Levan Polkka", 0,0, "enemy","projectile",18,3)); // zwykly atak
                }
            }
            parameters.effectPanel = parameters.charPanel.transform.Find("Effects").gameObject;
            parameters.currentHP = parameters.maxHP;
            parameters.currentMana = parameters.maxMana;
            parameters.currentSpeed = parameters.speed;
            parameters.currentDodgeChance = parameters.dodgeChance;
            parameters.currentCritChance = parameters.critChance;
            parameters.parentTransform = parameters.characterObject.transform.parent.transform;
            parameters.IsDead = false;
            if (!parameters.enemy)
            {
                parameters.skillsPos.GetComponent<MainAssets.SkillOrbsBehaviour>().characterInfo = parameters;
                foreach (SkillDetails elem in parameters.skills)
                {
                    GameObject skill = Instantiate(Resources.Load<GameObject>("SkillPrefab"));
                    skill.transform.SetParent(parameters.skillsPos.transform);
                    skill.transform.localPosition = Vector3.zero;
                    skill.name = elem.skillName;
                    skill.transform.Find("InsideOrbMask").Find("InsideOrb").GetComponent<Image>().sprite = Resources.Load<Sprite>("SkillTexture/"+elem.skillName+" Texture");
                    Transform skillDesc = skill.transform.Find("SkillDescription").transform;
                    skillDesc.Find("Name").GetComponent<TextMeshProUGUI>().text = elem.skillName;
                    string pathToMaterial = FindCharactersMaterial(parameters.name);
                    skillDesc.Find("Name").GetComponent<TextMeshProUGUI>().fontMaterial = Resources.Load<Material>(pathToMaterial);
                    skillDesc.Find("Description").GetComponent<TextMeshProUGUI>().text = elem.skillDesc;
                    skillDesc.Find("CooldownNumber").GetComponent<TextMeshProUGUI>().text = elem.skillCooldown.ToString();
                    skillDesc.Find("ManaDrainNumber").GetComponent<TextMeshProUGUI>().text = elem.skillManaDrain.ToString();
                }
            }
        }
    }

    private string FindCharactersMaterial(string name)
    {
        string path;
        if (name == "Meeko")
        {
            path = "Materials/EffectPurpleBlueMaterial";
        }
        else if(name == "Asu")
        {
            path = "Materials/EffectPurpleMaterial";
        }
        else if(name == "Marek")
        {
            path = "Materials/EffectGreenMaterial";
        }
        else if (name == "Keiji Shinogi")
        {
            path = "Materials/EffectOrangeMaterial";
        }
        else
        {
            path = "Materials/EffectBlueMaterial";
        }
        return path;
    }
}
