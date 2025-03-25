using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BigMouthEntrance : MonoBehaviour
{
    public Transform teleport;
    public Image FIFO;
    public bool isProduce;

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.transform.CompareTag("Player") && collision.GetComponent<Playrer>().onInteract) {
            if (isProduce)
                return;
            StartCoroutine(fadeinAndOut(collision));
        }
    }

    IEnumerator fadeinAndOut(Collider2D collision) {
        isProduce = true;

        for (int i = 0; i < 101; i++) {
            FIFO.color = new Color(0.01f, 0.01f, 0.05f, (i * 0.01f));
            yield return new WaitForSeconds(0.005f);
        }

        yield return new WaitForSeconds(0.5f);
        collision.transform.position = teleport.position;

        for (int i = 0; i < 101; i++) {
            FIFO.color = new Color(0.01f, 0.01f, 0.05f, 1 - (i * 0.01f));
            yield return new WaitForSeconds(0.01f);
        }

        isProduce = false;
    }
}
