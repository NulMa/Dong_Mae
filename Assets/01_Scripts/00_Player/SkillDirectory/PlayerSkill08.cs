using UnityEditor;
using UnityEngine;

public class PlayerSkill08 : ISkillScript
{
    Playrer player;
    Rigidbody2D rigid;
    public string skillName = "Stone Body";
    SkillSet skillSet;

    public enumSkillType Type = enumSkillType.Active;

    public PlayerSkill08Effect Effect;
    PlayerSkill08Effect nowEffect;

    public float CoolTime = 3f;
    public float CurCoolTime;
    public bool InSkill;
    public float DurationTime = 5f;
    public float CurDurationTime;

    float mp = 10f;
    SkillKeybinding keyBind;

    public PlayerSkill08(Playrer _player, PlayerSkill08Effect _prefab, SkillKeybinding keyBind)
    {
        Init(_player);
        Effect = _prefab;
        this.keyBind = keyBind;
    }

    ~PlayerSkill08()
    {
        if (nowEffect.gameObject.activeSelf)
        { 
            Object.DestroyImmediate(nowEffect.gameObject, true);
        }
    }

    public void Process()
    {
        if (InSkill)
        {
            CurDurationTime -= Time.deltaTime;

            if (CurDurationTime < 0f)
            {
                CurDurationTime = 0f;
            }

            // Effect.transform.position = PlayerScript.Instance.transform.position;
            // PlayerScript.Instance.PlayerInvincible = true;

            if (CurDurationTime == 0f)
            {
                // PlayerScript.Instance.PlayerInvincible = false;
                CurCoolTime = 3f;
                InSkill = false;
                if (Effect != null)
                {
                    Object.Destroy(nowEffect.gameObject);
                }

            }
        }


        if (!InSkill)
        {
            CurCoolTime -= Time.deltaTime;
            if (CurCoolTime < 0f)
            {
                CurCoolTime = 0f;
            }


        }

        PlayerSkillManager.Instance.SkillCoolTimeUpdate(keyBind, CurCoolTime, 3f);

    }

    public void Execute()
    {
        if (CurCoolTime > 0f) return;
        if (PlayerManager.Instance.CurMP < mp) return;

        nowEffect = Object.Instantiate<PlayerSkill08Effect>(Effect, Playrer.Instance.transform);
        nowEffect.transform.localPosition = Vector3.up * 2;
        InSkill = true;
        CurDurationTime = DurationTime;
    }

    public void Init(Playrer _player)
    {
        rigid = _player.GetComponent<Rigidbody2D>();

    }

    public void Release()
    {
        if (InSkill == false) return;
        Debug.Log("Cancel");
        InSkill = false;
        CurCoolTime = 3f;
        if (nowEffect != null)
        {
            Object.Destroy(nowEffect.gameObject);
        }



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