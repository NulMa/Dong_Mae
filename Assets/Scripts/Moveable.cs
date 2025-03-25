using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : MonoBehaviour{

    public Vector3 initialPos;
    Rigidbody2D rigid;

    private void Awake() {
        initialPos = transform.position;
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "MoveableLimit") {
            backup_GameManager.instance.player.GetComponent<Playrer>().moveable = null;
            rigid.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
            transform.position = initialPos;
        }
    }





}
