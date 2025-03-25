using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


// 01_Fist Skill
public class Skill9 : SkillScript, IBindSkill
{
    public PlayerScript player;
    public string skillName = "HEAL";
    public enumSkillType Type = enumSkillType.Active;
    public Skill9_Effect ProjectilePrefab;

    bool FireReady;
    float ReadyDelay;
    bool ReadyToFire;
    bool shootReady;

    float coolTime;

    private void Start()
    {
        coolTime = 0f;
    }

    private void FixedUpdate()
    {
        coolTime -= Time.fixedDeltaTime;

    }


    public void Started()
    {
        if (coolTime > 0f) return;

        coolTime = 3f;
        Instantiate<Skill9_Effect>(ProjectilePrefab, PlayerScript.Instance.transform);
    }

    public void Performed(float _hold)
    {

    }
    public void Canceled()
    {
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
