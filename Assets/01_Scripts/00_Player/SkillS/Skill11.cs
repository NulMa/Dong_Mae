using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


// 01_Fist Skill
public class Skill11 : SkillScript, IBindSkill
{
    public PlayerScript player;
    public string skillName = "StoneBody";
    public enumSkillType Type = enumSkillType.Charging;
    // public Skill9_Effect ProjectilePrefab;
    public GameObject Effect;

    public float CoolTime = 3f;
    public float CurCoolTime;
    public bool InSkill;
    public float DurationTime = 2f;
    public float CurDurationTime;


    private void Start()
    {

    }

    private void Update()
    {



        if (InSkill)
        {
            CurDurationTime -= Time.deltaTime;

            if (CurDurationTime < 0f)
            {
                CurDurationTime = 0f;
            }

            Effect.transform.position = PlayerScript.Instance.transform.position;
            PlayerScript.Instance.PlayerInvincible = true;

            if (CurDurationTime == 0f)
            {
                PlayerScript.Instance.PlayerInvincible = false;
                CurDurationTime = 0f;
                CurCoolTime = 3f;
                InSkill = false;
                Effect.SetActive(false);
            }
        }


        if (InSkill)
        {

        }
        else
        {
            CurCoolTime -= Time.deltaTime;
            if (CurCoolTime < 0f)
            {
                CurCoolTime = 0f;
            }


        }





    }


    public void Started()
    {
        if (CurCoolTime > 0f) return;

        Effect.SetActive(true);
        InSkill = true;
        CurDurationTime = DurationTime;


    }

    public void Performed(float _hold)
    {

    }
    public void Canceled()
    {
        if (InSkill == false) return;
        Debug.Log("Cancel");
        InSkill = false;
        DurationTime = 0f;
        CurCoolTime = 3f;
        Effect.SetActive(false);
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
