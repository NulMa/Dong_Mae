using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public enum GateType { key, lever, trap, other}
    public GateType gatetype;
    public bool isLock;
    public bool stopChecking;
    public int[] key_Item_Code;
    public GameObject Lock;
    public GameObject Lever;
    public GameObject keyMonster;
    public GameObject keyPlayer;

    public BoxCollider2D trap;

    Animator anim;
    Collider2D coll;
    private void Awake() {
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();


        switch (gatetype) {
            case GateType.trap:     // if use key, disable lever
                Lever.gameObject.SetActive(false);
                GetComponent<BoxCollider2D>().size = Vector2.zero;
                break;

            case GateType.key:      
            case GateType.other:
                Lever.gameObject.SetActive(false);
                break;

            case GateType.lever:    // if use lever, change interact position to lever's pos
                coll.offset = new Vector2(Lever.transform.localPosition.x, Lever.transform.localPosition.y);
                break;
        }
    }

    private void Update() {
        if (isLock && !Lock.gameObject.activeSelf) {
            anim.SetBool("isOpen", false);
            Lock.gameObject.SetActive(true);
        }

        if (!isLock) {
            anim.SetBool("isOpen", true);
            Lock.gameObject.SetActive(false);
        }
        

        if(gatetype == GateType.trap && !keyMonster.activeSelf) {
            trap.gameObject.SetActive(false);
            isLock = false;
        }
    }

    private void FixedUpdate() {
        CheckCollisionWithPlayer();
    }

    private void CheckCollisionWithPlayer() {
        if (trap == null)
            return;

        Collider2D[] hits = Physics2D.OverlapBoxAll(trap.bounds.center, trap.bounds.size, 0);

        foreach (var hit in hits) {
            if (hit.CompareTag("Player")) {
                isLock = true;

            }
        }
    }


    public void keyCheck() { // GateType.key
        int keyCheck = 0;
        foreach (int keyNum in key_Item_Code) {
            if(keyPlayer.GetComponent<Playrer>().items[keyNum].isHave == true)
                keyCheck++;
        }

        if(keyCheck == key_Item_Code.Length) {
            StartCoroutine(gateOpenProduce());
        }
            
    }

    IEnumerator gateOpenProduce() {
        keyPlayer.GetComponent<Playrer>().letterboxOn();
        yield return new WaitForSeconds(0.8f);
        isLock = false;
        yield return new WaitForSeconds(0.8f);
        keyPlayer.GetComponent<Playrer>().letterboxOff();
    }

    IEnumerator distancedLeverOpen() {
        this.gameObject.GetComponent<CamMove>().istrue = true;
        yield return new WaitForSeconds(1.5f);
        Lever.GetComponent<Animator>().SetTrigger("LeverActive");
        isLock = false;
        yield return new WaitForSeconds(1.5f);
        this.gameObject.GetComponent<CamMove>().istrue = false;

    }


    public void leverCheck() { // GateType.lever
        if (isLock) {
            if(this.gameObject.GetComponent<CamMove>() == null) {
                Lever.GetComponent<Animator>().SetTrigger("LeverActive");
                isLock = false;
            }
            else {
                StartCoroutine(distancedLeverOpen());
            }

        }
    }

    public void otherOpen() {
        isLock = false;

    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.transform.tag == "Player" && collision.GetComponent<Playrer>().onInteract /*backup_GameManager.instance.player.GetComponent<Playrer>().onInteract*/ && isLock) {

            switch (gatetype) {
                case GateType.key:
                    keyCheck();
                    break;

                case GateType.lever:
                    leverCheck();
                    break;
            }
        }
    }



    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.transform.tag == "Player") {
            switch (gatetype) {
                case GateType.trap:
                    isLock = true;
                    break;
            }
        }
    }


}
