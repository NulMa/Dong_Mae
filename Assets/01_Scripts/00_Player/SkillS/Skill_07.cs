using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 01_Fist Skill
public class Skill_07 : SkillScript, IBindSkill
{
    public PlayerScript player;
    public string skillName = "Heal";
    public enumSkillType Type = enumSkillType.Casting;
    public GameObject HealEffectPrefab;
    float chargingTime = 2f;
    float curChargingTime;
    float curTime;
    int numbPunchs;
    bool ActiveSkill;

    private void Start()
    {
        curChargingTime = 0f;
    }

    private void FixedUpdate()
    {
        curTime -= Time.fixedDeltaTime;
        if (curTime < 0f)
        {
            curTime = 0f;
        }

        if (ActiveSkill)
        {
            Debug.Log(curChargingTime);
            curChargingTime += Time.fixedDeltaTime;
        }

    }


    public void Started()
    {
        if (curTime > 0f) return;

        ActiveSkill = true;
        PlayerScript.Instance.PlayerAttackCheck = true;
        curChargingTime = 0f;



    }

    public void Performed(float _hold)
    {
        if (curTime > 0f) return;



    }
    public void Canceled()
    {
        if (curTime > 0f) return;



        PlayerScript.Instance.PlayerInvincible = false;

        if (curChargingTime < 0.15f)
        {
            numbPunchs = 1;
        }
        else if (curChargingTime < 0.5f)
        {
            numbPunchs = 2;
        }
        else if (curChargingTime < 1f)
        {
            numbPunchs = 3;
        }
        else if (curChargingTime < 1.5f)
        {
            numbPunchs = 4;
        }
        else if (curChargingTime > 2f)
        {
            numbPunchs = 5;
        }

        StartCoroutine(nanta(numbPunchs));
        curChargingTime = 0f;
    }

    IEnumerator nanta(int numb = 1)
    {

        int curCount = 0;
        while (curCount < numb)
        {
            // Instantiate(PunchEffectPrefab, PlayerScript.Instance.transform);
            yield return new WaitForSeconds(0.15f);
            // Instantiate(PunchEffectPrefab, PlayerScript.Instance.transform);
            yield return new WaitForSeconds(0.15f);
            curCount++;
        }

        ActiveSkill = false;
        PlayerScript.Instance.PlayerAttackCheck = false;
        curTime = 1f;
    }


    public override void Equip(PlayerScript.PlayerActionBind _bind)
    {
        CurrentBind = _bind;
        PlayerScript.Instance.StartActions[_bind.ToString()] = Started;
        PlayerScript.Instance.PerformActions[_bind.ToString()] = Performed;
        PlayerScript.Instance.CancelActions[_bind.ToString()] = Canceled;

        // Player / Action = Skill Action
    }

    public override void UnEquip()
    {
        PlayerScript.Instance.StartActions[CurrentBind.ToString()] -= Started;
        PlayerScript.Instance.PerformActions[CurrentBind.ToString()] -= Performed;
        PlayerScript.Instance.CancelActions[CurrentBind.ToString()] -= Canceled;
    }
}
