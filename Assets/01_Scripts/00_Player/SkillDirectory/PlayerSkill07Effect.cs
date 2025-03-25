using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill07Effect : ProjectileScript
{
    public BoxCollider2D coll;
    float lifetime;
    [SerializeField] GameObject Effect; // particle
    List<GameObject> enemies;

    private void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        lifetime = 0.35f;
        Effect.gameObject.SetActive(false);
        enemies = new List<GameObject>();
        Effect.gameObject.SetActive(true);
    }

    private void FixedUpdate()
    {
        lifetime -= Time.fixedDeltaTime;

        if (lifetime < 0f)
        { 
            
            TriggerHit();
        }

    }

    private void TriggerHit()
    {
        if (enemies.Count == 0) return;

        for (int i = enemies.Count -1; i > -1; i--)
        {
            // enemies[i].hit
            enemies[i].GetComponent<Enemy>().Mon_CurHp -= 1;
            enemies[i].GetComponent<Enemy>().StartCoroutine(enemies[i].GetComponent<Enemy>().enemyHit(GetComponentInParent<Transform>().transform, 3f));
        }

        Destroy(gameObject);
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (enemies.Contains(collision.gameObject) == false)
            { 
                enemies.Add(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (enemies.Contains(collision.gameObject) == true)
            {
                enemies.Remove(collision.gameObject);
            }
        }
    }

}
