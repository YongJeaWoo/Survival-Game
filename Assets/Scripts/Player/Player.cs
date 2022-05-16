﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpPower = 8f;

    float hAxis;
    float vAxis;

    bool walkDown;
    bool jumpDown;
    bool dodgeDown;

    bool interAction;

    bool isJump;
    bool isDodge;

    bool isBorder;

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

    Vector3 moveVec;
    Vector3 dodgeVec;

    Rigidbody rigid;
    Animator anim;

    GameObject objs;
    Weapon equipWeapons;

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
    public GameObject[] grenades;

    public Camera cam;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        InputGet();
        Move();
        Jump();
        Attack();
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
        Debug.DrawRay(transform.position, transform.forward * 1f, Color.green);
        isBorder = Physics.Raycast(transform.position, transform.forward, 1f, LayerMask.GetMask("Border", "Objects"));
    }

    private void FixedUpdate()
    {
        FreezeRotation();
        StopWall();
    }

    // 충돌 수정 끝

    void InputGet()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        walkDown = Input.GetButton("Walk");
        jumpDown = Input.GetButtonDown("Jump");
        fireDown = Input.GetButton("Fire1");
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


        if (isSwap || isReload ||!isFire)
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
        if (jumpDown && !isJump && !isDodge && !isReload)
        {
            // 즉각 반응
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            rigid.angularVelocity = Vector3.zero;       // 관성 움직임 없애기
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
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

        if (fireDown && isFire && !isDodge && !isSwap)
        {
            equipWeapons.Use();
            anim.SetTrigger(equipWeapons.type == Weapon.Type.Melee ? "doSwing" : "doShot");
            fireDelay = 0;
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

        if (ReloadDown && !isJump && !isDodge && !isSwap && isFire)
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
        if (dodgeDown && !isJump && moveVec != Vector3.zero && !isDodge)
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
        if (interAction && objs != null && !isDodge)
        {
            if (objs.tag == "Weapons")
            {
                Item item = objs.GetComponent<Item>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;

                Destroy(objs);
            }
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
        if (other.tag == "Items")
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
