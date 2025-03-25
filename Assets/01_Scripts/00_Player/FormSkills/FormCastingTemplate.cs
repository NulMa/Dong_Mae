using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FormCastingTemplate : FormSkill, IFormSkill
{
    float CoolTime;
    float CurTime;

    float CastCoolTime;
    float CastCurTime;

    bool Casting;
    // Casting UI..

    private void Start()
    {
        SkillType = enumSkillType.Casting;
        CoolTime = SkillContainer.Skill_cooldown;
        CastCoolTime = SkillContainer.Skill_casttime;

        CurTime = 0f;
        CastCurTime = 0f;
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

        // CurTime = CoolTime;
        Casting = true;
        Debug.Log("Casting Skill Executed");
    }
    public void Performed(float _hold)
    {
        // Active Skill not use.
    }

    public void Canceled()
    {
    }




}