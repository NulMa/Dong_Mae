using System.Drawing.Printing;
using UnityEngine;

public class PlayerSkill07 : ISkillScript
{
    Playrer player;
    Rigidbody2D rigid;
    public string skillName = "Ildo";
    SkillSet skillSet;
    SkillKeybinding keyBind;
    
    public enumSkillType Type = enumSkillType.Active;


    public float FPconsume;

    public PlayerSkill07Effect EffectPrefab;
    float cooltime;

    bool skillActive;
    float duration;

    float sp = 25f;

    public PlayerSkill07(Playrer _player, PlayerSkill07Effect _prefab, SkillKeybinding keyBind)
    {
        Init(_player);
        EffectPrefab = _prefab;
        duration = 2f;
        cooltime = 5f;
        this.keyBind = keyBind;
    }

    public void Process()
    {
        cooltime -= Time.deltaTime;

        if (cooltime < 0f)
        {
            cooltime = 0f;
        }

        PlayerSkillManager.Instance.SkillCoolTimeUpdate(keyBind, cooltime, 3f);

    }

    public void Execute()
    {
        if (cooltime > 0f) return;
        if (PlayerManager.Instance.CurSP < sp) return;

        PlayerManager.Instance.CurSP -= sp;
        // PlayerManager.Instance.FP -= FPconsume;

        Playrer.Instance.anim.SetTrigger("Atk_00");
        Vector2 pvec = Playrer.Instance.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pvec + Vector2.up, ((Playrer.Instance.sprite.flipX) ? Vector2.left : Vector2.right), 10f, LayerMask.GetMask("Ground"));
        if (hit)
        {
            PlayerSkill07Effect ep = Object.Instantiate<PlayerSkill07Effect>(EffectPrefab);
            Playrer.Instance.transform.position = hit.point - ((Playrer.Instance.sprite.flipX) ? Vector2.left : Vector2.right) / 2f;
            ep.transform.position = (hit.point + pvec)/2f;
            ep.coll.size = new Vector2(Playrer.Instance.transform.position.x - pvec.x, ep.coll.size.y);
        }
        else
        {
            PlayerSkill07Effect ep = Object.Instantiate<PlayerSkill07Effect>(EffectPrefab);
            Playrer.Instance.transform.position = pvec + ((Playrer.Instance.sprite.flipX) ? Vector2.left : Vector2.right) * 10f;
            ep.transform.position = pvec + Vector2.up + ((Playrer.Instance.sprite.flipX) ? Vector2.left : Vector2.right) * 5f;
            ep.coll.size = new Vector2(10f, 1);
        }

        cooltime = 3f;
        // Playrer.Instance.PlayerInvincible = true;
    }

    public void Init(Playrer _player)
    {
        rigid = _player.GetComponent<Rigidbody2D>();

    }

    public void Release()
    {
        if (skillActive == false) return;


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