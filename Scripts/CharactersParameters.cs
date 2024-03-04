using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "CharParam", menuName = "ScriptableObjects/CharParam", order = 1)]
[Serializable]
public class CharactersParameters : ScriptableObject
{
    public int currentHP, maxHP,currentMana,maxMana, speed, currentSpeed, dodgeChance, critChance, currentDodgeChance, currentCritChance;
    public GameObject charPanel, skillsPos, effectPanel,characterObject;
    public bool enemy,IsBoss,IsDead;
    public BoxCollider2D charCollider;
    public List<EffectDetails> effects = new();
    public List<SkillDetails> skills = new();
    public Transform parentTransform;
    public float volumeScale;
    public string castPath;
}
[Serializable]
public class SkillDetails
{
    public string skillName { get; set; }
    public int skillCooldown { get; set; }
    public string skillTarget { get; set; }
    public string skillType { get; set; }
    public int skillManaDrain { get; set; }
    public int skillValue { get; set; }
    public int skillValueRange { get; set; }
    public int nowSkillCooldown = 0;
    public string skillDesc;

    public SkillDetails(string name, int cooldown, int manaDrain, string target,string type, int value = 0, int valueRange = 0)
    {
        skillName = name;
        skillCooldown = cooldown;
        skillTarget = target;
        skillType = type;
        skillManaDrain = manaDrain;
        skillValue = value;
        skillValueRange = valueRange;
        Dictionary<string, string> actionMap = new()
        {
            { "Ice Cone", "Uwięzienie w słupie lodu, ogłuszając na 1 turę." },
            { "Snowball", "Śnieżka." },
            { "Solid Ice", "Zimna tarcza, która broni od obrażeń i ma do 90 punktów życia." },
            { "Freezing Gun", "Strzela bronią śnieżną bijąc kilka razy." },
            { "Shattered Ice", "Narucza 'Shattered' na każdego przeciwnika" },  
            { "Ice Wallow", "Narzuca 'Unfreeze' regenerując mane co ture każdemu sojusznikowi." },
            { "Blood Freeze", "Potężny atak obszarowy." }, 
            { "Slip And Slide", "Zmniejsza szansę na unik trafionemu przeciwnikowi." },
            { "Gacha Addict", "Ruletka:\n15%: przywrócenie sobie całej many\n15%: przywrócenie komuś do 70 HP\n10%: CRIT 4x hit   25%: 2x hit   25%: 1x hit\n10%: niewypał" },
            { "Power of Friendship", "Poświęcając kolejną turę, sojusznicy raz zyskują dwie tury zamiast jednej."},
            { "Killjoy", "Narzuca na siebie efekty 'Contra' i 'Provocation'" },
            { "High Ping", "Przyśpiesza postać, powodując częstsze występowanie." },
            { "Cuteness Overload", "Tylko na Mrożona - Potężne uderzenie." },
            { "Ace", "Pięć oddzielnych wystrzałów." },
            { "Shotgun", "Trafia wszystkich przeciwników, główny cel otrzymuje 100% obrażeń, a reszta 40%." },
            { "Precise Shot", "Atak na jednego przeciwnika ze zwiększoną szansą oraz wartością krytycznych obrażeń." },
            { "Target", "Na przeciwny cel zostaje nadany efekt 'Targeted', zwiększający zadawane obrażenia na dwie tury." },
            { "Knife Stab", "Atak nożem." },
            { "Fire Dragon", "AoE narzucające 'Burn' na dwie tury." },
            { "Hack", "Narzuca na siebie 'Repeat' i 'Cold'." },
            { "Pawulon's Nemesis", "Pięści, które pokonały Pawulona." },
            { "Insulin Shot", "Przywraca sojusznikowi do 50 HP oraz many." },
            { "Richard's Blade", "Szpon Richarda." },
            { "Game's Bug", "???" },
            { "Woroh's Hair", "Zmniejsza szybkość przeciwnika." },
            { "Mrożon's Turn", "Oddanie tury Mrożonowi." },
            { "Russian Roulette", "Jeżeli nie ma efektu 'Game Begun', narzuca ten efekt. Wykonuje dwie próby strzału - pierwszą w siebie, drugą w przeciwnika." },
            { "Laugh", "Nakłada na wszystkich przeciwników 'Scared', zmniejszając zadawane obrażenia na jedną turę." },
            { "Fixed Shot", "Usuwa efekt 'Game Begun' (jeżeli jest) i poprostu strzela." },
            { "Cheer Up", "Nakłada na wszystkich sojuszników efekt 'Cheered'." },
            { "Braindead Play", "Nakłada na jednego przeciwnika 'Brainless' utrzymujący się 3 tury, niwelując szansę na obrażenia krytyczne i unik." },
            { "Woroh's Sword", "Atak długim mieczem Woroha, nadaje 'Bleed'." },
            { "Asu's Revenge", "Narzuca na sojusznika efekt 'Contra'." },
            { "Let's Do It Again", "Przywraca tylko swoim sojusznikom 30% many." },
            { "Streaming Heart", "Przywraca wszystkim sojusznikom do 50 HP." },
            { "Unhappy Refrain", "Nakłada na jednego przeciwnika 'Sorrow' i 'Stun', zmniejszając szansze na unik i ogłuszając wroga." },
            { "Levan Polkka", "Pora na rzut." },
            { "Defend The Queen", "Nakłada na siebie 'Provocation'." }
        };
        actionMap.TryGetValue(skillName, out skillDesc);
    }
}
[Serializable]
public class EffectDetails
{
    public string effectName { get; set; }
    public string effectDesc;
    public int effectDuration { get; set; }
    public int effectValue { get; set; }
    public int effectValueRange { get; set; }
    public CharactersParameters effectParent { get; set; }
    public string effectFramePath { get; set; }
    public string effectBackgroundPath { get; set; }
    public GameObject effectObject;

    public EffectDetails(string name, int duration, string frame, string background, CharactersParameters parent,int value=0, int valueRange=0)
    {
        effectName = name;
        effectParent = parent;
        effectDuration = duration;
        effectFramePath = frame;
        effectBackgroundPath = background;
        effectValue = value;
        effectValueRange = valueRange;
        Dictionary<string, string> actionMap = new()
        {
            { "Stun", "Pominięcie jednej tury." },
            { "Shield", "Broni od maksymalnie 90 obrażeń."},
            { "Shattered", "Ataki mają zmniejszoną szansę na trafienie."},
            { "Slowed Down","Zmniejsza prędkość."},
            { "Cold","Ataki mają zmniejszone obrażenia."},
            { "Unfreeze", "Przed każdą turą, mana jest regenrowana."},
            { "Repeat", "Posiadający bedzie miał dwie tury zamiast jednej."},
            { "Sped Up", "Zwiększona prędkość."},
            { "Targeted","Postać otrzymuje więcej obrażeń."},
            { "Contra", "Po zostaniu zaatakowanym, przeciwnik otrzymuje 'Bleed'." },
            { "Provocation","Prowokuje przeciwników do atakowania postaci z tym efektem. Otrzymywane obrażenia są zmniejszane o 35%."},
            { "Bleed","Otrymuje dodatkowe obrażenia na końcu tury."},
            { "Respiration","Otrzymuje dodatkowe zdrowie na końcu tury."},
            { "Sorrow", "Zmniejsza szansę na unik."},
            { "Cheered" , "Zwiększa zadawane obrażenia oraz zwiększa szansę na krytyczne obrażenia."},
            { "Braindead","Niweluje szansę na unik i szansę na obrażenia krytyczne."},
            { "Plot Armor","Mrożon nie może zginąć, dopóki żyją jego sojusznicy."},
            { "Game's Begun", "Załadowany rewolwer z jednym nabojem. Z każdym klikiem zwiększa się szansa na wystrzał. Kogo trafi?" }
        };
        actionMap.TryGetValue(effectName, out effectDesc);
    }
}

