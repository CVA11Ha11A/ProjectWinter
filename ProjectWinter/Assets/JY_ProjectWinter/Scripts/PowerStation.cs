using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PowerStation : MonoBehaviourPun
{
    public event Action action;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    public void CompleteFixPS()
    {
        GameManager.instance.RepairPowerStation();
    }
}
