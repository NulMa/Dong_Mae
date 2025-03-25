using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.PlayerSettings;

public class Breakable : MonoBehaviour{

    public enum BreakType { ground, wall }
    public BreakType breakType;


    public AtkType atkType;

    public Transform pos;
    public Vector2 boxSize;
    public GameObject remove;


    // Update is called once per frame
    void FixedUpdate(){

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
        foreach (Collider2D collider in collider2Ds) {
            if (collider.tag == "Player") {


                switch (breakType) {
                    case BreakType.ground:
                        //play sfx
                        //delay
                        remove.gameObject.SetActive(false);
                        break;

                    case BreakType.wall:
                        if (collider.GetComponent<Playrer>().isAtk) {
                            if (collider.GetComponent<Playrer>().atkType.ToString() == atkType.ToString()) {
                                //play sfx
                                //delay
                                remove.gameObject.SetActive(false);
                            }
                            else {
                                //play sfx
                                Debug.Log("diff");
                            }
                        }
                        break;

                }
                
            }
        }


    }
}
