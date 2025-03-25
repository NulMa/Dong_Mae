using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodTrap : MonoBehaviour
{
    BoxCollider2D coll;


    // Start is called before the first frame update
    private void Awake() {
        coll = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            Playrer player;
            player = collision.GetComponent<Playrer>();
            Debug.Log("Player Damaged");
            player.StartCoroutine(player.playerKnock(transform, 10));
        }
    }

    public void trapOn() {
        coll.size = new Vector2(1.5f, 3);
    }

    public void trapOff() {
        coll.size = new Vector2(0, 0);

    }
}
