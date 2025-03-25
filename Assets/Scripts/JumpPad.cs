using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour{

    public float jumpPower;

    float rotZ;
    float rads;
    Vector2 dir;

    private void OnCollisionEnter2D(Collision2D collision) {
        rotZ = transform.eulerAngles.z;
        rads = rotZ * Mathf.Deg2Rad;
        dir = new Vector2(Mathf.Sin(-rads), Mathf.Cos(rads));
        Debug.Log(dir);

        if (collision.transform.tag == "Player") {
            collision.transform.GetComponent<Playrer>().isOnPad = true;
            collision.transform.GetComponent<Rigidbody2D>().AddForce(dir * jumpPower, ForceMode2D.Impulse);
        }
    }
}
