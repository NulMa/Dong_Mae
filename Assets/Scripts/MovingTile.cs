    using System.Collections;
    using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
    using System.Text;
    using UnityEngine;

    public class MovingTile : MonoBehaviour{
    [System.Serializable]
    public class AllocatedEntryType {
        public List<Vector3> Path;
        public Transform transform;
    }
    public AllocatedEntryType AllocatedEntry;

    public Transform playerPar;

    public float speed;
    public bool isMove;

    Rigidbody2D rigid;

    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        AllocatedEntry.Path[0] = this.transform.position;
        for (int i = 1; i < AllocatedEntry.Path.Count; i++) {
            if (i == 0) {
                AllocatedEntry.Path[i] = this.transform.position; // Set the first element
            }
            else {
                AllocatedEntry.Path[i] += this.transform.position; // Add transform position to others
            }
        }

        StartCoroutine(EntryCoroutine());
    }

    
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.tag == "Player") {
            collision.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.transform.tag == "Player") {
            collision.transform.GetComponent<Playrer>().resetParent();
        }
    }


    public IEnumerator EntryCoroutine() {
        Queue<Vector2> queVector = new Queue<Vector2>();

        for (int idx = 0; idx < AllocatedEntry.Path.Count; idx++) {
            queVector.Enqueue(AllocatedEntry.Path[idx]);
        }

        queVector.Enqueue(AllocatedEntry.transform.position);
        Vector2 Road = queVector.Dequeue();
        bool flag = false;
        float sightX = transform.position.x;

        while (flag == false) {
            transform.position = Vector2.MoveTowards(transform.position, Road, Time.deltaTime * speed);
            //rigid.AddForce((Road - (Vector2)transform.position).normalized * speed, ForceMode2D.Force);

            yield return null;
            sightX = transform.position.x;
            if (Vector2.Distance((Vector2)transform.position, Road) < 0.005f) {
                if (queVector.Count == 0) {
                    flag = true;
                }
                else {
                    Road = queVector.Dequeue();
                }
            }
        }
        //loop
        StartCoroutine(EntryCoroutine());
    }
}
