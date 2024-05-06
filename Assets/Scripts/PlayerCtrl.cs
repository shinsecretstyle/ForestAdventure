using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    public float Speed;
    public float RunSpeed;

    public int hp;
    public int maxHP;
    public Slider HPslider;
    InputAction move;
    InputAction jump;

    InputAction run;
    InputAction mouseX;
    InputAction mouseY;
    InputAction Def;
    InputAction Taunting;
    InputAction WrapMove;
    InputAction Skill1;

    Vector2 turn;
    int attackStage;
    private Animator m_Animator;
    public bool isAttacking;
    bool isAttack1;
    bool isAttack2;

    bool canMove;
    public int atk = 10;
    public int id;
    public float atkCD;
    public float jumpHeight = 10.0f;
    public float jumpCD = 1f;
    public bool isDefing;
    public bool canAttack;
    public float blinkDistance = 10f;
    public float blinkCooldown = 3f;

    bool canBlink = true;

    public bool canTeleport;

    public bool isTaunting;
    private int TauntingHash;

    public Transform blockTransform;
    public GameObject blockEF;
    public Transform bloodTransform;
    public GameObject bloodEF;
    bool canBlock;
    float BlockTime;

    GameObject Sword;
    Collider SwordCollider;

    bool notTalking = true;

    //slash effect
    public GameObject slashObj1;
    public GameObject slashObj2;
    //buff effect
    public GameObject HealEffect;

    public SkillCtrl SkillCtrl;
    CharacterController characterController;
    private void Awake()
    {
        maxHP = 120;
        if (PlayerPrefs.HasKey("PlayerHP"))
        {
            hp = PlayerPrefs.GetInt("PlayerHP");
            Debug.Log("load hp " + hp + " success");
        }else hp = maxHP;

        HPslider.maxValue = maxHP;
        HPslider.value = hp;

        isAttacking = false;
        attackStage = 1;
        Speed = 15;
        RunSpeed = 25;
        atk = 1;
        canAttack = true;
        PlayerInput input = GetComponent<PlayerInput>();

        move = input.actions["Move"];
        jump = input.actions["Jump"];
        run = input.actions["Run"];
        mouseX = input.actions["MouseX"];
        mouseY = input.actions["MouseY"];
        Def = input.actions["Defense"];
        Taunting = input.actions["Taunting"];
        WrapMove = input.actions["Wrap"];
        Skill1 = input.actions["Skill1"];
        m_Animator = GetComponent<Animator>();

        canMove = true;
        isDefing = false;
        canBlock = false;

        id = 1;
        atkCD = 0.4f;
        SwordCollider = GetComponentInChildren<BoxCollider>();

        characterController = GetComponent<CharacterController>();

        Cursor.visible = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        jumpHeight = 20.0f;
    }

    // Update is called once per frame
    void Update()
    {
        HPslider.value = hp;
        if(hp <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
        Vector2 m = move.ReadValue<Vector2>();
        var horizontal = m.x;
        var vertical = m.y;
        var velocity = new Vector3(horizontal, 0, vertical).normalized;
        var speed = run.IsPressed() ? 2 : 1;
        var rotationSpeed = 600 * Time.deltaTime;
        
        m_Animator.SetFloat("Speed", velocity.magnitude * speed * m.y, 0.1f, Time.deltaTime);

        Movement();

        isDefing = m_Animator.GetBool("Defense");

        if(isDefing && !Def.IsPressed())
        {
            m_Animator.SetBool("Defense", false);
            canMove = true;
        }

        //isTaunting = m_Animator.GetBool("Taunting");
        
        //test for blink skill
        if(isTaunting && IsAnimFinished("Taunting"))
        {
            isTaunting= false;
            canMove = true;
        }

        if (canBlock)
        {
            BlockTime-= Time.deltaTime;
        }

        if(BlockTime < 0)
        {
            canBlock = false;
        }


        if(WrapMove.triggered)
        {
            Debug.Log("Wrap is Pressed");
        }
    }

    void Movement()
    {

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        Vector2 moveInput = move.ReadValue<Vector2>();
        
        Vector3 moveDirection = cameraForward * moveInput.y + cameraRight * moveInput.x;
        moveDirection.y -= Physics.gravity.magnitude * Time.deltaTime *30;
        moveDirection.Normalize();
        
        float speed = Speed;
        
        Vector3 newPosition = transform.position;
        if (run.IsPressed())
        {
            speed = RunSpeed;
        }
        else speed = Speed;

        if(moveInput.y < 0)
        {
            speed *= -1;
            moveDirection *= -1;
        }
        //if(jump.IsPressed() && jumpCD <=0)
        //{
        //    moveDirection.y = Mathf.Sqrt(100 * Physics.gravity.magnitude * jumpHeight);
        //    Debug.Log("jump");
        //    jumpCD = 1f;

        //    moveDirection.y *= 0.5f;
        //}

        if (canMove)
        {
            characterController.Move(moveDirection * speed * Time.deltaTime);
        }

        turn.x += Mouse.current.delta.x.ReadValue() * Time.deltaTime * 80;
        turn.y += Mouse.current.delta.y.ReadValue();
        transform.localRotation = Quaternion.Euler(0, turn.x, 0);
        
    }

    void OnFire() {

        if (canAttack)
        {
            isAttacking = true;
            canAttack = false;
            m_Animator.SetTrigger("Attacks");
        }
        StartCoroutine(resetCD());
    }
    
    void OnDefense()
    {
        m_Animator.SetBool("Defense",true);
        canMove = false;
        canBlock = true;
        BlockTime = 0.4f;
    }

    void OnTaunting()
    {
        //test blink
        //m_Animator.SetTrigger("WrapMove");
        //isTaunting = true;
        //Debug.Log("Taunting");
    }

    bool IsAnimFinished(string stateName) {//test

        AnimatorStateInfo animaStateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
        if (animaStateInfo.IsName(stateName))
        {
            //Debug.Log(stateName + " is finished");
        }
        return animaStateInfo.IsName(stateName) && animaStateInfo.normalizedTime >= 0.28f;
    }

    void ApplyDamage(int power)
    {
        if (canBlock && isDefing)
        {
            Instantiate(blockEF, blockTransform.position, Quaternion.identity);
            Debug.Log("Just Block");
        }
        else if (isDefing && !canBlock)
        {
            m_Animator.SetTrigger("DefenseHit");
            hp -= power;
        }
        else 
        {
            m_Animator.SetTrigger("GetHit");
            Instantiate(bloodEF,bloodTransform.position, Quaternion.identity);
            hp -= power;
        }

    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Teleport"))
        {
            canTeleport = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Teleport"))
        {
            canTeleport = false;
        }
    }

    void OnTeleport()
    {
        
        if (canTeleport && SceneManager.GetActiveScene().name == "WorldMap")
        {
            PlayerPrefs.SetInt("PlayerHP", hp);
            PlayerPrefs.Save();
            SceneManager.LoadScene("BossStage");
        }
        else if (canTeleport && SceneManager.GetActiveScene().name == "BossStage")
        {
            PlayerPrefs.SetInt("PlayerHP", hp);
            PlayerPrefs.Save();
            SceneManager.LoadScene("WorldMap");
        }
    }
    void onWrap()
    {
        //m_Animator.SetTrigger("WrapMove");
        //Debug.Log("Move");
    }

    public void wrapMove()
    {

        Vector3 blinkEndPosition = transform.position + transform.forward * blinkDistance;
        characterController.enabled = false;
        transform.position = blinkEndPosition;
        characterController.enabled = true;
    }

    void OnExitGame()
    {
        Application.Quit();
        Debug.Log("exit");
    }

    //healing skill
    void OnSkill1()
    {
        if (SkillCtrl.skill1canUse)
        {
            SkillCtrl.useSkill1();
            StartCoroutine(Heal());
            if (hp >= maxHP - 20)
            {
                hp = maxHP;
            }
            else hp += 20;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        
    }

    IEnumerator resetCD()
    {
        yield return new WaitForSeconds(atkCD);
        canAttack = true;
        isAttacking = false;
    }

    IEnumerator Slash1()
    {
        slashObj1.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        slashObj1.SetActive(false);
    }
    IEnumerator Slash2()
    {
        slashObj2.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        slashObj2.SetActive(false);
    }

    IEnumerator Heal()
    {
        HealEffect.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        HealEffect.SetActive(false);
    }

}
