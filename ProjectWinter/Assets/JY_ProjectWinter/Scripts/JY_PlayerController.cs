using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class JY_PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;
    private float aniSpeed = 0;

    private float horizontal;
    private float vertical;
    public static int speed = 5;

    private bool doSomething = false;

    // 플레이어의 직업
    public static bool isSurvivor;
    public static bool isTrator;
    // 플레이어의 직업

    private bool hand = false;

    private RaycastHit hitInfo;


    private Vector3 moveVec;

    private GameObject hitObject;
    // 공격관련
    private float attackPower;  // 마우스로 공격 차지
    private bool isAttack;
    // 공격관련
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

    }

    private void Update()
    {
        if (!doSomething)
        {
            PlayerMove();

            PLayerIsClick();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if(!doSomething)
            {
                // 플레이어의 앞에 있는 물체를 판별
                if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 1.0f))
                {
                    if (hitInfo.collider.gameObject.tag == "Player")
                    {
                        PlayerHealth playerHealth = hitInfo.collider.gameObject.GetComponent<PlayerHealth>();
                        bool isPlayerDown = playerHealth.isDown;
                        if (isPlayerDown)
                        {
                            PressE();        
                        }
                            
                    }
                }
                // 플레이어의 앞에 있는 물체를 판별

                // 행동이 완료돼 끝났을때도 두썸띵 거짓으로 만들기
            }
            else
            {
                doSomething = false;
                animator.SetBool("DoSomething", doSomething);
            }

        }
       

        // 플레이어의 앞에 있는 물체를 판별
        //if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 1.0f))
        //{
        //    
        //
        //}
        // 플레이어의 앞에 있는 물체를 판별        

    }


    private void PlayerMove()           // 플레이어 움직임
    {
        // 좌표이동
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        moveVec = new Vector3(horizontal, 0, vertical).normalized;

        transform.position += moveVec * speed * Time.deltaTime;

        transform.LookAt(transform.position + moveVec);
        // 좌표이동

        // 애니메이션
        if (Mathf.Abs(horizontal) > 0 || Mathf.Abs(vertical) > 0)
        {
            aniSpeed = Mathf.MoveTowards(aniSpeed, 1, Time.deltaTime * 3);
        }
        else
        {
            aniSpeed = Mathf.MoveTowards(aniSpeed, 0, Time.deltaTime * 3);
        }
        animator.SetFloat("Speed", aniSpeed);
        // 애니메이션
    }

    // 공격
    #region
    private void PLayerIsClick()
    {
        if (Input.GetMouseButton(0) && !isAttack)
        {
            Attack();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isAttack = true;
            StartCoroutine(EndAttack());
        }
    }

    private void Attack()
    {
        if(!hand)
        {       //아이템 안들었을때
            animator.Play("Punch_R", -1, 0.35f);

            attackPower += Time.deltaTime;
        }
        else
        {       //아이템 들었을때

            animator.Play("AttackAnimation", -1, 0.35f);

            attackPower += Time.deltaTime;
        }
    }

    private IEnumerator EndAttack()
    {
        if (!hand)
        {       //아이템 안들었을때
            animator.SetBool("attack", isAttack);

            yield return new WaitForSeconds(0.8f);
            isAttack = false;
            attackPower = 0;
        }
        else
        {       //아이템 들었을때
            animator.SetBool("attack", isAttack);

            yield return new WaitForSeconds(0.8f);
            isAttack = false;
            attackPower = 0;
        }
    }
    #endregion
    // 공격   

    private void PressE()
    {
        if (hitInfo.collider.gameObject.tag == "Item") // 루팅모션
        {
            
        }
        else        // 작업모션
        {
            animator.SetFloat("Speed", 0);
            doSomething = true;
            animator.SetBool("DoSomething", doSomething);
            animator.Play("DoSomething");
        }
        // 행동이 완료되기까지 남은 시간 게이지
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Building"))
        {
            GameObject cameraObject = GameObject.Find("CM vcam1");
            CameraFollow cameraFollow = cameraObject.gameObject.GetComponent<CameraFollow>(); // 카메라를 둘 오브잭트를 찾아 카메라를 둠
            bool isInside = cameraFollow.isInside;
            cameraFollow.isInside = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Building"))
        {
            GameObject cameraObject = GameObject.Find("CM vcam1");
            CameraFollow cameraFollow = cameraObject.gameObject.GetComponent<CameraFollow>(); // 카메라를 둘 오브잭트를 찾아 카메라를 둠
            bool isInside = cameraFollow.isInside;
            cameraFollow.isInside = false;
        }
    }

}
//animator.SetLayerWeight(1, 0.0f);       // 두번째(1) 레이어의 애니메이션을 멈춤