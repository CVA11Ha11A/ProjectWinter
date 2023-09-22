using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Tree : LivingEntity
{
    public string targetChildName; // 비활성화할 특정 자식 오브젝트의 이름    

    void Start()
    {

    }

    void Update()
    {

    }

    public override void Die()
    {
        isDead = true;
        Debug.Log("죽었나?");
        photonView.RPC("TreeDestroy", RpcTarget.All);
    }

    [PunRPC]
    public void TreeDestroy()
    {

        // 특정 자식 오브젝트를 찾아서 비활성화합니다.
        Transform targetChild = transform.Find(targetChildName);

        if (targetChild != null)
        {
            targetChild.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("찾으려는 자식 오브젝트를 찾지 못했습니다: " + targetChildName);
        }

        Vector3 treePosition = new Vector3(targetChild.position.x + 3, targetChild.position.y + 3, targetChild.position.z - 2);
        Quaternion treeRotation = targetChild.rotation;


        // 파괴된 위치에 프리팹을 인스턴스화합니다.
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject changeTrees = PhotonNetwork.Instantiate("WoodPiece", treePosition, treeRotation);
            // 스프라이트의 크기를 조절합니다.
            Vector3 newScale = new Vector3(0.5f, 0.5f, 0.5f); // X, Y, Z 축의 크기를 조절합니다.
            changeTrees.transform.localScale = newScale;
        }      
    }
}
