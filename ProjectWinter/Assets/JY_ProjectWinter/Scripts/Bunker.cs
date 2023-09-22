using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;
using DG.Tweening;

public class Bunker : MonoBehaviourPun
{
    public GameObject bunkerButton1Ui;
    public GameObject bunkerButton2Ui;

    public GameObject leftBunkerDoor;
    public GameObject rightBunkerDoor;

    private PressEKey pressEKeyOne;
    private PressEKey pressEKeyTwo;

    public bool isOpenBunker = false;
    // Start is called before the first frame update
    void Start()
    {
        pressEKeyOne = bunkerButton1Ui.GetComponent<PressEKey>();
        pressEKeyTwo = bunkerButton2Ui.GetComponent<PressEKey>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (pressEKeyOne.isComplete && pressEKeyTwo.isComplete && !isOpenBunker)
            {
                photonView.RPC("OpenBunker", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    public void OpenBunker()
    {
        isOpenBunker = true;
        bunkerButton1Ui.SetActive(false);
        bunkerButton2Ui.SetActive(false);
        leftBunkerDoor.transform.DOLocalMoveX(-3f, 1.5f);
        rightBunkerDoor.transform.DOLocalMoveX(3f, 1.5f);
    }

}
