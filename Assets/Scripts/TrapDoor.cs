using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDoor : MonoBehaviour
{
    public Gate[] gates;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.transform.tag == "Player") {
            foreach (Gate gate in gates) {
                gate.isLock = true;
            }
        }
    }
}
