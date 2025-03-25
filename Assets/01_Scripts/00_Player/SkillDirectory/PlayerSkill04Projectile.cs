using System.Collections;
using UnityEngine;


public class PlayerSkill04Projectile : ProjectileScript
{
    public Transform trs;
    public Collider2D coll;

    public Transform PlayerTrs;
    public bool fire;
    public Vector3 direction;
    float destroyTime;
    float throwingTime;

    private void Start()
    {
        destroyTime = 3f;
        trs = GetComponent<Transform>();
        coll = GetComponent<Collider2D>();
        direction = (Playrer.Instance.sprite.flipX ? Vector3.left : Vector3.right);
    }

    private void Update()
    {
        if (fire == false) return;

        if (fire == true)
        {
            destroyTime -= Time.deltaTime;
            throwingTime += Time.deltaTime * 2.5f;
            if (destroyTime < 0f) Destroy(gameObject);
        }

        transform.position += direction * throwingTime;


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