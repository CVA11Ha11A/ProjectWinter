using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    private float aniSpeed = 0;

    private float horizontal;
    private float vertical;
    public int speed = 10; 
    private Vector3 moveVec;
    private Animator animator;

    private CameraFollow cameraFollow;
    public GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        //if (photonView.isMine)
        {
            CinemachineVirtualCamera followCam = FindObjectOfType<CinemachineVirtualCamera>();
            followCam.LookAt = transform;

            cameraFollow = player.GetComponent<CameraFollow>();
            cameraFollow.ghostController = this;
            //followCam.LookAt = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        GhostMove();
    }

    private void GhostMove()
    {
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
    }

    // 실내 여부
    #region
    // ###########################
    // isMine일때만 실행
    // ###########################

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Building"))           // 플레이어가 건물 안으로 들어갔으면
        {
            cameraFollow.inside = other.gameObject;
            cameraFollow.isInside = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Building"))
        {
            cameraFollow.isInside = false;
        }
    }
    #endregion
    // 실내 여부
}
