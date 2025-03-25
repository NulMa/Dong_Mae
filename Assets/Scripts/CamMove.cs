using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CamMove : MonoBehaviour
{
    public bool istrue;
    public bool cammove;

    public CamFollow cam;
    public GameObject player;
    public Image FIFO;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (istrue & !cammove) {
            cammove = true;
            StartCoroutine(fadeIn());
        }

        if (!istrue & cammove) {
            cammove = false;
            StartCoroutine(fadeOut());
        }
    }

    public void camOn() {

    }

    IEnumerator fadeIn() {
        for (int i = 0; i < 101; i++) {
            FIFO.color = new Color(0.01f, 0.01f, 0.05f, (i * 0.01f));
            yield return new WaitForSeconds(0.001f);
        }

        yield return new WaitForSeconds(0.25f);
        cam.player = this.gameObject;
        yield return new WaitForSeconds(0.25f);

        for (int i = 0; i < 101; i++) {
            FIFO.color = new Color(0.01f, 0.01f, 0.05f, 1 - (i * 0.01f));
            yield return new WaitForSeconds(0.001f);
        }
    }

    IEnumerator fadeOut() {
        for (int i = 0; i < 101; i++) {
            FIFO.color = new Color(0.01f, 0.01f, 0.05f, (i * 0.01f));
            yield return new WaitForSeconds(0.001f);
        }

        yield return new WaitForSeconds(0.25f);
        cam.player = player;
        yield return new WaitForSeconds(0.25f);

        for (int i = 0; i < 101; i++) {
            FIFO.color = new Color(0.01f, 0.01f, 0.05f, 1 - (i * 0.01f));
            yield return new WaitForSeconds(0.001f);
        }
    }
}
