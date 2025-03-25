using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{ 
    Melee,
    Range,
}

public class Enemy_Test : MonoBehaviour
{
    public EnemyType enemyType;
    Rigidbody2D rigid;

    public bool invincible;
    public float invincibleTime;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        invincibleTerm();
    }

    public void invincibleTerm()
    {
        if (invincible == false) return;

        invincibleTime -= Time.fixedDeltaTime;
        if (invincibleTime < 0f)
        {
            invincibleTime = 0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("PlayerAttack") && invincible == false)
        {
            float contactX = collision.collider.transform.position.x;
            if (contactX - transform.position.x > 0)
            {
                //right
                rigid.AddForce(Vector2.right * 0.3f + Vector2.up * 0.2f);

            }
            else if (contactX - transform.position.x < 0)
            {
                rigid.AddForce(Vector2.left * 0.3f + Vector2.up * 0.2f);
            }
            else
            {
                rigid.AddForce(Vector2.up * 0.3f);
            }

            invincible = true;
            invincibleTime = 0.3f;
        }
    }
}
