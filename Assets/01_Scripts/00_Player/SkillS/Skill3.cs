using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Skill3 : SkillScript, IBindSkill
{
    public PlayerScript player;
    public string skillName = "StoneFire";
    public enumSkillType Type = enumSkillType.Stock;
    public Skill3Projectiles ProjectilePrefab;


    float coolTime;
    bool combo;
    float comboDelay;
    float comboTime;

    private void Start()
    {

    }

    private void Update()
    {
        coolTime -= Time.deltaTime;
        comboTime -= Time.deltaTime;
        comboDelay -= Time.deltaTime;

        if(coolTime < 0f) coolTime = 0f;
        if (comboTime < 0f) comboTime = 0f;
        if (comboDelay < 0f) comboDelay = 0f;

        if (comboTime == 0f && combo == true)
        {
            coolTime = 3f;
            combo = false;
        }


    }



    public void Started()
    {
        if (coolTime > 0f) return;

        if (combo == false)
        {
            comboTime  = 4f;
            combo = true;
        }

        if (combo == true)
        {
            if (comboDelay > 0f) return;

            comboDelay = 0.15f;
            Skill3Projectiles go = Instantiate<Skill3Projectiles>(ProjectilePrefab);
            go.transform.position = PlayerScript.Instance.transform.position + (PlayerScript.Instance.playerDirection == true ? Vector3.right * 0.33f : Vector3.left * 0.33f);
            go.transform.position += new Vector3(0f, UnityEngine.Random.Range(-0.3f, 0.3f), 0f);
            go.fire = true;
            go.direction = PlayerScript.Instance.playerDirection;
            go.transform.localScale = (go.direction == true ? Vector3.one : new Vector3(-1f, 1f, 1f));
        }


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
