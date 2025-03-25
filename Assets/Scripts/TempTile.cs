using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempTile : MonoBehaviour{

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.transform.tag == "Player") {
            Debug.Log(this.transform.position);
            StartCoroutine(TileReposition());
        }
    }

    IEnumerator TileReposition() {
            yield return new WaitForSeconds(1f);
            gameObject.transform.localScale = Vector3.zero;        

            yield return new WaitForSeconds(1f);
            gameObject.transform.localScale = Vector3.one;

    }




}
