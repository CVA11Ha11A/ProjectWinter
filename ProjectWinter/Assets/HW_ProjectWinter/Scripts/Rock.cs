using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Rock : LivingEntity
{
    void Update()
    {

    }

    public override void Die()
    {
        isDead = true;
        Debug.Log("죽었나?");
        photonView.RPC("DestroyObjectAndInstantiatePrefab", RpcTarget.All);
    }

    [PunRPC]
    public void DestroyObjectAndInstantiatePrefab()
    {

        Debug.Log("꼬마돌 파괴");

        Vector3 rockPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        Quaternion rockRotation = transform.rotation;

        // 현재 게임 오브젝트를 파괴합니다.
        Destroy(gameObject);

        // 파괴된 위치에 프리팹을 인스턴스화합니다.
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject changeRock = PhotonNetwork.Instantiate("RockPiece", rockPosition, rockRotation);
            // 스프라이트의 크기를 조절합니다.
            Vector3 newScale = new Vector3(0.5f, 0.5f, 0.5f); // X, Y, Z 축의 크기를 조절합니다.
            changeRock.transform.localScale = newScale;
        }

    }
}