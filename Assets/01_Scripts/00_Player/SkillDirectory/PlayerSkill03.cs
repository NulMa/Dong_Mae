using UnityEngine;

public class PlayerSkill03 : ISkillScript
{
    Playrer player;
    Rigidbody2D rigid;
    public string skillName = "OraRush";
    SkillSet skillSet;
    SkillKeybinding keyBind;


    public enumSkillType Type = enumSkillType.Active;

    public PlayerSkill03Projectile Punch;
    float curTime;
    float duration;
    bool ActiveSkill;

    float mp = 4f;

    public PlayerSkill03(Playrer _player, PlayerSkill03Projectile _prefab, SkillKeybinding keyBind)
    {
        Init(_player);
        Punch = _prefab;
        duration = 0.15f;
        this.keyBind = keyBind;
    }

    public void Process()
    {
        curTime -= Time.fixedDeltaTime;
        duration -= Time.fixedDeltaTime;
        if (curTime < 0f)
        {
            ActiveSkill = false;
            curTime = 0f;
        }
        PlayerSkillManager.Instance.SkillCoolTimeUpdate(keyBind, curTime, 0.12f);

        if (duration < 0f)
        {
            duration = 0f;
        }

    }

    public void Execute()
    {
        if (curTime > 0f) return;
        if (PlayerManager.Instance.CurMP < mp) return;

        PlayerManager.Instance.CurMP -= mp;

        curTime = 0.12f;
        duration = 0.5f;
        ActiveSkill = true;
        PlayerSkill03Projectile fp = Object.Instantiate(Punch);
        fp.transform.position = Playrer.Instance.transform.position + Vector3.up;
        fp.GetComponent<SpriteRenderer>().flipX = Playrer.Instance.sprite.flipX;
        fp.fire = Playrer.Instance.sprite.flipX;
    }

    public void Init(Playrer _player)
    {
        rigid = _player.GetComponent<Rigidbody2D>();

    }

    public void Release()
    {

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