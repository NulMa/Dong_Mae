using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScrolling : MonoBehaviour
{
    #region Inspector
    public enum BGType { stat, dyna, desig }
    public BGType bgType;

    public Renderer renderer;
    public Transform player;
    public float speed = 1f;
    public float input;
    public float offX;

    #endregion

    private void Awake() {
        renderer = this.gameObject.GetComponent<MeshRenderer>();
    }

    private void Update() {

        switch (bgType) {
            case BGType.stat:
                float move = speed * input * 0.01f;
                input = player.transform.position.x;
                
                renderer.material.mainTextureOffset = new Vector2(move, 0);
                break;

            case BGType.dyna:

                offX -=  Time.deltaTime;
                renderer.material.mainTextureOffset = new Vector2(offX * speed, 0);
                break;

            case BGType.desig:
                //transform.position = new Vector3(this.transform.position.x, 60 - ); 
                break;
        }
    }
    /*
    IEnumerator randY() {
        float offY = 0;
        offY = Random.Range(-1, 1);

        yield return new WaitForSeconds(2f);
        for(int i = 0; i < 10; i++) {
            
        }

    }
    */
}
