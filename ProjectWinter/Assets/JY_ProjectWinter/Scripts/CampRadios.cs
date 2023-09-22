using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class CampRadios : MonoBehaviourPun
{
    public PressEKey pressEKey;
    private bool isOnFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(pressEKey.isComplete && !isOnFlag)
        {
            photonView.RPC("OnCampRadios", RpcTarget.All);
            isOnFlag = true;
        }
    }

    [PunRPC]
    public void OnCampRadios()
    {
        GameManager.instance.CallHeli();
        gameObject.GetComponent<AwakeNoticeCanvas>().enabled = false;
        pressEKey.enabled = false;
    }
}
