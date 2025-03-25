using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
//using static UnityEditor.PlayerSettings;
//using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Enemy : MonoBehaviour{

    public enum AtkType { Agressive, Non_Agressive, Neutral }
    public enum MobType { Normal, Elite, Boss }


    [Header("Mon_Info")]
    public AtkType atkType;
    public MobType mobType;
    public Gate[] Connected_Gate;
    public int Mon_Index;
    public string Mon_name;

    [Header("Mon_Basic_Stat")] //Attack & Defence
    public float Mon_MaxHp;
    public float Mon_CurHp;
    //-----------------------------------------
    public float Mon_Atk;
    public float Mon_Matk;
    public float Mon_Def;
    public float Mon_Mdef;
    public float Mon_Spd;
    public float Mon_Min_Dis; //minimum distance

    [Header("Mon_Adv_Stat")]  //Elements_Resist
    public float Mon_FRes;
    public float Mon_WRes;
    public float Mon_TRes;
    public float Mon_MRes;
    public float Mon_RRes;

    [Header("Mon_Ext_Resources")] //External Resources
    public RuntimeAnimatorController Mon_Ani;

    [Header("Mon_AI_Needs")] //External Resources
    public Vector3 initialPos;
    public Transform Target;
    public Transform pos;
    public Transform atkBox;
    public Vector2 chaseRange;
    public bool playerDetected;
    public bool isBack;
    public bool isDead;
    public bool isAtk;
    public bool isKnock;
    public bool isBerserk;
    public float dir;
    public float domain; //if exit to domain area, get back to initial position
    public float backTime; //time check to force getback();
    public int atk_num;
    public float atkDelay;

    //boss
    public int atk3_max;
    public int atk3_cur;
    public float[] bossAtkDelays;
    public float delayLevel;

    public GameObject tentacle;

    
    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D polygonCollider;
    private Sprite previousSprite;



    #region Component Paramas
    Rigidbody2D rigid;
    Animator anim;
    Collider2D coll;
    #endregion



    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();

        anim.runtimeAnimatorController = Mon_Ani;
        StartCoroutine(ThinkNextDir());
        Mon_CurHp = Mon_MaxHp;
        initialPos = transform.position;
        atkOff();

        if(mobType == MobType.Boss) {
            spriteRenderer = GetComponent<SpriteRenderer>();
            polygonCollider = GetComponent<PolygonCollider2D>();
            UpdateColliderShape();
        }
    }

    void UpdateColliderShape() {
        //take cur sprite
        Sprite sprite = spriteRenderer.sprite;

        if (sprite == null)
            return;

        // update poly coll path
        polygonCollider.pathCount = sprite.GetPhysicsShapeCount();

        for (int i = 0; i < polygonCollider.pathCount; i++) {
            List<Vector2> path = new List<Vector2>();
            sprite.GetPhysicsShape(i, path);

            // cut off
            for (int j = 0; j < path.Count; j++) {
                if (path[j].y < -4) {   // cut off height
                    path[j] = new Vector2(path[j].x, -4);
                }
            }

            polygonCollider.SetPath(i, path.ToArray());
        }

        // update prev sprite
        previousSprite = sprite;
    }



    private void Update() {
        delayLevel = isBerserk == true ? 0.75f : 1;

        if((Mon_MaxHp/2) >= Mon_CurHp){
            isBerserk = true;
        } 
        if(mobType == MobType.Boss) {
            if (spriteRenderer.sprite != previousSprite) {
                UpdateColliderShape();
            }
        }


        if (Target != null) {
            if (atkDelay > 0)
                atkDelay -= Time.deltaTime;
            else
                atkDelay = 0;


            for (int i = 0; i < bossAtkDelays.Length; i++) {
                if (bossAtkDelays[i] > 0)
                    bossAtkDelays[i] -= Time.deltaTime;
                else
                    bossAtkDelays[i] = 0;
            }
        }
    }

    private void FixedUpdate() {



        if (isDead || isAtk)
            return;

        SpriteFlip();
        BackTo();

        DetectPlayer();
        DetectCliff();

        Move();
        enemyMove();
        AccSpd();
        ForceBack();

        if(Mon_CurHp <= 0) {
            deadAnim();
        }

    }

    


    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0.1f, 0.1f);
        Gizmos.DrawWireCube(pos.position, chaseRange);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
            if (collision.transform.tag == "Player") {
            collision.transform.GetComponent<Playrer>().curHp -= 3;
            collision.transform.GetComponent<Playrer>().StartCoroutine(collision.transform.GetComponent<Playrer>().playerKnock(GetComponentInParent<Transform>().transform, 3f));
        }
    }

    public void enemyMove() {
        if (dir != 0)
            anim.SetBool("isMove", true);
        else
            anim.SetBool("isMove", false);
    }

    public void deadAnim() {
        anim.SetTrigger("enemyDead");
        dir = 0;
        isDead = true;
    }
    public void Dead() {
        
        foreach(Gate gate in Connected_Gate) {
            if (Connected_Gate == null)
                return;

            gate.GetComponent<BoxCollider2D>().size = Vector2.zero;
            gate.isLock = false;
        }
        gameObject.SetActive(false);
    }

    public void BossAtkRand() {
        //공격 리스트
    }

    public void enemyAtk() {    //atk_00

        if (atkDelay > 0 || isKnock)
            return;


        isAtk = true;
        anim.SetTrigger("enemyAtk");
        float dis = transform.position.x - Target.position.x;
        transform.localScale = (dis > 0) ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
        //box enable, need delay?
        
        if(mobType == MobType.Normal) {
            atkBox.gameObject.SetActive(true);
        }
        

        //if in box, player damaged
        //disable box, isAtk
    }

    public void atkboxOn() {
        atkBox.gameObject.SetActive(true);
    }

    public void atkOff() {
        isAtk = false;
        if(mobType == MobType.Normal) {
            atkBox.gameObject.SetActive(false);
        }
        else {
            atkBox.GetComponent<BoxCollider2D>().size = Vector2.zero;
        }
        
        atkDelay = (mobType == MobType.Normal) ? 2f : 3f * delayLevel;  //if type is boss, its used by global cooltime
    }


    public void DetectPlayer() {
        if (isBack)
            return;

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, chaseRange, 0);
        if (!playerDetected)
            Target = null;

        playerDetected = false;

        foreach (Collider2D collider in collider2Ds) {
            float distance;
            if (collider.CompareTag("Player")) {
                playerDetected = true;
                Target = collider.transform;
                if (Vector2.Distance(transform.position, Target.position) < (distance = (mobType == MobType.Normal) ? 2.5f : 10f)) {
                    dir = 0;
                    if(mobType == MobType.Normal)
                        enemyAtk();
                    else {
                        bossAtkSelect();
                    }
                    break;
                }

                float dis = transform.position.x - Target.position.x;
                dir = (dis > 0) ? -1 : 1;


                break;
            }
        }
        if (!playerDetected) {
            Target = null;
        }
    }

    public void Move() {
        rigid.velocity = new Vector2(dir * Mon_Spd, rigid.velocity.y);
    }

    IEnumerator ThinkNextDir() { //change move direction with random time
        if (Target == null)
            dir = Random.Range(-1, 2);
        float time = Random.Range(1, 3);
        yield return new WaitForSeconds(time);
        StartCoroutine(ThinkNextDir());
    }

    public IEnumerator enemyHit(Transform push, float force) {
        if (mobType != MobType.Boss && !isAtk) {
            isKnock = true;
            atkOff();
            anim.SetTrigger("enemyHit");

            Vector2 knockbackDirection = (transform.position - push.transform.position).normalized;
            float dirX = knockbackDirection.x >= 0 ? 1 : -1;
            // 반대 방향으로 힘을 가함
            rigid.AddForce(Vector2.right * dirX * force * 20, ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.3f);
            isKnock = false;
        }   
    }
    public void DetectCliff() { //if detecte cliff, change move direcrtion
        if (mobType == MobType.Boss)
            return;

        Vector2 frontVec = new Vector2(rigid.position.x + dir * 0.5f, rigid.position.y);
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 2, LayerMask.GetMask("Ground"));
        RaycastHit2D rayHitWall = Physics2D.Raycast(frontVec, Vector3.right, 2, LayerMask.GetMask("Ground"));
        Debug.DrawRay(frontVec, Vector3.down * 2, Color.cyan);
        Debug.DrawRay(rigid.position, Vector3.right * dir, Color.green);
        if (rayHit.collider == null || rayHitWall.collider != null) {
            dir *= -1;
        }
    }

    public void SpriteFlip() {
        if(Target != null) {
            float dis = transform.position.x - Target.position.x;
            transform.localScale = (dis > 0) ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
            return;
        }

        transform.localScale = dir < 0 ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);   
    }

    public void AccSpd() { 
        if (playerDetected)
            Mon_Spd = 3;
        else {
            Mon_Spd = 2;
        }
    }

    public void BackTo() { //if distance with initial position is too far, get back
        if (Vector2.Distance(transform.position, initialPos) > domain) {
            isBack = true;
            dir = transform.position.x - initialPos.x > 0 ? -1 : 1;
            StartCoroutine(BackToTime());
        }

    }

    public void ForceBack() { //ignore chase and back to initial position
        if (isBack)
            backTime += Time.deltaTime;
        if(backTime > 5) {
            backTime = 0;
            isBack = false;
            transform.position = initialPos;
        }
    }

    IEnumerator BackToTime() {  //time to force back
        yield return new WaitForSeconds(2f);
        isBack = false;
    }

    void setAtkBox(Vector2 boxsize, Vector2 boxoffset) {
        atkBox.GetComponent<EnemyAtkSystem>().boxsize = boxsize;
        atkBox.GetComponent<EnemyAtkSystem>().offset = boxoffset;
    }

    //boss pat
    void bossAtkSelect() {
        if (isAtk == true)
            return;
        //random pat in array
        //do not insert delayed atks

        atk_num = Random.Range(0, 4);
        //atk_num = 3;
        
        if (bossAtkDelays[atk_num] > 0) { //if choose delayed atk, re-roll
            bossAtkSelect();
            return;
        }
        
        switch (atk_num) {
            case 0:
                enemyAtk();
                bossAtkDelays[atk_num] = 0; //set delay
                break;

            case 1:
                atk_01();
                bossAtkDelays[atk_num] = 5 * delayLevel;
                break;

            case 2:
                atk_02();
                bossAtkDelays[atk_num] = 10 * delayLevel;
                break;

            case 3:
                atk_03();
                bossAtkDelays[atk_num] = 20 * delayLevel;
                break;
        }

        
    }

    public void bossAtkOn() {
        atkBox.gameObject.GetComponent<EnemyAtkSystem>().atkOn();
    }
    public void bossAtkOff() {
        atkBox.GetComponent<EnemyAtkSystem>().atkOff();
    }

    void atk_01() {
        isAtk = true;
        anim.SetTrigger("Atk_01");
        float dis = transform.position.x - Target.position.x;
        transform.localScale = (dis > 0) ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
        setAtkBox(new Vector2(4, 5), new Vector2(1, 0));

        //box enable, need delay?
    }

    void atk_02() {
        isAtk = true;
        anim.SetTrigger("Atk_02");
        setAtkBox(new Vector2(4, 5), new Vector2(1, 0));
        StartCoroutine(atk_02_02());

    }

    IEnumerator atk_02_02() {
        float playerX = Target.position.x;
        yield return new WaitForSeconds(1f);
        for(int i = 0; i < 5; i++) {
            yield return new WaitForSeconds(0.4f * delayLevel);
            Instantiate(tentacle, new Vector2(playerX+i*2, -21), Quaternion.identity);
            Instantiate(tentacle, new Vector2(playerX-i*2, -21), Quaternion.identity);
        }
        isAtk = false;
    }

    void atk_03() {
        isAtk = true;
        setAtkBox(new Vector2(1,1), new Vector2(2,2));
        Physics2D.IgnoreLayerCollision(9, 11, true);

        anim.SetTrigger("Atk_03_1");

    }
    
    void atk_03_02() {
        if(atk3_cur < atk3_max) {
            StartCoroutine(atk_03_02_delay());
            atk3_cur += 1;
        }

        else {
            anim.SetTrigger("Atk_03_3");
            atk3_cur = 0;
        }
    }

    IEnumerator atk_03_02_delay() {
        yield return new WaitForSeconds(1f * delayLevel);
        float dis = transform.position.x - Target.position.x;
        int atkDis = (dis > 0) ? -1 : 1;

        anim.SetTrigger("Atk_03_2");
        transform.localScale = (atkDis < 0) ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
        rigid.velocity = new Vector2(atkDis * 20, rigid.velocity.y);
        setAtkBox(new Vector2(3, 4), new Vector2(5, -1));
    }
    public void atk_03_03() {
        isAtk = false;
        Physics2D.IgnoreLayerCollision(9, 11, false);
    }


    void setAtkboxOff() {
        rigid.velocity = Vector2.zero;
        setAtkBox(Vector2.zero, Vector2.zero);
    }

    

    void setMobColl(Vector2 off, Vector2 size) {

        GetComponent<CapsuleCollider2D>().offset = off;
        GetComponent<CapsuleCollider2D>().size = size;
    }







}   

