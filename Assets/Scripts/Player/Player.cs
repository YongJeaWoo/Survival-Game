using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpPower = 8f;

    public GameManager manager;
    public TalkManager talkManager;

    float hAxis;
    float vAxis;

    bool walkDown;
    bool jumpDown;
    bool dodgeDown;
    bool grenadeDown;

    bool interAction;

    bool isJump;
    bool isDodge;
    bool isDead;

    bool isBorder;

    // 사운드
    public AudioSource jumpSound;

    // 무기 스왑
    bool isSwap;
    bool swap1;
    bool swap2;
    bool swap3;

    // 공격
    bool fireDown;
    bool isFire = true;
    float fireDelay;

    // 장전
    bool ReloadDown;
    bool isReload;

    // 무적
    bool isDamage;

    Vector3 moveVec;
    Vector3 dodgeVec;

    Rigidbody rigid;
    Animator anim;

    GameObject scanObject;

    GameObject objs;
    public Weapon equipWeapons;
    MeshRenderer[] meshRender;

    int equipWeaponIndex = -1;

    [Header ("Player Info")]
    // 캐릭터 뭐가 있지
    public int ammo;
    public int maxAmmo;
    public int hp;
    public int maxHp;
    public int hasGrenades;
    public int maxHasGrenades;

    [Header("Weapons Info")]
    public GameObject[] weapons;
    public bool[] hasWeapons;

    [Header("Grenade Info")]
    public GameObject[] grenades;
    public GameObject grenadeObj;
    public Transform grenadePos;

    public Camera cam;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        meshRender = GetComponentsInChildren<MeshRenderer>();
    }

    private void Update()
    {
        InputGet();
        Move();
        Jump();
        Attack();
        Grenade();
        Reload();
        Dodge();
        InterAction();
        Swap();
    }

    // 플레이어 충돌 수정 

    // 충돌 회전 방지
    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
    }

    // 벽 충돌 뚫기 방지
    void StopWall()
    {
        Vector3 rayVec = transform.position;
        rayVec.y += 0.5f;
        Debug.DrawRay(rayVec, transform.forward * 1f, Color.green);
        isBorder = Physics.Raycast(rayVec, transform.forward, 1f, LayerMask.GetMask("Border", "Objects"));
    }

    void Scanning()
    {
        RaycastHit rayHit;
        Vector3 searchVec = transform.position;
        searchVec.y += 0.3f;
        Debug.DrawRay(searchVec, transform.forward * 1f, Color.red);
        bool check = Physics.Raycast(searchVec, transform.forward, out rayHit, 1f, LayerMask.GetMask("NPC"));
        
        if (check)
        {
            scanObject = rayHit.collider.gameObject;
        }
        else
        {
            scanObject = null;
        }
    }

    private void FixedUpdate()
    {
        FreezeRotation();
        StopWall();
        Scanning();
    }

    // 충돌 수정 끝

    void InputGet()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        walkDown = Input.GetButton("Walk");
        jumpDown = Input.GetButtonDown("Jump");
        fireDown = Input.GetButton("Fire1");
        grenadeDown = Input.GetButtonDown("Fire2");
        ReloadDown = Input.GetButtonDown("Reload");
        dodgeDown = Input.GetButtonDown("Dodge");
        interAction = Input.GetButtonDown("InterAction");
        swap1 = Input.GetButtonDown("Swap1");
        swap2 = Input.GetButtonDown("Swap2");
        swap3 = Input.GetButtonDown("Swap3");
    }

    void Swap()
    {
        if (swap1 && (!hasWeapons[0] || equipWeaponIndex == 0))
            return;
        if (swap2 && (!hasWeapons[1] || equipWeaponIndex == 1))
            return;
        if (swap3 && (!hasWeapons[2] || equipWeaponIndex == 2))
            return;

        int weaponIndex = -1;
        
        if (swap1) weaponIndex = 0;
        if (swap2) weaponIndex = 1;
        if (swap3) weaponIndex = 2;

        if ((swap1 || swap2 || swap3) && !isJump && !isDodge && !isSwap)
        {
            if (equipWeapons != null)
                equipWeapons.gameObject.SetActive(false);             // 

            equipWeaponIndex = weaponIndex;
            equipWeapons = weapons[weaponIndex].GetComponent<Weapon>();       // 해당 무기 가지기
            equipWeapons.gameObject.SetActive(true);

            anim.SetTrigger("doSwap");

            isSwap = true;

            Invoke("SwapTime", 0.4f);
        }
    }

    void SwapTime()
    {
        isSwap = false;
    }

    void Move()
    {
        // 움직이기
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        moveVec = Quaternion.Euler(0, cam.transform.rotation.eulerAngles.y, 0) * moveVec;

        // 회피하면서 방향 전환?
        if (isDodge)
            moveVec = dodgeVec;

        // 움직임 제한
        if (isSwap || isReload ||!isFire || isDead || talkManager.isAction)
            moveVec = Vector3.zero;

        if (!isBorder)
            transform.position += moveVec * moveSpeed * (walkDown ? 0.3f : 1f) * Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", walkDown);

        // 회전
        transform.LookAt(transform.position + moveVec);
    }

    void Jump()
    {
        if (jumpDown && !isJump && !isDodge && !isReload && !isDead && (!talkManager.isAction))
        {
            // 즉각 반응
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            rigid.angularVelocity = Vector3.zero;       // 관성 움직임 없애기
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;

            jumpSound.Play();
        }
    }

    void Attack()
    {
        if (equipWeapons == null)
            return;

        if (isReload)
            return;

        fireDelay += Time.deltaTime;
        isFire = equipWeapons.rate < fireDelay;

        if (fireDown && isFire && !isDodge && !isSwap && !isDead && !(talkManager.isAction))
        {
            equipWeapons.Use();
            anim.SetTrigger(equipWeapons.type == Weapon.Type.Melee ? "doSwing" : "doShot");
            fireDelay = 0;
        }
    }
    
    void Grenade()
    {
        if (hasGrenades == 0)
            return;

        if (grenadeDown && !isSwap && !isDodge && !isReload && !isDead && !(talkManager.isAction))
        {
            GameObject instantGrenade = Instantiate(grenadeObj, grenadePos.position, grenadePos.rotation);
            Rigidbody grenadeRigid = instantGrenade.GetComponent<Rigidbody>();

            Vector3 grenadeVec = grenadePos.forward * 10f + Vector3.up * 5f;

            grenadeRigid.AddForce(grenadeVec, ForceMode.Impulse);
            grenadeRigid.AddTorque(Vector3.back * 20f, ForceMode.Impulse);

            hasGrenades--;
            grenades[hasGrenades].SetActive(false);
        }
    }

    void Reload()
    {
        if (equipWeapons == null)
            return;

        if (equipWeapons.type == Weapon.Type.Melee)
            return;

        if (ammo == 0)
            return;

        if (equipWeapons.curAmmo == equipWeapons.maxAmmo)
            return;

        if (ReloadDown && !isJump && !isDodge && !isSwap && isFire && !isDead && !(talkManager.isAction))
        {
            anim.SetTrigger("doReload");
            isReload = true;

            Invoke("Reloading", 2.5f);
        }
    }

    void Reloading()
    {
        int reAmmo = ammo + equipWeapons.curAmmo < equipWeapons.maxAmmo ? ammo : equipWeapons.maxAmmo - equipWeapons.curAmmo;

        equipWeapons.curAmmo += reAmmo;
        ammo -= reAmmo;
        isReload = false;
    }

    void Dodge()
    {
        if (dodgeDown && !isJump && moveVec != Vector3.zero && !isDodge && !isDead && !(talkManager.isAction))
        {
            dodgeVec = moveVec;

            moveSpeed *= 2f;
            anim.SetTrigger("doDodge");
            isDodge = true;

            // 시간차 호출
            Invoke("DodgeTime", 0.7f);
        }
    }

    void DodgeTime()
    {
        moveSpeed *= 0.5f;
        isDodge = false;
    }

    void InterAction()
    {
        if (interAction && scanObject != null)
        {
            talkManager.Action(scanObject);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        // 착지 구현
        if (collision.gameObject.tag == "Ground")
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }

        if (collision.gameObject.tag == "Objects")
        {
            anim.SetBool("isJump", false);
        }

        if (collision.gameObject.tag == "Buildings")
        {
            anim.SetBool("isJump", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapons"))
        {
            Item item = other.GetComponent<Item>();
            int weaponIndex = item.value;
            hasWeapons[weaponIndex] = true;

            Destroy(other.gameObject);
        }

        else if (other.CompareTag("Items"))
        {
            Item item = other.GetComponent<Item>();
            switch (item.type)
            {
                case Item.Type.Grenade:
                    grenades[hasGrenades].SetActive(true);
                    hasGrenades += item.value;
                    if (hasGrenades > maxHasGrenades)
                        hasGrenades = maxHasGrenades;
                    break;
                case Item.Type.Hp:
                    if (hp > maxHp)
                        hp = maxHp;
                    break;
                case Item.Type.Ammunition:
                    ammo += item.value;
                    if (ammo > maxAmmo)
                        ammo = maxAmmo;
                    break;
            }
            Destroy(other.gameObject);
        }

        else if (other.CompareTag("EnemyMelee"))
        {
            if (!isDamage)
            {
                EnemyCol enemyCol = other.GetComponent<EnemyCol>();
                hp -= enemyCol.damage;
                StartCoroutine(OnDamage(false));
            }
        }

        else if (other.CompareTag("EnemyBullet"))
        {
            if (!isDamage)
            {
                enemyMissileTri enemyMissile = other.GetComponent<enemyMissileTri>();
                hp -= enemyMissile.damage;

                bool isBossAtk = other.name == "BossMelee";
                StartCoroutine(OnDamage(isBossAtk));
            }
        }

        // 굿 엔딩 분기점
        else if (other.CompareTag("Victory"))
        {
            manager.GoodEnding();
        }
    }

    IEnumerator OnDamage(bool isBossAtk)
    {
        isDamage = true;
        foreach(MeshRenderer mesh in meshRender)
        {
            mesh.material.color = Color.cyan;
        }
        if (isBossAtk)
            rigid.AddForce(-(transform.forward) * 25, ForceMode.Impulse);

        // 체력이 없으면 게임 오버
        if (hp <= 0 && !isDead)
        {
            hp = 0;
            OnDie();

            if (hp < 0)
                hp = 0;
        }

        yield return new WaitForSeconds(1f);

        isDamage = false;
        foreach (MeshRenderer mesh in meshRender)
        {
            mesh.material.color = Color.white;
        }

        if(isBossAtk)
            rigid.velocity = Vector3.zero;
    }

    void OnDie()
    {
        anim.SetTrigger("doDie");
        isDead = true;
        manager.GameOver();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapons")
            objs = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapons")
            objs = null;
    }
}
