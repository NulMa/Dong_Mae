using UnityEngine;

public class PlayerSkill01Projectile : ProjectileScript
{
    Animator animCtrl;
    Animation animClip;
    bool collActive;
    Collider2D coll;
    Rigidbody2D rigid;
    float lifecycle = 2f;

    private void Start()
    {
        animCtrl = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        rigid = GetComponent<Rigidbody2D>();

    }

    private void Update()
    {
        lifecycle -= Time.deltaTime;

        if (lifecycle < 1.5f)
        {
            collActive = true;
            coll.enabled = true;
        }

        if (lifecycle < 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            if (AttackCheck == false)
            {
                collision.GetComponent<Enemy>().Mon_CurHp -= 1;
                collision.GetComponent<Enemy>().StartCoroutine(collision.GetComponent<Enemy>().enemyHit(GetComponentInParent<Transform>().transform, 3f));
                AttackCheck = false;
            }

        }
    }
}