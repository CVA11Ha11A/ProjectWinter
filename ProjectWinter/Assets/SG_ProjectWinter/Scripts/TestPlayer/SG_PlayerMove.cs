using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SG_PlayerMove : MonoBehaviour
{

    private Rigidbody rigid;
    private float moveSpeed = 5f;


    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        // 플레이어의 움직임 입력을 받음
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // 움직임 벡터 계산
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput);
        moveDirection.Normalize(); // 벡터 정규화

        // 플레이어를 움직임
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

}
