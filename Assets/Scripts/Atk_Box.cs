using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk_Box : MonoBehaviour{
    BoxCollider2D coll;

    public float Damage;

    private void Awake() {
        coll = GetComponent<BoxCollider2D>();
        gameObject.SetActive(false);
    }

    public void BoxSet(Vector2 size, Vector2 offset, float dmg) {
        coll.size = size;
        coll.offset = offset;
        Damage = dmg;

        gameObject.SetActive(true);
    }

    IEnumerator bossHitEff(Collider2D collision) {
        collision.transform.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 0.8f);
        yield return new WaitForSeconds(0.1f);
        collision.transform.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Enemy") {
            collision.GetComponent<Enemy>().Mon_CurHp -= Damage;
            collision.GetComponent<Enemy>().StartCoroutine(collision.GetComponent<Enemy>().enemyHit(GetComponentInParent<Transform>().transform, 3f));
        }

        //if (collision.GetComponent<Enemy>().mobType == Enemy.MobType.Boss)
        //    StartCoroutine(bossHitEff(collision));
    }
}
