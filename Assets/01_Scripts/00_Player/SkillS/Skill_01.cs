using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 01_Fist Skill
public class Skill_01: SkillScript, IBindSkill
{
    public PlayerScript player;
    public string skillName = "Fist";
    public enumSkillType Type = enumSkillType.Active;
    public GameObject FistPrefab;
    float curTime;
    float duration;
    bool ActiveSkill;

    private void Start()
    {
        duration = 0.15f;
    }

    private void FixedUpdate()
    {
        curTime -= Time.fixedDeltaTime;
        duration -= Time.fixedDeltaTime;
        if (curTime < 0f)
        {
            ActiveSkill = false;
            curTime = 0f;
        }

        if (duration < 0f)
        {
            if (PlayerScript.Instance.PlayerAttackCheck && ActiveSkill == true)
            {
                PlayerScript.Instance.PlayerAttackCheck = false;
            }
            duration = 0f;
        }
    }


    public void Started()
    {
        if (curTime > 0f) return;
        curTime = 1f;
        duration = 0.15f;
        ActiveSkill = true;
        Instantiate(FistPrefab, PlayerScript.Instance.transform);
        PlayerScript.Instance.PlayerAttackCheck = true;
        PlayerScript.Instance.GetComponent<Rigidbody2D>().AddForce(PlayerScript.Instance.playerDirection ? Vector2.right* 100f : Vector2.left* 100f, ForceMode2D.Impulse);
 
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
