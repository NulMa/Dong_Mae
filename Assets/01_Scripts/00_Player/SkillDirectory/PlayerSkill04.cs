using UnityEngine;
using UnityEngine.UIElements;

public class PlayerSkill04 : ISkillScript
{
    Playrer player;
    Rigidbody2D rigid;
    public string skillName = "Charge Attack";
    SkillSet skillSet;
    SkillKeybinding keyBind;

    public enumSkillType Type = enumSkillType.Active;

    public PlayerSkill04Projectile ProjectilePrefab;
    float curTime;
    float duration;
    bool ActiveSkill;

    Vector3 Charge0Scale = new Vector3(1.5f, 2f, 1f);
    Vector3 Charge1Scale = new Vector3(3f, 4f, 1f);
    Vector3 Charge2Scale = new Vector3(6f, 8f, 1f);

    float coolTime = 0f;
    bool charging;
    float chargingTime;

    float mp = 10f;

    GameObject ChargingEffect;
    GameObject ce;

    public PlayerSkill04(Playrer _player, PlayerSkill04Projectile _prefab, GameObject _chargingEffect, SkillKeybinding keyBind)
    {
        Init(_player);
        ProjectilePrefab = _prefab;
        duration = 0.15f;
        this.keyBind = keyBind;
        ChargingEffect = _chargingEffect;
    }

    public void Process()
    {
        coolTime -= Time.fixedDeltaTime;
        curTime -= Time.fixedDeltaTime;
        duration -= Time.fixedDeltaTime;
        if (charging)
        {
            chargingTime += Time.fixedDeltaTime;

            if (ce != null)
            {
                if (chargingTime < 1.5f)
                {
                    ce.transform.localScale = Vector3.one;
                }
                else if (chargingTime < 3f)
                {
                    ce.transform.localScale = Vector3.one * 2;
                }
                else if (chargingTime >= 3f)
                {
                    ce.transform.localScale = Vector3.one * 3;
                }
            }

        }


        if (curTime < 0f)
        {
            ActiveSkill = false;
            curTime = 0f;
        }

        PlayerSkillManager.Instance.SkillCoolTimeUpdate(keyBind, coolTime, 3f);

        if (duration < 0f)
        {
            duration = 0f;
        }

    }

    public void Execute()
    {
        if (coolTime > 0f) return;
        if (PlayerManager.Instance.CurMP < mp) return;

        PlayerManager.Instance.CurMP -= mp;

        charging = true;
        ce = Object.Instantiate<GameObject>(ChargingEffect, Playrer.Instance.transform);
        ce.transform.localPosition= Vector3.up;
    }

    public void Init(Playrer _player)
    {
        rigid = _player.GetComponent<Rigidbody2D>();

    }

    public void Release()
    {
        if (charging == false) return;

        Object.Destroy(ce);

        charging = false;
        coolTime = 3f;


        Vector3 scale;

        if (chargingTime < 1.5f)
        {
            PlayerSkill04Projectile pj = Object.Instantiate<PlayerSkill04Projectile>(ProjectilePrefab);
            pj.transform.SetParent(null);
            scale = Charge0Scale;
            scale.x = Playrer.Instance.sprite.flipX ? -1f : 1f;
            pj.transform.localScale = scale;
            pj.transform.position = Playrer.Instance.transform.position + Vector3.up;
            pj.fire = true;

        }
        else if (chargingTime < 3f)
        {
            PlayerSkill04Projectile pj = Object.Instantiate<PlayerSkill04Projectile>(ProjectilePrefab);
            pj.transform.SetParent(null);
            scale = Charge1Scale;
            scale.x = Playrer.Instance.sprite.flipX == true ? -1f : 1f;
            pj.transform.localScale = scale;
            pj.transform.position = Playrer.Instance.transform.position + Vector3.up;
            pj.fire = true;
        }
        else if (chargingTime >= 3f)
        {
            PlayerSkill04Projectile pj = Object.Instantiate<PlayerSkill04Projectile>(ProjectilePrefab);
            pj.transform.SetParent(null);
            scale = Charge2Scale;
            scale.x = Playrer.Instance.sprite.flipX ? -1f : 1f;
            pj.transform.localScale = scale;
            pj.transform.position = Playrer.Instance.transform.position + Vector3.up;
            pj.fire = true;
        }

        chargingTime = 0f;
    }

    public SkillSet GetBind()
    {
        return skillSet;
    }

    public void Init(Playrer playrer, SkillSet _set)
    {
        player = playrer;
        skillSet = _set;
    }
}