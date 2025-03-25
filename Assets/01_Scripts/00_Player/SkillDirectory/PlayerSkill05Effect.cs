using System.Collections;
using UnityEngine;


public class PlayerSkill05Effect: ProjectileScript
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


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            if (AttackCheck == false)
            {
                collision.GetComponent<Enemy>().Mon_CurHp -= 1;
                collision.GetComponent<Enemy>().StartCoroutine(collision.GetComponent<Enemy>().enemyHit(transform, 3f));
                AttackCheck = false;
            }

        }
    }
}