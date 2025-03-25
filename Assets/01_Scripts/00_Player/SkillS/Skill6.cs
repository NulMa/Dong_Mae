using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 01_Fist Skill
public class Skill6 : SkillScript, IBindSkill
{
    public PlayerScript player;
    public string skillName = "Dash";
    public enumSkillType Type = enumSkillType.Active;
    public GameObject DashEffectPrefab;
    public float FPconsume;
    public float coolTime;

    private void Start()
    {
    }

    private void FixedUpdate()
    {
        coolTime -= Time.fixedDeltaTime;
        if (coolTime < 0f)
        {
            coolTime = 0f;
        }

    }


    public void Started()
    {
        if (coolTime > 0f) return;
        //if (PlayerManager.Instance.FP < FPconsume) return;

        //PlayerManager.Instance.FP -= FPconsume;

        Vector2 pvec = PlayerScript.Instance.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pvec, ((PlayerScript.Instance.playerDirection == true) ? Vector2.right : Vector2.left), 3f, LayerMask.GetMask("Ground"));
        if (hit)
        {
            PlayerScript.Instance.transform.position = hit.point - ((PlayerScript.Instance.playerDirection == true) ? Vector2.right : Vector2.left) / 2f;
        }
        else
        {
            PlayerScript.Instance.transform.position = pvec + ((PlayerScript.Instance.playerDirection == true) ? Vector2.right : Vector2.left) * 3f;
        }

        coolTime = 3f;
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
