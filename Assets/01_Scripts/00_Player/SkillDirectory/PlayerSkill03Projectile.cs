using System.Collections;
using UnityEngine;


public class PlayerSkill03Projectile : ProjectileScript
{
    public Transform trs;
    public Collider2D coll;

    public Transform PlayerTrs;
    public bool fire;
    public bool direction;
    float destroyTime;

    private void Start()
    {
        destroyTime = 0.125f;
        trs = GetComponent<Transform>();
        coll = GetComponent<Collider2D>();
    }

    private void Update()
    {

        destroyTime -= Time.deltaTime;

        if (destroyTime < 0f) Destroy(gameObject);

        transform.position += (fire ? Vector3.left: Vector3.right) * Time.deltaTime * 100f;


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