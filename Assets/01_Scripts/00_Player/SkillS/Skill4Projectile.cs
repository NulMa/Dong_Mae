using System.Collections;
using UnityEngine;


public class Skill4Projectiles : MonoBehaviour
{
    public Transform trs;
    public Collider2D coll;

    public Transform PlayerTrs;
    public bool fire;
    public Vector3 direction;
    float destroyTime;
    float throwingTime;

    private void Start()
    {
        destroyTime = 3f;
        trs = GetComponent<Transform>();
        coll = GetComponent<Collider2D>();
        direction = (PlayerScript.Instance.playerDirection == true ? Vector3.right : Vector3.left);
    }

    private void Update()
    {
        if (fire == false) return;

        if (fire == true)
        {
            destroyTime -= Time.deltaTime;
            throwingTime += Time.deltaTime * 2.5f;
            if (destroyTime < 0f) Destroy(gameObject);
        }

        transform.position += direction * throwingTime;


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }
}