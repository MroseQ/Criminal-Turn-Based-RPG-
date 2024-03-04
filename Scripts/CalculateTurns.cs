using System;
            else
            {
                ReceiveEffect(effectToReceive, true);
                effectToReceive = new("Plot Armor", 999, "effblue", "EffectsTexture/Plot Armor Texture", boss);
                GetComponent<StartGameAudio>().sourceSwitch = true;
            }
            if (changeStateAt == 0)
            {
                foreach (CharactersParameters param in charParameters.FindAll(c => c.IsDead && c.enemy))
                {
                    param.currentSpeed = turnAllocation[0].speed + param.speed;
                    StartCoroutine(SetCharacter(param, turnAllocation[0].speed));
                }
            }
                t.SetIdentifier(indexToSet);
                indexToSet++;
                MessageToFill.Invoke(t);
            }
    {
        yield return new WaitWhile(() => blackScreen.activeSelf);
        foreach (CharactersParameters c in charParameters.FindAll(c => c.IsDead))
        {
            c.characterObject.SetActive(false);
        }
        turnAllocation = turnAllocation.OrderBy(c => c.speed).ToList();
        StartCoroutine(MakeTurnOf(turnAllocation[0].name));
    }
        {
            yield break;
        }
            {
                StartCoroutine(RemoveEffect(eff, attacking));
            }
        {
            if (attacking.effects.Exists(effect => effect.effectName == "Repeat" && effect.effectValue == 0))
        }
        else
        {
            changeTurn = true;
        }
            {
                chance = 0;
            }
            {
                chance = 0;
            }
        {
            target.characterObject.GetComponent<Animator>().SetTrigger("Hit");
            if (target.currentHP - damage <= 0)
            {
                if (target.effects.Exists(effect => effect.effectName == "Plot Armor"))
                {
                    if (attacking != target)
                    {
                        PlayOneAudioFromPath(characterAudio, "Audio/" + target.name + "/Hit", target.volumeScale);
                    }
                    target.currentHP = 1;
                }
                else
                {
                    characterAudio.SetNewClip(Resources.Load<AudioClip>("Audio/" + target.name + "/Death"), target.volumeScale);
                    RemoveCharacter(target);
                    if (charParameters.FindAll(t => !t.IsDead && t.enemy).Count == 1)
                    {
                        StartCoroutine(RemoveEffect(boss.effects.Find(e => e.effectName == "Plot Armor"), boss));
                    }
                    List<CharactersParameters> newtarget = LoadTargets("allEnemies");
                    if (newtarget.Count != 0)
                    {
                        targets = new() { newtarget[UnityEngine.Random.Range(0, newtarget.Count)] };
                    }
                    else
                    {
                        // finish game flag
                    }
                }
            }
            else
            {
                if (attacking != target)
                {
                    PlayOneAudioFromPath(characterAudio, "Audio/" + target.name + "/Hit", target.volumeScale);
                }
                target.currentHP -= damage;
            }
            if (isCrit)
            {
                CreateText("DamageNumbersCrit", target.parentTransform, damage.ToString(), color);
                characterAudio.SetNewClip(Resources.Load<AudioClip>("Audio/" + attacking.name + "/Crit"), attacking.volumeScale);
            }
            else
            {
                CreateText("DamageNumbers", target.parentTransform, damage.ToString(), color);
            }
                && attacking.effects.FindAll(effect => effect.effectName == "Bleed").Count < 3)
            {
                ReceiveEffect(new("Bleed", 2, "efforange", "EffectsTexture/Bleed Texture", attacking, 8, 2), false);
            }
            {
                GetComponent<OutlineHandler>().confirmed
            };
        {
            turn.speed -= speedDiff;
        }
        {
            foreach (AudioSource source in sources)
            {
                if(source.volume - Time.deltaTime/1.5f > 0)
                {
                    source.volume -= Time.deltaTime/1.5f;
                }
                else
                {
                    source.volume = 0;
                }
            }
            Color c = fader.GetComponent<Image>().color;
            c.a += Time.deltaTime/3f;
            fader.GetComponent<Image>().color = c;
            yield return new WaitForEndOfFrame();
        }
        SceneManager.LoadScene("EndGame");