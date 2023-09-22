using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JY_PlayerHealth : LivingEntity
{
    private Animator animator;

    // 체력관련
    private float playerDown = 100;
    public bool isDown = false;

    private bool playerEnd;

    //private bool isDead = false;
    // 체력관련

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        // onDeath += Down;
        playerEnd = false;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Die()
    {
        base.Die();
    }

    private void onDeath()
    {
        isDown = true;
        animator.SetBool("Down", isDown);
        PlayerController playerController = transform.gameObject.GetComponent<PlayerController>();
        playerController.speed = 2;
        // 시간당 playerDown을 줄여나감
        if (playerDown <= 0)
        {
            Dead();
        }
    }

    private void Dead()
    {
        playerEnd = true;
        animator.SetBool("Dead", playerEnd);

    }
}