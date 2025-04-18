using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Breakable;

[System.Serializable]
public class playerItems {
    public string itemName;
    public bool isHave;
}

public class Playrer : MonoBehaviour{

    // public GameObject parentObj;
    public static Playrer Instance;

    public Vector2 inputVec;
    public float maxSpeed;
    public float curHp;
    public float MaxHp;
    public float jump;
    public float crawlSpeed;
    public float globalDelay;

    public bool allowUnderJump;
    public bool onInteract;

    public bool isRope;
    public bool isWater;
    public bool isOnPad;
    public bool isBlood;
    public bool isKnock;
    public bool isStick;
    public bool isDead;

    public bool onMovingPlat;
    public float onMPSpeed;
    public enum AtkType { normal, water, fire, earth, metal }
    public AtkType atkType;
    public bool isAtk; // temporary, need to change;

    public Vector3 rayPos;
    public float rayLen;

    public GameObject AtkBox;
    public LetterBoxCtrl letterBox;
    public Transform moveable;
    public Transform upperRope;
    public Transform lowerRope;
    public Transform playerPar;

    public bool jumping;
    public playerItems[] items;

    //TODO: Key Binding Added.

    public ISkillScript KeyZAction;
    public ISkillScript KeyXAction;
    public ISkillScript KeyCAction;

    // <<<<<<<<<<<<<<<<


    #region Component Paramas
    public Rigidbody2D rigid;
    public SpriteRenderer sprite;
    public Animator anim;
    public Collider2D coll;
    public FixedJoint2D fixJoint;
    #endregion

    private void Awake() {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        fixJoint = GetComponent<FixedJoint2D>();
        letterBox = letterBox.GetComponent<LetterBoxCtrl>();


        fixJoint.connectedBody = null;
        fixJoint.enabled = false;
        isRope = false;
    }

    private void FixedUpdate() {
        if(curHp <= 0 && !isDead) {
            isDead = true;
            anim.SetTrigger("Dead");
            // 인스펙터 플레이어의 애니메이션 "Dead" 끝에 이벤트 함수 추가해서 게임 오버 UI팝업 및 타임스케일  0으로

        }
        
                //Debug.Log($"Speed: {rigid.velocity.magnitude}, X: {rigid.velocity.x}, Y: {rigid.velocity.y}");
        //Debug.Log("Y: " + Mathf.Round(rigid.velocity.y));
        if (SkillPanelOpen) return;

        if (KeyZAction != null)
        {
            KeyZAction.Process();
        }
        if (KeyXAction != null)
        {
            KeyXAction.Process();
        }
        if (KeyCAction != null)
        {
            KeyCAction.Process();
        }

        
        globalDelay -= Time.deltaTime;
        findRope();
        LockFriction();
        ropePosCtrl();
        playerXflip();
        Vector2 nextVec = inputVec * maxSpeed;
        //player movement
        if (!isAtk && !isKnock && !isStick) {
            rigid.velocity += Vector2.right * nextVec;
        }

        if (isStick) {
            float limit = GetComponentInParent<Stick>().heightLimit;
            if (transform.localPosition.y > limit && inputVec.y > 0) {
                transform.localPosition = new Vector2(0, limit - 0.01f);
                return;
            }
            else {
                transform.Translate(inputVec.y * Vector2.up * Time.deltaTime * maxSpeed);
            }
            

        }


        //temp
        if (isRope) {

            if (nextVec.y > 0) {
                transform.position = Vector3.MoveTowards(transform.position, upperRope.position, maxSpeed * Time.deltaTime);
                if (lowerRope == null) {

                }
            }

            else if (nextVec.y < 0) {// move lower
                if (lowerRope == null) {
                    fixJoint.connectedBody = null;
                    fixJoint.enabled = false;
                    isRope = false;
                    return;
                }
                transform.position = Vector3.MoveTowards(transform.position, lowerRope.position, maxSpeed * Time.deltaTime);
                if (upperRope == null) {

                }
                
            }
            if (upperRope == null) {
                return;
            }
            else if (Vector3.Distance(transform.position, upperRope.position) < 0.01f && upperRope.GetComponent<FixedJoint2D>() != null) {
                fixJoint.connectedBody = upperRope.GetComponent<Rigidbody2D>();
            }

            if (lowerRope == null)
                return;

            else if (Vector3.Distance(transform.position, lowerRope.position) < 0.01f && lowerRope != null) {
                fixJoint.connectedBody = lowerRope.GetComponent<Rigidbody2D>();
            }

        }

        if (rigid.velocity.y < 0) {
            anim.SetBool("isWall", false);
        }

        if (rigid.velocity.x > maxSpeed) {
            rigid.velocity = new Vector2(maxSpeed * crawlSpeed, rigid.velocity.y);
        }
        else if (rigid.velocity.x < maxSpeed * (-1)) {
            rigid.velocity = new Vector2(maxSpeed * (-1) * crawlSpeed, rigid.velocity.y);
        }

        //player move anim
        if (!anim.GetBool("isWall") && !isStick) {
            bool move = (nextVec.x != 0) ? true : false;
            anim.SetBool("isMove", move);
        }


        //jump to under check
        rayPos = transform.position - new Vector3(0, 2.5f, 0); //range to enable
        RaycastHit2D hit = Physics2D.Raycast(rayPos, transform.up * -1, rayLen);
        Debug.DrawRay(rayPos, transform.up * -1 * rayLen, Color.yellow); //visualize

        //moveable object hold
        if(moveable != null) {

            //object direction
            if(transform.localScale.z < 0.1f)
                moveable.transform.position = transform.position + new Vector3(-2f, 3f, 0);
            else
                moveable.transform.position = transform.position + new Vector3(2f, 3f, 0);
            
            moveable.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        }




        if (hit.collider == null) {
            allowUnderJump = true;
            
        }

        else if (hit.collider.gameObject.layer == 6 && allowUnderJump == true) {
            allowUnderJump = false;
            StartCoroutine(underTime());
            
        }

        //Debug.DrawRay(rigid.position, Vector2.down, new Color(1, 0, 0));
        RaycastHit2D jumped = Physics2D.Raycast(rigid.position, Vector2.down, 1, LayerMask.GetMask("Ground"));

        if (jumped.distance > 0.5f && jumping) {
            anim.SetBool("isJump", true);
        }


    }

    private void OnCollisionStay2D(Collision2D collision) {


        if((collision.transform.tag == "Tilemaps"|| collision.transform.tag == "Enemy") && anim.GetBool("isJump") && !isKnock) {// player landing
            rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            anim.SetBool("isJump", false);
            isOnPad = false;
        } 

        if(collision.transform.tag == "Tilemaps") {
            Debug.DrawRay(rigid.position + new Vector2(-1, -0.25f), Vector2.right * 2, new Color(0, 1, 0));
            RaycastHit2D hitGround = Physics2D.Raycast(rigid.position + new Vector2(-1, -0.25f), Vector2.right, 2, LayerMask.GetMask("Ground"));

            
            if (hitGround.collider == null) {
                anim.SetBool("isWall", true);
                anim.SetBool("isMove", false);
            }
            else {
                anim.SetBool("isWall", false);
            }
        }

            

    }

    

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.transform.CompareTag("Stick") && (inputVec.y > 0)) {
            isStick = true;
            rigid.velocity = Vector2.zero;
            transform.SetParent(collision.transform);
            transform.position = new Vector2(collision.transform.position.x, transform.position.y);
            rigid.gravityScale = 0;
            anim.SetBool("isMove", false);
            anim.SetBool("isJump", false);
            anim.SetBool("isWall", true);


        }


        if (collision.transform.CompareTag("Rope") && !isRope && 0.05f < Mathf.Abs(inputVec.y)) {
            Rigidbody2D rig = collision.gameObject.GetComponent<Rigidbody2D>();
            fixJoint.enabled = true;
            fixJoint.connectedBody = rig;
            isRope = true;

            Vector2 playerPosition = transform.position;
            playerPosition.x = rig.transform.position.x;
            transform.position = playerPosition;
        }

        if (collision.transform.CompareTag("Water")) {
            Debug.Log("water");
            isWater = true;
            rigid.drag = 5;
            //rigid.gravityScale = 0;
        }

        if (collision.transform.CompareTag("Blood")) {
            Debug.Log("Blood");
            isBlood = true;
            StartCoroutine(onBlood(0.3f, 1));
        }
    }

    public void ropePosCtrl() {
        if(fixJoint.connectedBody != null && inputVec == Vector2.zero) {
            this.transform.position = fixJoint.connectedBody.transform.position;
        }
    }
    IEnumerator onBlood(float time, float dmg) {
        yield return new WaitForSeconds(time);
        Debug.Log("player damaged : " + dmg);

        PlayerManager.Instance.CurHP -= dmg;

        // curHp -= dmg;
        StartCoroutine(playerHitEff());


        if (isBlood)
            StartCoroutine(onBlood(time, dmg));
    }

    IEnumerator underTime() {
        yield return new WaitForSeconds(0.5f);
        this.gameObject.layer = 9;
    }

    private void OnTriggerExit2D(Collider2D collision) {

        if (collision.transform.CompareTag("Stick")){
            resetParent();
            isStick = false;
            rigid.gravityScale = 3;
        }


        if (collision.transform.CompareTag("Tilemaps")){
            anim.SetBool("isWall", false);
        }

        if (collision.transform.CompareTag("Water")) {
            Debug.Log("water out");
            anim.SetBool("isJump", false);
            isWater = false;
            rigid.drag = 0.2f;
            //rigid.gravityScale = 3;
        }

        if (collision.transform.CompareTag("Blood")) {
            Debug.Log("water out");
            isBlood = false;
            anim.SetBool("isJump", false);
            isWater = false;
            rigid.drag = 0.2f;
            //rigid.gravityScale = 3;
        }

    }


    public IEnumerator playerHitEff() {
        Color origin = new Color(1, 1, 1, 1);
        Color eff = new Color(1, 1, 1, 0.25f);
        for(int i = 0; i < 6; i++) {
            sprite.color = (i % 2 == 1) ? origin : eff;
            yield return new WaitForSeconds(0.1f);
        }
    }
    public IEnumerator playerKnock(Transform push, float force) {
        
            

        isKnock = true;
        StartCoroutine(playerHitEff());

        rigid.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        Vector2 knockbackDirection = (transform.position - push.transform.position).normalized;
        float dirX = knockbackDirection.x >= 0 ? 1 : -1;
        // 반대 방향으로 힘을 가함
        rigid.AddForce(Vector2.right * dirX * force * 20, ForceMode2D.Impulse);





        if(transform.position.y > push.transform.position.y) {
            rigid.AddForce(Vector2.up * force * 10, ForceMode2D.Impulse);
            rigid.AddForce(Vector2.right * dirX * force * 50, ForceMode2D.Impulse);//?
        }
        yield return new WaitForSeconds(0.3f);
        rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        isKnock = false;
    }
    public void resetParent() {
        transform.SetParent(playerPar);
    }

    void findRope() {
        if (isRope) { //find upper & lower
            upperRope = fixJoint.connectedBody.GetComponent<FixedJoint2D>().connectedBody.transform;
            foreach (GameObject rope in fixJoint.connectedBody.GetComponentInParent<Ropes>().ropeIndex) {
                if(rope == null) {
                    lowerRope = null;   
                    break;
                }
                if (rope.GetComponent<FixedJoint2D>().connectedBody == fixJoint.connectedBody) {
                    lowerRope = rope.transform;
                    break;
                }
            }

        }
        else {
            upperRope = null;
            lowerRope = null;
        }
    }
    void LockFriction() {
        //player position freeze
        if (inputVec.x == 0) {  //no input = freeze
            if (!onMovingPlat && !isRope && !isOnPad && !isKnock)
{
                rigid.velocity = Vector2.MoveTowards(rigid.velocity, Vector2.zero, Time.deltaTime * 10f);
            }
                // rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
        else {
            rigid.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public void letterboxOn() {
        letterBox.GetComponent<LetterBoxCtrl>().StartCoroutine(letterBox.GetComponent<LetterBoxCtrl>().letterboxOn());
    }
    public void letterboxOff() {
        letterBox.GetComponent<LetterBoxCtrl>().StartCoroutine(letterBox.GetComponent<LetterBoxCtrl>().letterboxOff());
    }

    void OnMove(InputValue value) {

        //player movement, input system
        inputVec = value.Get<Vector2>();

        //change player sprite flipX
        if (SkillPanelOpen)
        {
            SkillPanel.instance.RollSelectCursor(inputVec);

            return;
        }

        //change player sprite flipX
        playerXflip();


        //change player collider size
        playerCrawl();      

    }

    public void playerXflip() {
        if (isAtk || isStick)
            return;
        //player sprite flipX
        if (inputVec.x < 0) {
            sprite.flipX = true;
        }
        else if (inputVec.x > 0) {
            sprite.flipX = false;
        }
    }
     
    public void playerCrawl() {
        //check player's crawl
        if (inputVec.y <= -0.05f) {
            coll.offset = new Vector2(0, 0.5f);
            coll.GetComponent<CapsuleCollider2D>().size = new Vector2(1f, 0.9f);
            anim.SetBool("isCrawl", true);
            crawlSpeed = 0.5f;
            jump = 3;
        }
        else {
            coll.offset = new Vector2(0, 1f);
            coll.GetComponent<CapsuleCollider2D>().size = new Vector2(1f, 1.8f);
            anim.SetBool("isCrawl", false);
            crawlSpeed = 1f;
            jump = 18;
        }
    }
    
    void OnJump() {
    	if (SkillPanelOpen) return;


        fixJoint.connectedBody = null;
        fixJoint.enabled = false;
        isRope = false;
        resetParent();

        if (isStick) {
            isStick = false;
            rigid.gravityScale = 3;
        }


        if (anim.GetBool("isJump")) return;


        anim.SetBool("isWall", false);

        //jump anim
        if (!anim.GetBool("isJump") || isWater) {
            anim.SetBool("isJump", true);
            rigid.velocity = Vector2.up * jump;
            StartCoroutine(jumpCheck());
        }

        if (allowUnderJump && inputVec.y <= -0.05f) {
            this.gameObject.layer = 7;
        }
    }

    IEnumerator jumpCheck() {
        jumping = true;
        yield return new WaitForSeconds(0.2f);
        jumping = false;
    }

    void OnDash() {
        if (SkillPanelOpen) return;


        if (this.gameObject.layer == 10 || isAtk)
            return;

        //player dash
        if (anim.GetBool("isMove") && !isRope) {
            
            //change layer : PlayerGhost
            this.gameObject.layer = 10;
            anim.SetBool("isWall", false);
            anim.SetTrigger("Dash");
            maxSpeed = 48;
            rigid.AddForce(Vector2.right * 1 * inputVec.x, ForceMode2D.Impulse);
        }
    }
    
    void OnAtk_00() {
        if (this.gameObject.layer == 10 || globalDelay > 0)
            return;

        globalDelay = 0.5f;
        isAtk = true;
        anim.SetTrigger("Atk_00");
        StartCoroutine(Atk_00_Set());
    }
    
    void OnKeyZ(InputValue value)
    {

        if (value.isPressed)
        {
            if (KeyZAction == null && SkillPanelOpen == false)
            {
                if (this.gameObject.layer == 10)
                    return;

                isAtk = true;
                anim.SetTrigger("Atk_00");
                StartCoroutine(Atk_00_Set());
                return;
            }

            if (SkillPanelOpen) return;
            if (KeyZAction != null) KeyZAction.Execute();
        }
        else
        {
            if (SkillPanelOpen)
            {
                KeyZAction = SkillPanel.instance.EquipSkill(SkillKeybinding.KeyZ);
                return;
            }

            if (KeyZAction != null) KeyZAction.Release();
        }
    }
    void OnKeyX(InputValue value)
    {

        if (value.isPressed)
        {
            if (KeyXAction == null && SkillPanelOpen == false)
            {
                if (this.gameObject.layer == 10)
                    return;

                isAtk = true;
                anim.SetTrigger("Atk_00");
                StartCoroutine(Atk_00_Set());
                return;
            }

            if (SkillPanelOpen) return;
            if (KeyXAction != null) KeyXAction.Execute();
        }
        else
        {
            if (SkillPanelOpen)
            {
                KeyXAction = SkillPanel.instance.EquipSkill(SkillKeybinding.KeyX);
                return;
            }

            if (KeyXAction != null) KeyXAction.Release();
        }
    }
        void OnKeyC(InputValue value)
    {


        if (value.isPressed)
        {
            if (KeyCAction == null && SkillPanelOpen == false)
            {
                if (this.gameObject.layer == 10)
                    return;

                isAtk = true;
                anim.SetTrigger("Atk_00");
                StartCoroutine(Atk_00_Set());
                return;
            }

            if (SkillPanelOpen) return;
            if (KeyCAction != null) KeyCAction.Execute();
        }
        else
        {
            if (SkillPanelOpen)
            {
                KeyCAction = SkillPanel.instance.EquipSkill(SkillKeybinding.KeyC);
                return;
            }

            if (KeyCAction != null) KeyCAction.Release();
        }
    }
        bool SkillPanelOpen;
    void OnKeyQ(InputValue value)
    {
        if (PlayerSkillManager.Instance.ChangeReady == false) return;

        if (value.isPressed)
        {
            SkillPanelOpen = true;
            SkillPanel.instance.gameObject.SetActive(value.isPressed);
        }
        else
        {
            SkillPanelOpen = false;
            SkillPanel.instance.gameObject.SetActive(value.isPressed);
        }
    }

    IEnumerator Atk_00_Set() {

        yield return new WaitForSeconds(0.1f);

        if(sprite.flipX == true)
            AtkBox.GetComponent<Atk_Box>().BoxSet(new Vector2(5, 3), new Vector2(-1, 2), 3);
        else
            AtkBox.GetComponent<Atk_Box>().BoxSet(new Vector2(5, 3), new Vector2(1, 2), 3);


        yield return new WaitForSeconds(0.3f);
        AtkBox.gameObject.SetActive(false);

    }

    public void Atk_Off() {
        isAtk = false;
    }

    public void OffDash() {
        maxSpeed = 12;   
        this.gameObject.layer = 9;      
    }


    IEnumerator interDelay() {
        onInteract = true;
        yield return new WaitForSeconds(0.5f);
        onInteract = false;
    }


    public void OnInteract() {
        if(moveable != null) { //drop down moveable
            moveable.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
            moveable = null;
        }

        if (this.transform.GetComponent<Scanner>().nearestTarget != null) {   //if moveable object is near
            moveable = this.transform.GetComponent<Scanner>().nearestTarget; //catch the nearest moveable
        }
        else {
            StartCoroutine(interDelay()); //active interact
        }


        if (this.transform.GetComponent<Scanner>().nearestTarget == null)
            return;

        switch (this.transform.GetComponent<Scanner>().nearestTarget.tag) {
            case "MassObj":
                if (this.transform.GetComponent<Scanner>().nearestTarget.GetComponent<DistanceJoint2D>().connectedBody == this.rigid)
                    this.transform.GetComponent<Scanner>().nearestTarget.GetComponent<DistanceJoint2D>().connectedBody = this.transform.GetComponent<Scanner>().nearestTarget.GetComponent<Rigidbody2D>();
                else
                    this.transform.GetComponent<Scanner>().nearestTarget.GetComponent<DistanceJoint2D>().connectedBody = this.rigid;
                break;
        }

    }
    
    public void OnPause(InputValue value)
    {
        Time.timeScale = 0f;
        GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
        GetComponent<PlayerInput>().enabled = false;
        
        PauseController.Instance.gameObject.SetActive(true);
        PauseController.Instance.GetComponent<PlayerInput>().enabled = true;
        PauseController.Instance.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
        
    }

}
