using System;
using Photon.Pun;
using UnityEngine;

// ����ü�μ� ������ ���� ������Ʈ���� ���� ���븦 ����
// ü��, ������ �޾Ƶ��̱�, ��� ���, ��� �̺�Ʈ�� ����
public class LivingEntity : MonoBehaviourPun, IDamageable
{
    public float startingHealth = 100f; // ���� ü��
    public float health; //{ get; protected set; } // ���� ü��
    public bool isDead; //{ get; protected set; } // ��� ����
    public event Action onDeath; // ����� �ߵ��� �̺�Ʈ


    // ȣ��Ʈ->��� Ŭ���̾�Ʈ �������� ü�°� ��� ���¸� ����ȭ �ϴ� �޼���
    [PunRPC]
    public virtual void ApplyUpdatedHealth(float newHealth, bool newDead)
    {
        health = newHealth;
        isDead = newDead;
    }

    // ����ü�� Ȱ��ȭ�ɶ� ���¸� ����
    protected virtual void OnEnable()
    {
        // ������� ���� ���·� ����
        isDead = false;
        // ü���� ���� ü������ �ʱ�ȭ
        health = startingHealth;
    }

    // ������ ó��
    // ȣ��Ʈ���� ���� �ܵ� ����ǰ�, ȣ��Ʈ�� ���� �ٸ� Ŭ���̾�Ʈ�鿡�� �ϰ� �����
    [PunRPC]
    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // ��������ŭ ü�� ����
            health -= damage;

            // ȣ��Ʈ���� Ŭ���̾�Ʈ�� ����ȭ
            photonView.RPC("ApplyUpdatedHealth", RpcTarget.Others, health, isDead);

            // �ٸ� Ŭ���̾�Ʈ�鵵 OnDamage�� �����ϵ��� ��
            photonView.RPC("OnDamage", RpcTarget.Others, damage, hitPoint, hitNormal);
        }

        // ü���� 0 ���� && ���� ���� �ʾҴٸ� ��� ó�� ����
        if (health <= 0 && !isDead)
        {
            Die();
        }
    }


    // ü���� ȸ���ϴ� ���
    [PunRPC]
    public virtual void RestoreHealth(float newHealth)
    {
        if (isDead)
        {
            // �̹� ����� ��� ü���� ȸ���� �� ����
            return;
        }

        // ȣ��Ʈ�� ü���� ���� ���� ����
        if (PhotonNetwork.IsMasterClient)
        {
            // ü�� �߰�
            health += newHealth;

            // �������� Ŭ���̾�Ʈ�� ����ȭ
            photonView.RPC("ApplyUpdatedHealth", RpcTarget.Others, health, isDead);

            // �ٸ� Ŭ���̾�Ʈ�鵵 RestoreHealth�� �����ϵ��� ��
            photonView.RPC("RestoreHealth", RpcTarget.Others, newHealth);
        }
    }

    public virtual void Die()
    {
        // onDeath �̺�Ʈ�� ��ϵ� �޼��尡 �ִٸ� ����
        if (onDeath != null)
        {
            onDeath();
        }

        // ��� ���¸� ������ ����
        isDead = true;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attack"))
        {
            Debug.Log("!");
            PlayerController playercontroller = other.transform.parent.GetComponent<PlayerController>();
            //PlayerHealth playerHealth = transform.parent.GetComponent<PlayerHealth>();
            float getdamage = playercontroller.damage;
            Vector3 hitpoint = other.ClosestPoint(transform.position);
            Vector3 hitnormal = transform.position - other.transform.position;
            photonView.RPC("OnDamage",RpcTarget.MasterClient, getdamage, hitpoint, hitnormal);
            Debug.Log("1"); 
        }
    }
}