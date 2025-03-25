using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FormActiveTemplate : FormSkill, IFormSkill
{
    float CoolTime;
    float CurTime;

    private void Start()
    {
        SkillType = enumSkillType.Active;
        CoolTime = SkillContainer.Skill_cooldown;
        CurTime = 0f;
    }

    private void FixedUpdate()
    {
        CurTime -= Time.fixedDeltaTime;
        if (CurTime < 0f) { CurTime = 0f; }
    }

    public void Started()
    {
        if (CurTime > 0f)
        {
            Debug.Log($"Cooltime Not Ready :: {CurTime}");
            return;
        } 

        CurTime = CoolTime;
        Debug.Log("Instant Skill Executed");
    }
    public void Performed(float _hold)
    {
        // Active Skill not use.
    }

    public void Canceled()
    {
    }




}