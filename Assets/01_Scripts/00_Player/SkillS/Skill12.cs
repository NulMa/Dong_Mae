using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


// 01_Fist Skill
public class Skill12 : SkillScript, IBindSkill
{
    public PlayerScript player;
    public string skillName = "StoneFire";
    public enumSkillType Type = enumSkillType.Stock;
    public Skill12Projectiles ProjectilePrefab;
    public Queue<Skill12Projectiles> Projectiles;

    bool FireReady;
    float ReadyDelay;
    bool ReadyToFire;
    bool shootReady;

    float coolTime;

    private void Start()
    {
        coolTime = 0f;
        Projectiles = new Queue<Skill12Projectiles>();
    }

    private void FixedUpdate()
    {
        coolTime -= Time.fixedDeltaTime;

        if (FireReady == true)
        {
            ReadyDelay -= Time.fixedDeltaTime;
            if (ReadyDelay < 0f)
            {
                ReadyDelay = 0f;
            }
        }

        if (FireReady == true && ReadyDelay == 0f)
        {
            FireReady = false;
            shootReady = true;
        }

    }


    public void Started()
    {
        if (coolTime > 0f) return;

        if (FireReady == false && shootReady == false)
        {
            ReadyDelay = 0.15f;
            FireReady = true;
            Queue<Skill12Projectiles> que = new Queue<Skill12Projectiles>();


            Skill12Projectiles go = Instantiate<Skill12Projectiles>(ProjectilePrefab, PlayerScript.Instance.transform);
            go.transform.position = PlayerScript.Instance.transform.position;
            que.Enqueue(go);
            StartCoroutine(ProjectileReady(que));
        }
    }

    public void Performed(float _hold)
    {

    }
    public void Canceled()
    {
    }


    public IEnumerator ProjectileReady(Queue<Skill12Projectiles> que)
    {
        float angle = 0f;
        Skill12Projectiles go = que.Dequeue();
        go.PlayerTrs = PlayerScript.Instance.transform;
        go.angle = angle + 10f ;
        go.Ready = true;
        Projectiles.Enqueue(go);

        yield return new WaitForSecondsRealtime(0.15f);
        FireReady = false;
        shootReady = false;
        coolTime = 2.5f;
        StartCoroutine(ProjectileFire());
    }

    public IEnumerator ProjectileFire()
    {
        Skill12Projectiles go = Projectiles.Dequeue();
        Vector2 playerHolder = PlayerScript.Instance.playerDirection ? Vector2.right * 30f : Vector2.left * 30f;
        Vector2 randHolder = new Vector2(0f, Random.Range(-0.5f, 1.5f));
        go.GetComponent<Rigidbody2D>().AddForce(playerHolder + randHolder, ForceMode2D.Impulse);
        go.Ready = false;
        go.Fired = true;
        go.transform.SetParent(null);
        yield return null;
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
