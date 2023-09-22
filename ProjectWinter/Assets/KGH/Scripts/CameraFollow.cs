using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviourPun
{
    public Transform toFallow;

    private float distance = 8.0f; 
    private float height = 11.0f;

    public bool isInside = false;
    public GameObject inside;

    private Vector3 offset = new Vector3(0.0f, 8.0f, -11.0f);

    public PlayerController playerController;
    public GhostController ghostController;
    private GameObject player;
    private CinemachineVirtualCamera followCam;

    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            followCam = FindObjectOfType<CinemachineVirtualCamera>();
            followCam.LookAt = transform;
            player = transform.gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
            if (ghostController != null)
        {
            player = ghostController.gameObject;
        }
        if (!isInside)
        {
            toFallow = player.transform;
            Vector3 targetPosition = toFallow.position + offset;
            followCam.transform.position = targetPosition;
        }
        else
        {
            Transform cameraObject = inside.transform.Find("Inside Camera");    // Inside Camera = 건물 안에 들어갔을떄 고정시킬 카메라 위치에 둘 오브잭트의 이름
            //GameObject moveCameraHere = cameraObject.gameObject;  // Legacy
            toFallow = cameraObject;

            followCam.transform.position = toFallow.position;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
        { return; }
        if (other.CompareTag("Building"))           // 플레이어가 건물 안으로 들어갔으면
        {
            inside = other.gameObject;
            isInside = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!photonView.IsMine)
        { return; }
        if (other.CompareTag("Building"))
        {            
            isInside = false;
        }
    }
}