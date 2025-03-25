using System.Collections;
using UnityEngine;


public class Skill12Projectiles : MonoBehaviour
{
    public Transform trs;
    public Rigidbody2D rigid;
    public Collider2D coll;

    public Transform PlayerTrs;
    public bool Ready;
    public bool Fired;
    public float angle;
    public float destroyTime = 3f;


    private void Start()
    {
        trs = GetComponent<Transform>();
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (Ready == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, PlayerTrs.position + new Vector3(Mathf.Sin(angle), Mathf.Cos(angle) + 0.35f, 0f), Time.deltaTime * 30f);
        }

        if (Fired == true)
        { 
            destroyTime -= Time.deltaTime;
            if (destroyTime < 0f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy") && Ready == false)
        {
            Destroy(gameObject);
        }
    }
}