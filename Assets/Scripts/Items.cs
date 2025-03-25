using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour{
    public string itemName; //Name_Location_Number
    public int itemCode;

    public float moveRange;
    public float moveSpeed;

    public Transform player;


    private Vector3 startPosition;  

    void Start(){
        startPosition = transform.position;
        player.GetComponent<Playrer>().items[itemCode].itemName = itemName;
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if(collision.tag == "Player" && player.GetComponent<Playrer>().onInteract) {
            Debug.Log("Get : " + itemName);
            player.GetComponent<Playrer>().items[itemCode].isHave = true;
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    private void FixedUpdate() {
        float newY = Mathf.Lerp(startPosition.y - moveRange, startPosition.y + moveRange,Mathf.PingPong(Time.time * moveSpeed, 1));
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
