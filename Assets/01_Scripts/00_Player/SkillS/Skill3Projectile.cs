using System.Collections;
using UnityEngine;


public class Skill3Projectiles : MonoBehaviour
{
    public Transform trs;
    public Collider2D coll;

    public Transform PlayerTrs;
    public bool fire;
    public bool direction;
    float destroyTime;

    private void Start()
    {
        destroyTime = 0.3f;
        trs = GetComponent<Transform>();
        coll = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (fire == false) return;

        if (fire == true)
        {
            destroyTime -= Time.deltaTime;

            if(destroyTime < 0f) Destroy(gameObject);
        }

        transform.position += (PlayerScript.Instance.playerDirection == true ? Vector3.right : Vector3.left) * Time.deltaTime * 10f;
        

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }
}