using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAtkSystem : MonoBehaviour
{
    public Vector2 boxsize;
    public Vector2 offset;
    public float damage;
    BoxCollider2D coll;

    private void Awake() {
        coll = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player") {
            collision.GetComponent<Playrer>().curHp -= damage;
            collision.GetComponent<Playrer>().StartCoroutine(collision.GetComponent<Playrer>().playerKnock(GetComponentInParent<Transform>().transform, 3f));
        }
    }

    public void atkOn() {
        coll.size = boxsize;
        coll.offset = offset;
    }
    
    public void atkOff() {
        coll.size = Vector2.zero;
    }



}
