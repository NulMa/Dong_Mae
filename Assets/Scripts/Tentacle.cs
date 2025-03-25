using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    public float damage;
    public void deleteTent() {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            collision.GetComponent<Playrer>().curHp -= damage;
            collision.GetComponent<Playrer>().StartCoroutine(collision.GetComponent<Playrer>().playerKnock(GetComponentInParent<Transform>().transform, 3f));
        }
    }
}
