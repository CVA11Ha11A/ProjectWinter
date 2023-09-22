using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI; // AI, 내비게이션 시스템 관련 코드를 가져오기

public class BearMoving : LivingEntity
{

    public LayerMask whatIsTarget; // 공격 대상 레이어 

    private LivingEntity targetEntity;
    private NavMeshAgent navMeshAgent;

    private Animator animalAnimator;

    public float damage = 20f;
    public float timeBetAttack = 0.5f; // 공격 간격
    private float lastAttackTime; // 마지막 공격 시점


    public float speed = 3f; // 이동 속도

    private int currentWaypointIndex = 1;

    public float rotationSpeed = 5f; // 바라보는 속도


    // 추적할 대상이 존재하는지 알려주는 프로퍼티
    private bool hasTarget
    {
        get
        {
            // 추적할 대상이 존재하고, 대상이 사망하지 않았다면 true
            if (targetEntity != null && !targetEntity.isDead)
            {
                return true;
            }

            // 그렇지 않다면 false
            return false;
        }
    }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animalAnimator = GetComponent<Animator>();
    }

    [PunRPC]
    public void Setup(float newHealth, float newDamage,
    float newSpeed)
    {
        // 체력 설정
        startingHealth = newHealth;
        health = newHealth;
        // 공격력 설정
        damage = newDamage;
        // 내비메쉬 에이전트의 이동 속도 설정
        navMeshAgent.speed = newSpeed;

    }

    private void Start()
    {
        //// 호스트가 아니라면 AI의 추적 루틴을 실행하지 않음
        //if (!PhotonNetwork.IsMasterClient)
        //{
        //    return;
        //}

        // 게임 오브젝트 활성화와 동시에 AI의 추적 루틴 시작
        StartCoroutine(UpdatePath());

    }

    private void Update()
    {
        // 호스트가 아니라면 애니메이션의 파라미터를 직접 갱신하지 않음
        // 호스트가 파라미터를 갱신하면 클라이언트들에게 자동으로 전달되기 때문.
        //if (!PhotonNetwork.IsMasterClient)
        //{
        //    return;
        //}

        bool hasValidTarget = hasTarget && targetEntity != null && !targetEntity.isDead;

        // 추적 대상의 존재 여부에 따라 다른 애니메이션을 재생
        animalAnimator.SetBool("HasTarget", hasValidTarget);

        if (hasValidTarget)
        {
            // 타겟 방향으로 봄
            Vector3 lookDirection = (targetEntity.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(lookDirection.x, 0, lookDirection.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }


    // 주기적으로 추적할 대상의 위치를 찾아 경로를 갱신
    private IEnumerator UpdatePath()
    {
        // 살아있는 동안 무한 루프
        while (!isDead)
        {
            if (hasTarget)
            {

                Debug.Log("타겟을 찾았다.");
                // 추적 대상 존재 : 경로를 갱신하고 AI 이동을 계속 진행
                navMeshAgent.isStopped = false;
                navMeshAgent.stoppingDistance = 5;

                float distanceToTarget = Vector3.Distance(transform.position, targetEntity.transform.position);


                // 일정 거리 내에 도달하면 걸어가는 애니메이션을 끄고 공격 애니메이션을 켬
                if (distanceToTarget <= navMeshAgent.stoppingDistance)
                {
                    animalAnimator.SetBool("BearWalk", false);
                    animalAnimator.SetBool("BearAttack", true);
                    yield return new WaitForSeconds(2f);
                    animalAnimator.SetBool("BearAttack", false);
                    yield return new WaitForSeconds(1f);

                }
                else
                {
                    animalAnimator.SetBool("BearWalk", true);
                    animalAnimator.SetBool("BearAttack", false);
                    navMeshAgent.SetDestination(targetEntity.transform.position);

                }


            }
            else
            {
                Debug.Log("타겟을 못찾았다.");

                // 추적 대상 없음 : AI 이동 중지
                navMeshAgent.isStopped = true;
                animalAnimator.SetBool("BearAttack", false);
                animalAnimator.SetBool("BearWalk", false);

                //WayPointMoving();

                // 20 유닛의 반지름을 가진 가상의 구를 그렸을때, 구와 겹치는 모든 콜라이더를 가져옴
                // 단, targetLayers에 해당하는 레이어를 가진 콜라이더만 가져오도록 필터링
                Collider[] colliders =
                    Physics.OverlapSphere(transform.position, 15f, whatIsTarget);

                // 모든 콜라이더들을 순회하면서, 살아있는 플레이어를 찾기
                for (int i = 0; i < colliders.Length; i++)
                {
                    // 콜라이더로부터 LivingEntity 컴포넌트 가져오기
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();

                    // LivingEntity 컴포넌트가 존재하며, 해당 LivingEntity가 살아있다면,
                    if (livingEntity != null && !livingEntity.isDead)
                    {
                        // 추적 대상을 해당 LivingEntity로 설정
                        targetEntity = livingEntity;

                        // for문 루프 즉시 정지
                        break;
                    }
                }
            }

            // 0.25초 주기로 처리 반복
            yield return new WaitForSeconds(0.25f);
        }
    }


    // 데미지를 입었을때 실행할 처리
    [PunRPC]
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        //// 아직 사망하지 않은 경우에만 피격 효과 재생
        //if (!dead)
        //{
        //    // 공격 받은 지점과 방향으로 파티클 효과를 재생
        //    hitEffect.transform.position = hitPoint;
        //    hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
        //    hitEffect.Play();

        //    // 피격 효과음 재생
        //    //zombieAudioPlayer.PlayOneShot(hitSound);
        //}

        // LivingEntity의 OnDamage()를 실행하여 데미지 적용
        base.OnDamage(damage, hitPoint, hitNormal);
    }

    // 사망 처리
    public override void Die()
    {
        // LivingEntity의 Die()를 실행하여 기본 사망 처리 실행
        base.Die();

        // 다른 AI들을 방해하지 않도록 자신의 모든 콜라이더들을 비활성화
        Collider[] animalColliders = GetComponents<Collider>();
        for (int i = 0; i < animalColliders.Length; i++)
        {
            animalColliders[i].enabled = false;
        }

        // AI 추적을 중지하고 내비메쉬 컴포넌트를 비활성화
        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;

        // 사망 애니메이션 재생
        animalAnimator.SetTrigger("Die");

        StartCoroutine(DeathMotion());
        // 사망 효과음 재생
        //zombieAudioPlayer.PlayOneShot(deathSound);
    }

    IEnumerator DeathMotion()
    {
        yield return new WaitForSeconds(3.0f);

        PhotonNetwork.Instantiate("RawMeat", transform.position, Quaternion.identity);
        PhotonNetwork.Instantiate("RawMeat", transform.position + Vector3.forward, Quaternion.identity);
        PhotonNetwork.Instantiate("RawMeat", transform.position + Vector3.left, Quaternion.identity);

        PhotonNetwork.Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        // 자신이 사망하지 않았으며,
        // 최근 공격 시점에서 timeBetAttack 이상 시간이 지났다면 공격 가능
        if (!isDead && Time.time >= lastAttackTime + timeBetAttack && animalAnimator.GetBool("BearAttack"))
        {
            // 상대방으로부터 LivingEntity 타입을 가져오기 시도
            LivingEntity attackTarget = other.GetComponent<LivingEntity>();

            // 상대방의 LivingEntity가 자신의 추적 대상이라면 공격 실행
            if (attackTarget != null)
            {
                // 타겟이 사망하지 않은 경우에만 공격 실행
                if (!attackTarget.isDead)
                {

                    Debug.Log("곰이 때리나?");

                    // 최근 공격 시간을 갱신
                    lastAttackTime = Time.time;

                    // 상대방의 피격 위치와 피격 방향을 근삿값으로 계산
                    Vector3 hitPoint = other.ClosestPoint(transform.position);
                    Vector3 hitNormal = transform.position - other.transform.position;

                    if (animalAnimator.GetBool("BearAttack"))
                    {
                        attackTarget.OnDamage(damage, hitPoint, hitNormal);
                    }
                    // 공격 실행
                }
            }
        }
        else
        {
            // 공격 중이 아니라면 추적을 멈추고 애니메이션을 초기화
            navMeshAgent.isStopped = true;
            animalAnimator.SetBool("BearAttack", false);
        }
    }
}
