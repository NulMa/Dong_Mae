using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ropes : MonoBehaviour{

    public GameObject ropePrefab;
    public int ropeCnt;
    public GameObject[] ropeIndex;
    public float margin;
    public Rigidbody2D pointRig;
    public Sprite RopeTip;

    public Vector3 pos;

    FixedJoint2D exJoint;

    void Start(){
        ropeIndex = new GameObject[ropeCnt + 1];

        for (int i = 0; i < ropeCnt; i++) {
            FixedJoint2D currentJoint = Instantiate(ropePrefab, transform).GetComponent<FixedJoint2D>();
            currentJoint.transform.localPosition = new Vector3(0, (i + 1) * -margin, 0);
            ropeIndex[i] = currentJoint.gameObject;

            if (i == 0) //first
                currentJoint.connectedBody = pointRig;
            else
                currentJoint.connectedBody = exJoint.GetComponent<Rigidbody2D>();

            exJoint = currentJoint;

            if(i == ropeCnt - 1) {
                currentJoint.GetComponent<Rigidbody2D>().mass = 20;
                currentJoint.GetComponent<SpriteRenderer>().sprite = RopeTip;
                currentJoint.GetComponent<CapsuleCollider2D>().size = new Vector2(0.2f, 0.25f);
                currentJoint.GetComponent<CapsuleCollider2D>().offset = new Vector2(0, 0.1f);
            }
        }
        
    }
    private void FixedUpdate() {
        pos = this.transform.position;
    }

}
