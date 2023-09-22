using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CampFirePlace : MonoBehaviourPun
{
    public bool isFire = default;   // 산장 안의 벽난로 작동 확인
    // Start is called before the first frame update
    void Start()
    {
        isFire = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
