using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI; // AI, ������̼� �ý��� ���� �ڵ带 ��������

public class WolfMoving : LivingEntity
{

    public LayerMask whatIsTarget; // ���� ��� ���̾� 

    private LivingEntity targetEntity;
    private NavMeshAgent navMeshAgent;

    private Animator animalAnimator;

    public float damage = 20f;
    public float timeBetAttack = 0.5f; // ���� ����
    private float lastAttackTime; // ������ ���� ����


    public float speed = 3f; // �̵� �ӵ�

    private int currentWaypointIndex = 1;

    public float rotationSpeed = 5f; // �ٶ󺸴� �ӵ�

    // ������ ����� �����ϴ��� �˷��ִ� ������Ƽ
    private bool hasTarget
    {
        get
        {
            // ������ ����� �����ϰ�, ����� ������� �ʾҴٸ� true
            if (targetEntity != null && !targetEntity.isDead)
            {
                return true;
            }

            // �׷��� �ʴٸ� false
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
        // ü�� ����
        startingHealth = newHealth;
        health = newHealth;
        // ���ݷ� ����
        damage = newDamage;
        // ����޽� ������Ʈ�� �̵� �ӵ� ����
        navMeshAgent.speed = newSpeed;

    }

    private void Start()
    {
        //// ȣ��Ʈ�� �ƴ϶�� AI�� ���� ��ƾ�� �������� ����
        //if (!PhotonNetwork.IsMasterClient)
        //{
        //    return;
        //}

        // ���� ������Ʈ Ȱ��ȭ�� ���ÿ� AI�� ���� ��ƾ ����
        StartCoroutine(UpdatePath());


    }

    private void Update()
    {
        // ȣ��Ʈ�� �ƴ϶�� �ִϸ��̼��� �Ķ���͸� ���� �������� ����
        // ȣ��Ʈ�� �Ķ���͸� �����ϸ� Ŭ���̾�Ʈ�鿡�� �ڵ����� ���޵Ǳ� ����.
        //if (!PhotonNetwork.IsMasterClient)
        //{
        //    return;
        //}

        // ���� ����� ���� ���ο� ���� �ٸ� �ִϸ��̼��� ���
        bool hasValidTarget = hasTarget && targetEntity != null && !targetEntity.isDead;

        // ���� ����� ���� ���ο� ���� �ٸ� �ִϸ��̼��� ���
        animalAnimator.SetBool("HasTarget", hasValidTarget);

        if (hasValidTarget)
        {
            // Ÿ�� �������� ��
            Vector3 lookDirection = (targetEntity.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(lookDirection.x, 0, lookDirection.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }


    }


    // �ֱ������� ������ ����� ��ġ�� ã�� ��θ� ����
    private IEnumerator UpdatePath()
    {
        // ����ִ� ���� ���� ����
        while (!isDead)
        {
            if (hasTarget)
            {

                Debug.Log("Ÿ���� ã�Ҵ�.");
                // ���� ��� ���� : ��θ� �����ϰ� AI �̵��� ��� ����
                navMeshAgent.isStopped = false;
                navMeshAgent.stoppingDistance = 2;

                float distanceToTarget = Vector3.Distance(transform.position, targetEntity.transform.position);


                // ���� �Ÿ� ���� �����ϸ� �ɾ�� �ִϸ��̼��� ���� ���� �ִϸ��̼��� ��
                if (distanceToTarget <= navMeshAgent.stoppingDistance)
                {
                    animalAnimator.SetBool("WolfWalk", false);
                    animalAnimator.SetBool("WolfAttack", true);
                    yield return new WaitForSeconds(0.5f);
                    animalAnimator.SetBool("WolfAttack", false);
                    yield return new WaitForSeconds(1f);

                }
                else
                {
                    animalAnimator.SetBool("WolfWalk", true);
                    animalAnimator.SetBool("WolfAttack", false);
                    navMeshAgent.SetDestination(targetEntity.transform.position);

                }


            }
            else
            {
                //Debug.Log("Ÿ���� ��ã�Ҵ�.");

                // ���� ��� ���� : AI �̵� ����
                navMeshAgent.isStopped = true;
                animalAnimator.SetBool("WolfAttack", false);
                animalAnimator.SetBool("WolfWalk", false);

                //WayPointMoving();

                // 20 ������ �������� ���� ������ ���� �׷�����, ���� ��ġ�� ��� �ݶ��̴��� ������
                // ��, targetLayers�� �ش��ϴ� ���̾ ���� �ݶ��̴��� ���������� ���͸�
                Collider[] colliders =
                    Physics.OverlapSphere(transform.position, 20f, whatIsTarget);

                // ��� �ݶ��̴����� ��ȸ�ϸ鼭, ����ִ� �÷��̾ ã��
                for (int i = 0; i < colliders.Length; i++)
                {
                    // �ݶ��̴��κ��� LivingEntity ������Ʈ ��������
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();

                    // LivingEntity ������Ʈ�� �����ϸ�, �ش� LivingEntity�� ����ִٸ�,
                    if (livingEntity != null && !livingEntity.isDead)
                    {
                        // ���� ����� �ش� LivingEntity�� ����
                        targetEntity = livingEntity;

                        // for�� ���� ��� ����
                        break;
                    }
                }
            }

            // 0.25�� �ֱ�� ó�� �ݺ�
            yield return new WaitForSeconds(0.25f);
        }
    }


    // �������� �Ծ����� ������ ó��
    [PunRPC]
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        //// ���� ������� ���� ��쿡�� �ǰ� ȿ�� ���
        //if (!dead)
        //{
        //    // ���� ���� ������ �������� ��ƼŬ ȿ���� ���
        //    hitEffect.transform.position = hitPoint;
        //    hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
        //    hitEffect.Play();

        //    // �ǰ� ȿ���� ���
        //    //zombieAudioPlayer.PlayOneShot(hitSound);
        //}

        // LivingEntity�� OnDamage()�� �����Ͽ� ������ ����
        base.OnDamage(damage, hitPoint, hitNormal);
    }

    // ��� ó��
    public override void Die()
    {
        // LivingEntity�� Die()�� �����Ͽ� �⺻ ��� ó�� ����
        base.Die();

        // �ٸ� AI���� �������� �ʵ��� �ڽ��� ��� �ݶ��̴����� ��Ȱ��ȭ
        Collider[] animalColliders = GetComponents<Collider>();
        for (int i = 0; i < animalColliders.Length; i++)
        {
            animalColliders[i].enabled = false;
        }

        // AI ������ �����ϰ� ����޽� ������Ʈ�� ��Ȱ��ȭ
        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;

        // ��� �ִϸ��̼� ���
        animalAnimator.SetTrigger("Die");

        // ��� ȿ���� ���
        //zombieAudioPlayer.PlayOneShot(deathSound);
    }

    private void OnTriggerStay(Collider other)
    {
        // �ڽ��� ������� �ʾ�����,
        // �ֱ� ���� �������� timeBetAttack �̻� �ð��� �����ٸ� ���� ����
        if (!isDead && Time.time >= lastAttackTime + timeBetAttack && animalAnimator.GetBool("WolfAttack"))
        {
            // �������κ��� LivingEntity Ÿ���� �������� �õ�
            LivingEntity attackTarget = other.GetComponent<LivingEntity>();

            // ������ LivingEntity�� �ڽ��� ���� ����̶�� ���� ����
            if (attackTarget != null)
            {
                // Ÿ���� ������� ���� ��쿡�� ���� ����
                if (!attackTarget.isDead)
                {

                    Debug.Log("���� ������?");

                    // �ֱ� ���� �ð��� ����
                    lastAttackTime = Time.time;

                    // ������ �ǰ� ��ġ�� �ǰ� ������ �ٻ����� ���
                    Vector3 hitPoint = other.ClosestPoint(transform.position);
                    Vector3 hitNormal = transform.position - other.transform.position;

                    if (animalAnimator.GetBool("WolfAttack"))
                    {
                        attackTarget.OnDamage(damage, hitPoint, hitNormal);
                    }
                    // ���� ����
                }
            }
        }
        else
        {
            // ���� ���� �ƴ϶�� ������ ���߰� �ִϸ��̼��� �ʱ�ȭ
            navMeshAgent.isStopped = true;
            animalAnimator.SetBool("WolfAttack", false);
        }
    }


}