using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 01_Fist Skill
public class Skill5 : SkillScript, IBindSkill
{
    public PlayerScript player;
    public string skillName = "Dash";
    public enumSkillType Type = enumSkillType.Active;
    public GameObject DashEffectPrefab;
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
        else
        {
            if (PlayerScript.Instance.inputVec == Vector2.zero && ActiveSkill == true)
            {
                Vector3 inputVector = new Vector3(PlayerScript.Instance.playerDirection == true ? 1f : -1f, 0f, 0f);
                PlayerScript.Instance.GetComponent<Rigidbody2D>().velocity = inputVector * 20f;
            }
            else if (ActiveSkill == true)
            {
                Vector3 inputVector = new Vector3(PlayerScript.Instance.inputVec.x, 0f, 0f);
                PlayerScript.Instance.GetComponent<Rigidbody2D>().velocity = inputVector * 20f;
            }

        }

    }


    public void Started()
    {
        if (curTime > 0f) return;
        curTime = 1f;
        duration = 0.3f;
        ActiveSkill = true;
        // Instantiate(DashEffectPrefab, PlayerScript.Instance.transform);
        PlayerScript.Instance.PlayerAttackCheck = true;
        PlayerScript.Instance.PlayerInvincible = true;






    }

    public void Performed(float _hold)
    {


    }
    public void Canceled()
    {
        PlayerScript.Instance.PlayerInvincible = false;
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
