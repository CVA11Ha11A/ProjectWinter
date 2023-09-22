using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;

public class HeliPad : MonoBehaviourPun
{
    public GameObject escapePlayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeHeli()
    {
        int playerActorNum = PhotonNetwork.LocalPlayer.ActorNumber;

        foreach (GameObject playerObject in GameManager.instance.playerObjects)
        {
            // 플레이어 오브젝트의 PhotonView 컴포넌트를 가져옴
            PhotonView photonView = playerObject.GetComponent<PhotonView>();

            if (photonView != null)
            {
                // PhotonView가 로컬 플레이어의 것인지 확인
                if (photonView.IsMine)
                {
                    escapePlayer = playerObject;
                }
            }
        }
        photonView.RPC("AddEscapePalyerList", RpcTarget.All, playerActorNum, escapePlayer);

    }

    [PunRPC]
    public void AddEscapePalyerList(int playerActorNum_ , GameObject escapePlayer_)
    {
        GameManager.instance.escapePlayerList.Add(playerActorNum_);
        escapePlayer_.SetActive(false);
    }


}
